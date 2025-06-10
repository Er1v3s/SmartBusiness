import { createContext, useContext } from "react";
import type { ServiceContextType } from "../../models";

export const ServiceContext = createContext<ServiceContextType | undefined>(
  undefined,
);

export const useService = () => {
  const context = useContext(ServiceContext);
  if (context === undefined) {
    throw new Error("useProduct must be used within a ProductProvider");
  }
  return context;
};
