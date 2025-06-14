from fastapi import APIRouter, Depends, HTTPException, Header
from services.sales_predictor import SalesPredictor
from repository.database import get_database
from auth import verify_jwt_token

router = APIRouter(dependencies=[Depends(verify_jwt_token)])
predictor = SalesPredictor()

@router.get("/api/predict/sales")
async def predict_sales(months_ahead: int = 1, db=Depends(get_database), X_Company_Id: str = Header(..., alias="X-Company-Id")):
    if(X_Company_Id is None or X_Company_Id == ""):
        raise HTTPException(status_code=403, detail="X-Company-Id header is required")
    
    result = await predictor.predict_sales(db, X_Company_Id, months_ahead=months_ahead)

    if 'error' in result:
        raise HTTPException(status_code=400, detail=result['error'])
    
    return result

@router.get("/api/predict/tax")
async def predict_tax(months_ahead: int = 1, db=Depends(get_database), X_Company_Id: str = Header(..., alias="X-Company-Id")):
    if(X_Company_Id is None or X_Company_Id == ""):
        raise HTTPException(status_code=403, detail="X-Company-Id header is required")
    
    result = await predictor.predict_tax(db, X_Company_Id, months_ahead=months_ahead)

    if 'error' in result:
        raise HTTPException(status_code=400, detail=result['error'])
    
    return result

@router.get("/api/predict/net-sales")
async def predict_net_sales(months_ahead: int = 1, db=Depends(get_database), X_Company_Id: str = Header(..., alias="X-Company-Id")):
    if(X_Company_Id is None or X_Company_Id == ""):
        raise HTTPException(status_code=403, detail="X-Company-Id header is required")
    
    result = await predictor.predict_net_sales(db, X_Company_Id, months_ahead=months_ahead)

    if 'error' in result:
        raise HTTPException(status_code=400, detail=result['error'])
    
    return result

@router.get("/api/predict/product-sales")
async def predict_product_sales(item_id: str, months_ahead: int = 1, db=Depends(get_database), X_Company_Id: str = Header(..., alias="X-Company-Id")):
    if(X_Company_Id is None or X_Company_Id == ""):
        raise HTTPException(status_code=403, detail="X-Company-Id header is required")
    
    result = await predictor.predict_product_sales(db, X_Company_Id, item_id, months_ahead=months_ahead)

    if 'error' in result:
        raise HTTPException(status_code=400, detail=result['error'])
    
    return result
