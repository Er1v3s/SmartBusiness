import { TransactionContext } from "./TransactionContext.tsx";
import apiWriteConnector from "../../api/apiWriteConnector.ts";
import apiReadConnector from "../../api/apiReadConnector.ts";
import type {
  GetTransactionByParamsQuery,
  Transaction,
  TransactionContextType,
  NewTransaction,
} from "../../models/transaction.ts";

export const TransactionProvider: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {
  const createTransaction = async (data: NewTransaction): Promise<void> => {
    await apiWriteConnector.createTransaction(data);
  };

  const updateTransaction = async (
    data: Partial<Transaction>,
  ): Promise<void> => {
    await apiWriteConnector.updateTransaction(data);
  };

  const deleteTransaction = async (serviceId: string): Promise<void> => {
    await apiWriteConnector.deleteTransaction(serviceId);
  };

  const fetchTransaction = async (
    serviceId: string,
  ): Promise<Transaction | null> => {
    const service = await apiReadConnector.getTransactionById(serviceId);
    return service;
  };

  const fetchTransactions = async (
    params: GetTransactionByParamsQuery,
  ): Promise<Transaction[]> => {
    return await apiReadConnector.getTransactions(params);
  };

  // Context value
  const value: TransactionContextType = {
    createTransaction,
    updateTransaction,
    deleteTransaction,
    fetchTransaction,
    fetchTransactions,
  };

  return (
    <TransactionContext.Provider value={value}>
      {children}
    </TransactionContext.Provider>
  );
};
