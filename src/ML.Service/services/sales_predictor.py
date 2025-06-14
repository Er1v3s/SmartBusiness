import numpy as np
import pandas as pd
from datetime import datetime
from typing import Optional
from prophet import Prophet
from sklearn.metrics import r2_score, mean_absolute_error
from repository.transaction_repository import get_transactions_from_db

class SalesPredictor:
    def __init__(self):
        # Modele trenowane osobno dla każdej firmy
        self.company_models = {}  # company_id: {model_total, model_tax, model_net, metrics_total, metrics_tax, metrics_net, last_train_time}

    async def fetch_data(self, db, company_id: str):
        data = await get_transactions_from_db(db, company_id)
        df = pd.DataFrame(data)
        if df.empty:
            return df
        
        # Convert 'timestamp' column to datetime
        try:
            df['timestamp'] = pd.to_datetime(df['timestamp'], format='mixed', errors='coerce')
        except TypeError:
            df['timestamp'] = pd.to_datetime(df['timestamp'], errors='coerce')

        df = df.dropna(subset=['timestamp'])

        # Delete first month if it is not full (because it will introduce anomalies)
        if not df.empty:
            first_month = df['timestamp'].dt.to_period('M').min()
            mask_full_month = df['timestamp'].dt.to_period('M') != first_month
            df = df[mask_full_month]

        # Delete current month if it is not full (because it will introduce anomalies)
        now = pd.Timestamp.now()
        current_month = now.to_period('M')
        df = df[df['timestamp'].dt.to_period('M') < current_month]

        # Convert 'total_amount', 'tax' and 'total_amount_minus_tax' to float
        df['total_amount'] = df['total_amount'].astype(float)
        df['tax'] = df['tax'].astype(float)

        return df

    def prepare_prophet_df(self, df: pd.DataFrame, target: str, item_id: Optional[str] = None):
        # Check if 'timestamp' column exists
        if item_id:
            df = df[df['item_id'] == item_id]
        if df.empty:
            return None
        
        # Sprawd�, czy kolumna 'timestamp' jest w formacie datetime
        df['month'] = df['timestamp'].dt.to_period('M').dt.to_timestamp()
        prophet_df = df.groupby('month')[target].sum().reset_index()
        prophet_df = prophet_df.rename(columns={'month': 'ds', target: 'y'})

        return prophet_df

    def train_prophet(self, prophet_df):
        model = Prophet()
        model.fit(prophet_df)
        return model

    def prophet_predict(self, model, periods, last_date):
        # Ensure last_date is the first day of the month
        last_month_start = pd.Timestamp(year=last_date.year, month=last_date.month, day=1)

        # periods = max(periods, 1)  # Ensure at least one period
        future = pd.date_range(start=last_month_start + pd.offsets.MonthBegin(1), periods=periods, freq='MS')
        future_df = pd.DataFrame({'ds': future})
        forecast = model.predict(future_df)
        preds = []
        for i in range(periods):
            preds.append({
                'target_month': future[i].strftime('%Y-%m'),
                'prediction': round(float(forecast.iloc[i]['yhat']), 2)
            })
        return preds

    async def train(self, db, company_id: str):
        df = await self.fetch_data(db, company_id)
        if df.empty:
            return None
        # Prophet for total sales
        prophet_df = self.prepare_prophet_df(df, 'total_amount')
        if prophet_df is not None and len(prophet_df) >= 3:
            model_total = self.train_prophet(prophet_df)
            y_true = prophet_df['y'].values
            y_pred = model_total.predict(prophet_df[['ds']])['yhat'].values
            r2 = r2_score(y_true, y_pred)
            mae = mean_absolute_error(y_true, y_pred)
            metrics_total = {'model': 'Prophet', 'r2': r2, 'mae': mae}
        else:
            model_total = None
            metrics_total = {}
        # Prophet for tax
        prophet_df_tax = self.prepare_prophet_df(df, 'tax')
        if prophet_df_tax is not None and len(prophet_df_tax) >= 3:
            model_tax = self.train_prophet(prophet_df_tax)
            y_true = prophet_df_tax['y'].values
            y_pred = model_tax.predict(prophet_df_tax[['ds']])['yhat'].values
            r2 = r2_score(y_true, y_pred)
            mae = mean_absolute_error(y_true, y_pred)
            metrics_tax = {'model': 'Prophet', 'r2': r2, 'mae': mae}
        else:
            model_tax = None
            metrics_tax = {}
        # Prophet for net sales
        prophet_df_net = self.prepare_prophet_df(df, 'total_amount_minus_tax')
        if prophet_df_net is not None and len(prophet_df_net) >= 3:
            model_net = self.train_prophet(prophet_df_net)
            y_true = prophet_df_net['y'].values
            y_pred = model_net.predict(prophet_df_net[['ds']])['yhat'].values
            r2 = r2_score(y_true, y_pred)
            mae = mean_absolute_error(y_true, y_pred)
            metrics_net = {'model': 'Prophet', 'r2': r2, 'mae': mae}
        else:
            model_net = None
            metrics_net = {}
        last_train_time = datetime.now().isoformat()
        self.company_models[company_id] = {
            'model_total': model_total,
            'model_tax': model_tax,
            'model_net': model_net,
            'metrics_total': metrics_total,
            'metrics_tax': metrics_tax,
            'metrics_net': metrics_net,
            'last_train_time': last_train_time
        }
        return self.company_models[company_id]

    async def predict_sales(self, db, company_id, months_ahead=1):
        if company_id not in self.company_models:
            await self.train(db, company_id)
        models = self.company_models.get(company_id)
        if not models or not models['model_total']:
            return {'error': 'Brak danych do predykcji'}
        df = await self.fetch_data(db, company_id)
        prophet_df = self.prepare_prophet_df(df, 'total_amount')
        if prophet_df is None:
            return {'error': 'Brak danych do predykcji'}
        last_date = prophet_df['ds'].max()
        preds = self.prophet_predict(models['model_total'], months_ahead, last_date)
        return {
            'predictions': preds,
            'metrics': models['metrics_total']
        }

    async def predict_tax(self, db, company_id, months_ahead=1):
        if company_id not in self.company_models:
            await self.train(db, company_id)
        models = self.company_models.get(company_id)
        if not models or not models['model_tax']:
            return {'error': 'Brak danych do predykcji'}
        df = await self.fetch_data(db, company_id)
        prophet_df = self.prepare_prophet_df(df, 'tax')
        if prophet_df is None:
            return {'error': 'Brak danych do predykcji'}
        last_date = prophet_df['ds'].max()
        preds = self.prophet_predict(models['model_tax'], months_ahead, last_date)
        return {
            'predictions': preds,
            'metrics': models['metrics_tax']
        }

    async def predict_net_sales(self, db, company_id, months_ahead=1):
        if company_id not in self.company_models:
            await self.train(db, company_id)
        models = self.company_models.get(company_id)
        if not models or not models['model_net']:
            return {'error': 'Brak danych do predykcji'}
        df = await self.fetch_data(db, company_id)
        prophet_df = self.prepare_prophet_df(df, 'total_amount_minus_tax')
        if prophet_df is None:
            return {'error': 'Brak danych do predykcji'}
        last_date = prophet_df['ds'].max()
        preds = self.prophet_predict(models['model_net'], months_ahead, last_date)
        return {
            'predictions': preds,
            'metrics': models['metrics_net']
        }

    async def predict_product_sales(self, db, company_id, item_id: str, months_ahead=1):
        if company_id not in self.company_models:
            await self.train(db, company_id)

        df = await self.fetch_data(db, company_id)
        
        if df.empty:
            return {'error': 'Brak danych dla tego produktu'}
        
        df = df[df['item_id'] == item_id]

        if df.empty:
            return {'error': 'Brak danych dla tego produktu'}
        
        df['month'] = df['timestamp'].dt.to_period('M').dt.to_timestamp()
        prophet_df = df.groupby('month').size().reset_index(name='y')
        prophet_df = prophet_df.rename(columns={'month': 'ds'})

        if len(prophet_df) < 3:
            return {'error': 'Za mało danych do predykcji'}
        
        model = self.train_prophet(prophet_df)
        y_true = prophet_df['y'].values
        y_pred = model.predict(prophet_df[['ds']])['yhat'].values
        metrics = {'model': 'Prophet', 'r2': r2_score(y_true, y_pred), 'mae': mean_absolute_error(y_true, y_pred)}
        last_date = prophet_df['ds'].max()
        preds = self.prophet_predict(model, months_ahead, last_date)

        return {
            'predictions': preds,
            'metrics': metrics
        }
