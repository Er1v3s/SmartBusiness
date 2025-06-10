import { axiosWrite } from "./axiosInstance";
import type { Transaction } from "../models/transaction";

const apiWriteConnector = {
    
  createTransaction: async (data: Transaction): Promise<void> => {
    await axiosWrite.post("/api/write/transactions", data);
  },

  updateTransaction: async (id: string, data: Partial<Transaction>): Promise<void> => {
    await axiosWrite.put(`/api/write/transactions/${id}`, data);
  },

  deleteTransaction: async (id: string): Promise<void> => {
    await axiosWrite.delete(`/api/write/transactions/${id}`);
  },

};

export default apiWriteConnector;
