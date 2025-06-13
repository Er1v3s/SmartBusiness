export interface SalesPrediction {
  target_month: string;
  prediction: number;
}
export interface PredictionMetrics {
  model: string;
  r2: number;
  mae: number;
}
export interface SalesPredictionResponse {
  predictions: SalesPrediction[];
  metrics: PredictionMetrics;
}

export interface PredictionContextType {
  fetchSalesPrediction: (monthsAhead?: number) => Promise<SalesPredictionResponse>;
  fetchTaxPrediction: (monthsAhead?: number) => Promise<SalesPredictionResponse>;
  fetchNetSalesPrediction: (monthsAhead?: number) => Promise<SalesPredictionResponse>;
  fetchServiceSalesPrediction: (serviceId: string, monthsAhead: number) => Promise<SalesPredictionResponse>;
}

export interface GetPredictionByParamsQuery {
  userId?: string;
  productId?: string;
}