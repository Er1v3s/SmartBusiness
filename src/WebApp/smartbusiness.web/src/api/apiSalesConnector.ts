import { axiosSales } from "./axiosInstance";
import type { GetProductsByParamsQuery, Product, NewProduct } from "../models/product";
import type { Service, GetServiceByParamsQuery, NewService } from "../models/service";

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
  createService: async (data: NewService): Promise<void> => {
    await axiosSales.post("/services", data);
  },

  updateService: async (data: Partial<Service>): Promise<void> => {
    const { id, name, description, category, price, tax, duration } = data;
    await axiosSales.put(`/services/${id}`, {
      name,
      description,
      category,
      price,
      tax,
      duration,
    });
  },

  deleteService: async (id: string): Promise<void> => {
    await axiosSales.delete(`/services/${id}`);
  },

  getServiceById: async (id: string): Promise<Service> => {
    const response = await axiosSales.get(`/services/${id}`);

    return response.data;
  },

  getServices: async (params: GetServiceByParamsQuery): Promise<Service[]> => {
    const response = await axiosSales.get<Service[]>("/services", { params });

    return response.data;
  },

};

export default apiSalesConnector;
