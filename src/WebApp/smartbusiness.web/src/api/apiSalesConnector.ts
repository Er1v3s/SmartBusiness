import { axiosSales } from "./axiosInstance";
import type { GetProductsByParamsQuery, Product, Service, NewProduct } from "../models/index";

const apiSalesConnector = {

  // PRODUCTS
  createProduct: async (data: NewProduct): Promise<void> => {
    await axiosSales.post("/products", data);
  },

  updateProduct: async (data: Partial<Product>): Promise<void> => {
    const { id, name, description, category, price, tax } = data;
    await axiosSales.put(`/products/${id}`, {
      name,
      description,
      category,
      price,
      tax,
    });
  },

  deleteProduct: async (id: string): Promise<void> => {
    await axiosSales.delete(`/products/${id}`);
  },

  getProductById: async (id: string): Promise<Product> => {
    const response = await axiosSales.get(`/products/${id}`);

    return response.data;
  },

  getProducts: async (params: GetProductsByParamsQuery): Promise<Product[]> => {
    const response = await axiosSales.get<Product[]>("/products", { params });

    return response.data;
  },

  // SERVICES
  getServiceById: async (id: string): Promise<Service> => {
    const response = await axiosSales.get(`/products/${id}`);

    return response.data;
  },

  getServices: async (): Promise<Service[]> => {
    const response = await axiosSales.get("/services");

    return response.data;
  },

  createService: async (data: Service): Promise<void> => {
    await axiosSales.post("/services", data);
  },

  updateService: async (id: string, data: Partial<Service>): Promise<void> => {
    await axiosSales.put(`/services/${id}`, data);
  },

  deleteService: async (id: string): Promise<void> => {
    await axiosSales.delete(`/services/${id}`);
  },
};

export default apiSalesConnector;
