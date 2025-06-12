import { axiosRead, axiosSales } from "./axiosInstance";
import type {
  EnrichedTransaction,
  GetTransactionByParamsQuery,
  Transaction
} from "../models/transaction";
import type { Service } from "../models/service";
import type { Product } from "../models/product";

const apiReadConnector = {

  getTransactionById: async (id: string): Promise<Transaction> => {
    const response = await axiosRead.get(`/read/transactions/${id}`);

    return response.data;
  },

  getTransactions: async (params: GetTransactionByParamsQuery): Promise<Transaction[]> => {
    const response = await axiosRead.get("/read/transactions", { params });
    
    return response.data;
  },

  getEnrichtedTransactions: async (params: GetTransactionByParamsQuery): Promise<EnrichedTransaction[]> => {
    const [transactionsRes, productsRes, servicesRes] = await Promise.all([
      axiosRead.get<Transaction[]>("/read/transactions", { params }),
      axiosSales.get<Product[]>("/products"),
      axiosSales.get<Service[]>("/services")
    ]);

    const transactions = transactionsRes.data;
    const products = productsRes.data;
    const services = servicesRes.data;

    const productMap = new Map(products.map(p => [p.id, p.name]));
    const serviceMap = new Map(services.map(s => [s.id, s.name]));

    const enrichedTransactions = transactions.map(tx => {
      const name =
          tx.itemType === "product"
              ? productMap.get(tx.itemId)
              : serviceMap.get(tx.itemId);

      return {
        ...tx,
        itemName: name ?? "Nieznana usługa/produkt",
      };
    });

    return enrichedTransactions;
  },

};

export default apiReadConnector;