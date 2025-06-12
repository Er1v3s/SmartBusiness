export interface Transaction {
  id: string;
  companyId: string;
  userId: string;
  itemId: string;
  itemType: "product" | "service";
  quantity: number;
  totalAmount: number;
  tax: number;
  totalAmountMinusTax: number;
  createdAt: string; // ISO string (DateTime)
}

export type NewTransaction = Omit<Transaction, "id" | "userId" | "companyId" | "totalAmountMinusTax" | "createdAt">;

export interface TransactionContextType {
  createTransaction: (data: NewTransaction) => Promise<void>;
  updateTransaction: (data: Partial<Transaction>) => Promise<void>;
  deleteTransaction: (transactionId: string) => Promise<void>;
  fetchTransaction: (transactionId: string) => Promise<Transaction | null>;
  fetchTransactions: (
    params: GetTransactionByParamsQuery,
  ) => Promise<Transaction[]>;
  fetchEnrichtedTransactions: (
    params: GetTransactionByParamsQuery
  ) => Promise<EnrichedTransaction[]>;
}

export interface GetTransactionByParamsQuery {
  userId?: string;
  productId?: string;
  Quantity?: number;
  minTotalAmount?: number;
  maxTotalAmount?: number;
  minTax?: number;
  maxTax?: number;
  minTotalAmountMinusTax?: number;
  maxTotalAmountMinusTax?: number;
  startDateTime?: string; // ISO string (DateTime)
  endDateTime?: string; // ISO string (DateTime)
  numberOfTransactions?: number;
}

export interface EnrichedTransaction extends Transaction {
  itemName: string;
  }