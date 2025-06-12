import { axiosWrite } from "./axiosInstance";
import type { NewTransaction, Transaction } from "../models/transaction";

const apiWriteConnector = {
    
  createTransaction: async (data: NewTransaction): Promise<void> => {
    await axiosWrite.post("write/transactions", data);
  },

  updateTransaction: async (data: Partial<Transaction>): Promise<void> => {
    const { id, itemId, itemType, quantity, totalAmount, tax  } = data;
    await axiosWrite.put(`write/transactions/${id}`, {
      itemId,
      itemType,
      quantity,
      totalAmount,
      tax,
      });
  },

  deleteTransaction: async (id: string): Promise<void> => {
    await axiosWrite.delete(`write/transactions/${id}`);
  },

};

export default apiWriteConnector;
