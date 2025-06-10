import { ServiceContext } from "./ServiceContext.tsx";
import apiSalesConnector from "../../api/apiSalesConnector.ts";
import type {
  GetServiceByParamsQuery,
  Service,
  ServiceContextType,
  NewService,
} from "../../models/service.ts";

export const ServiceProvider: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {
  const createService = async (data: NewService): Promise<void> => {
    await apiSalesConnector.createService(data);
  };

  const updateService = async (data: Partial<Service>): Promise<void> => {
    await apiSalesConnector.updateService(data);
  };

  const deleteService = async (serviceId: string): Promise<void> => {
    await apiSalesConnector.deleteService(serviceId);
  };

  const fetchService = async (serviceId: string): Promise<Service | null> => {
    const service = await apiSalesConnector.getServiceById(serviceId);
    return service;
  };

  const fetchServices = async (
    params: GetServiceByParamsQuery,
  ): Promise<Service[]> => {
    return await apiSalesConnector.getServices(params);
  };

  // Context value
  const value: ServiceContextType = {
    createService,
    updateService,
    deleteService,
    fetchService,
    fetchServices,
  };

  return (
    <ServiceContext.Provider value={value}>{children}</ServiceContext.Provider>
  );
};
