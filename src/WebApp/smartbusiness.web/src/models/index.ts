// Types
export interface User {
  id: string;
  username: string;
  email: string;
  createdAt?: string; // Optional, can be used for user creation date
}

export interface Company {
  id: string;
  name: string;
  createdAt?: string; // Optional, can be used for company creation date
}

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

export interface Service {
  id: string;
  companyId: string;
  name: string;
  description: string;
  category: string;
  price: number;
  tax: number;
  createdAt: string; // ISO string (DateTime)
  duration?: number | null;
}

export type NewProduct = Omit<Product, "id" | "companyId" | "createdAt">;
export type NewService = Omit<Service, "id" | "companyId" | "createdAt">;

export interface CompanyContextType {
  company: Company | null;
  companies: Company[];
  fetchCompanies: () => Promise<void>;
  setActiveCompany: (companyId: string) => Promise<void> | void;
  fetchCompanyData: (companyId: string) => Promise<Company>;
  createCompany: (name: string) => Promise<void>;
  updateCompany: (name: string) => Promise<void>;
  deleteCompany: () => Promise<void>;
  isCompanySet: boolean;
}

export interface AuthContextType {
  user: User | null;
  login: (email: string, password: string, rememberMe: boolean) => Promise<void>;
  register: (username: string, email: string, password: string) => Promise<void>;
  fetchUserData: () => Promise<void>;
  logout: () => void;
  sendResetLink: (email: string) => Promise<void>;
  resetPassword: (token: string, password: string) => Promise<void>;
  isAuthenticated: boolean;
  // token: string | null;
}

export interface AccountContextType {
  updateAccount: (username: string, email: string) => Promise<void>;
  changePassword: (currentPassword: string, newPassword: string) => Promise<void>;
  deleteAccount: (password: string) => Promise<void>;
}

export interface ProductContextType {
  createProduct: (product: NewProduct) => Promise<void>;
  updateProduct: (product: Partial<Product>) => Promise<void>;
  deleteProduct: (productId: string) => Promise<void>;
  fetchProduct: (productId: string) => Promise<Product | null>;
  fetchProducts: (params: GetProductsByParamsQuery) => Promise<Product[]>;
}

export interface ServiceContextType {
  createService: (service: NewService) => Promise<void>;
  updateService: (service: Partial<Service>) => Promise<void>;
  deleteService: (serviceId: string) => Promise<void>;
  fetchService: (serviceId: string) => Promise<Service | null>;
  fetchServices: (params: GetServiceByParamsQuery) => Promise<Service[]>;
}

export interface GetProductsByParamsQuery {
  Name?: string;
  Category?: string;
  MinPrice?: number;
  MaxPrice?: number;
}

export interface GetServiceByParamsQuery {
  Name?: string;
  Category?: string;
  MinPrice?: number;
  MaxPrice?: number;
  MinDuration?: number;
  MaxDuration?: number;
}

export interface LoginForm {
  email: string;
  password: string;
  rememberMe: boolean;
}

export interface RegisterForm {
  username: string;
  email: string;
  password: string;
}