export interface Product {
  id: string;
  companyId: string;
  name: string;
  description: string;
  category: string;
  price: number;
  tax: number;
  createdAt: string; // ISO string (DateTime)
}

export type NewProduct = Omit<Product, "id" | "companyId" | "createdAt">;

export interface ProductContextType {
  createProduct: (product: NewProduct) => Promise<void>;
  updateProduct: (product: Partial<Product>) => Promise<void>;
  deleteProduct: (productId: string) => Promise<void>;
  fetchProduct: (productId: string) => Promise<Product | null>;
  fetchProducts: (params: GetProductsByParamsQuery) => Promise<Product[]>;
}

export interface GetProductsByParamsQuery {
  Name?: string;
  Category?: string;
  MinPrice?: number;
  MaxPrice?: number;
}