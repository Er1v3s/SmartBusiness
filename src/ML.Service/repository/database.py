import os
from motor.motor_asyncio import AsyncIOMotorClient
from dotenv import load_dotenv

load_dotenv()


class Database:
    client: AsyncIOMotorClient = None
    database = None


database = Database()


async def get_database():
    return database.database


async def connect_to_mongo():
    mongo_url = os.getenv("MONGODB_URL")
    database_name = os.getenv("DATABASE_NAME")

    database.client = AsyncIOMotorClient(mongo_url)
    database.database = database.client[database_name]

    print(f"Connected to MongoDB... {database_name}")


async def close_mongo_connection():
    if database.client:
        database.client.close()
        print("Closing MongoDB connection...")