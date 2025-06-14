# LIBRARIES
from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from repository.database import connect_to_mongo, close_mongo_connection
from api.sales import router as sales_router
from api.health import router as health

app = FastAPI()

# COSR Configuration FOR React Frontend
app.add_middleware(
    CORSMiddleware,
    allow_origins=["http://localhost:5000"],  # Dodaj porty React
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

@app.on_event("startup")
async def startup_event():
    await connect_to_mongo()
    print("ML Service has been started!")

@app.on_event("shutdown")
async def shutdown_event():
    await close_mongo_connection()

app.include_router(sales_router)
app.include_router(health)

if __name__ == "__main__":
    import uvicorn
    uvicorn.run(app, host="0.0.0.0", port=2500)