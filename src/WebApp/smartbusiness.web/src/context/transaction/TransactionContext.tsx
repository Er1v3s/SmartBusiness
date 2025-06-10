import { createContext, useContext } from "react";
import type { TransactionContextType } from "../../models/transaction.ts";

export const TransactionContext = createContext<
  TransactionContextType | undefined
>(undefined);

export const useTransaction = () => {
  const context = useContext(TransactionContext);
  if (context === undefined) {
    throw new Error("useTransaction must be used within a ServiceProvider");
  }
  return context;
};
