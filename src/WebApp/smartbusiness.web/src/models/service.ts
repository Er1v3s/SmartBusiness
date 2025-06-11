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

export type NewService = Omit<Service, "id" | "companyId" | "createdAt">;

export interface ServiceContextType {
  createService: (service: NewService) => Promise<void>;
  updateService: (service: Partial<Service>) => Promise<void>;
  deleteService: (serviceId: string) => Promise<void>;
  fetchService: (serviceId: string) => Promise<Service | null>;
  fetchServices: (params: GetServiceByParamsQuery) => Promise<Service[]>;
}

export interface GetServiceByParamsQuery {
  Name?: string;
  Category?: string;
  MinPrice?: number;
  MaxPrice?: number;
  MinDuration?: number;
  MaxDuration?: number;
}