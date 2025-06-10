import { axiosRead } from "./axiosInstance";
import type { Transaction } from "../models/transaction";

const apiReadConnector = {

  getTransactionById: async (id: string): Promise<Transaction> => {
    const response = await axiosRead.get(`/read/transactions/${id}`);

    return response.data;
  },

  getTransactions: async (): Promise<Transaction[]> => {
    const response = await axiosRead.get("/read/transactions");
    
    return response.data;
  },

};

export default apiReadConnector;
