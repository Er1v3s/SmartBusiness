import { createContext, useContext } from "react";
import type { ServiceContextType } from "../../models/service.ts";

export const ServiceContext = createContext<ServiceContextType | undefined>(
  undefined,
);

export const useService = () => {
  const context = useContext(ServiceContext);
  if (context === undefined) {
    throw new Error("useService must be used within a ServiceProvider");
  }
  return context;
};
