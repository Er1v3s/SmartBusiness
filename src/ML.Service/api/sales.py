from fastapi import APIRouter, Depends, HTTPException, Header
from services.sales_predictor import SalesPredictor
from repository.database import get_database

router = APIRouter()
predictor = SalesPredictor()

@router.post("/predict/sales")
async def predict_sales(months_ahead: int = 1, db=Depends(get_database), X_Company_Id: str = Header(..., alias="X-Company-Id")):
    result = await predictor.predict_sales(db, X_Company_Id, months_ahead=months_ahead)
    if 'error' in result:
        raise HTTPException(status_code=400, detail=result['error'])
    return result

@router.post("/predict/tax")
async def predict_tax(months_ahead: int = 1, db=Depends(get_database), X_Company_Id: str = Header(..., alias="X-Company-Id")):
    result = await predictor.predict_tax(db, X_Company_Id, months_ahead=months_ahead)
    if 'error' in result:
        raise HTTPException(status_code=400, detail=result['error'])
    return result

@router.post("/predict/net_sales")
async def predict_net_sales(months_ahead: int = 1, db=Depends(get_database), X_Company_Id: str = Header(..., alias="X-Company-Id")):
    result = await predictor.predict_net_sales(db, X_Company_Id, months_ahead=months_ahead)
    if 'error' in result:
        raise HTTPException(status_code=400, detail=result['error'])
    return result

@router.post("/predict/product_sales")
async def predict_product_sales(item_id: str, months_ahead: int = 1, db=Depends(get_database), X_Company_Id: str = Header(..., alias="X-Company-Id")):
    result = await predictor.predict_product_sales(db, X_Company_Id, item_id, months_ahead=months_ahead)
    if 'error' in result:
        raise HTTPException(status_code=400, detail=result['error'])
    return result
