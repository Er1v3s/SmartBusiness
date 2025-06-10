export interface Transaction {
  id: string;
  companyId: string;
  userId: string;
  productId: string;
  quantity: number;
  totalAmount: number;
  tax: number;
  totalAmountMinusTax: number;
  createdAt: string; // ISO string (DateTime)
}

