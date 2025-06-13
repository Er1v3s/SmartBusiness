from motor.motor_asyncio import AsyncIOMotorClient

async def get_transactions_from_db(db: AsyncIOMotorClient, company_id: str ):
    query = { "companyId" : {company_id} , "itemType": {"$in": ["service"]} }

    cursor = db.transactions.find(query).sort("timestamp", 1)

    transactions = []
    async for transaction in cursor:
        transactions.append({
            "id": str(transaction["_id"]),
            "company_id": transaction.get("companyId"),
            "user_id": transaction.get("userId"),
            "item_id": transaction.get("itemId"),
            "item_type": transaction.get("itemType"),
            "quantity": transaction.get("quantity"),
            "total_amount": float(transaction["totalAmount"].to_decimal()),
            "tax": float(transaction["tax"]),
            "total_amount_minus_tax": float(transaction["TotalAmountMinusTax"].to_decimal()),
            "timestamp": transaction.get("timestamp").isoformat()
        })

    return transactions