import { createContext, useContext } from "react";
import type { CompanyContextType } from "../../models/account";

// Company Context
export const CompanyContext = createContext<CompanyContextType | undefined>(
  undefined,
);

export const useCompany = () => {
  const context = useContext(CompanyContext);
  if (context === undefined) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
};
