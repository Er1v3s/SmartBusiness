import { axiosRead } from "./axiosInstance";
import type { GetTransactionByParamsQuery, Transaction } from "../models/transaction";

const apiReadConnector = {

  getTransactionById: async (id: string): Promise<Transaction> => {
    const response = await axiosRead.get(`/read/transactions/${id}`);

    return response.data;
  },

  getTransactions: async (params: GetTransactionByParamsQuery): Promise<Transaction[]> => {
    const response = await axiosRead.get("/read/transactions", { params });
    
    return response.data;
  },

};

export default apiReadConnector;
