import { useState, useEffect, useCallback } from "react";
import type { Company, CompanyContextType } from "../../models/account.ts";
import apiAccountConnector from "../../api/apiAccountConnector.ts";
import { CompanyContext } from "./CompanyContext.tsx";
import { useAuth } from "../auth/AuthContext";

export const CompanyProvider: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {
  const [companies, setCompanies] = useState<Company[]>([]);
  const [company, setCompany] = useState<Company | null>(null);
  const { user } = useAuth();

  const fetchCompanies = useCallback(async () => {
    try {
      const data = await apiAccountConnector.getCompanies();
      setCompanies(data);

      const storedId = localStorage.getItem("COMPANY_ID");
      if (storedId) {
        const found = data.find((c) => c.id === storedId);
        if (found) {
          setCompany(found);
          return;
        }
      }

      setCompany(null);
    } catch (error) {
      console.error("Failed to fetch companies:", error);

      setCompanies([]);
      setCompany(null);
    }
  }, []);

  // Fetch companies on initial render OR when user changes
  useEffect(() => {
    if (user) {
      fetchCompanies();
    } else {
      setCompanies([]);
      setCompany(null);
    }
  }, [fetchCompanies, user]);

  // Usuwaj COMPANY_ID jeśli nie ma ACCESS_TOKEN
  useEffect(() => {
    const accessToken = localStorage.getItem("ACCESS_TOKEN");
    if (!accessToken) {
      localStorage.removeItem("COMPANY_ID");
      setCompany(null);
    }
  }, []);

  const setActiveCompany = async (companyId: string) => {
    const found = companies.find((c) => c.id === companyId);

    if (found) {
      setCompany(found);
      localStorage.setItem("COMPANY_ID", found.id);
    }
  };

  const fetchCompanyData = async (companyId: string): Promise<Company> => {
    try {
      const companyData = await apiAccountConnector.getCompany(companyId);
      setCompany(companyData);

      return companyData;
    } catch (error) {
      setCompany(null);

      throw error;
    }
  };

  const createCompany = async (name: string) => {
    try {
      await apiAccountConnector.createCompany(name);
      await fetchCompanies();

      // After creating a new company, we fetch the list again to ensure we have the latest data
      const updated = await apiAccountConnector.getCompanies();
      if (updated.length > 0) {
        setCompany(updated[updated.length - 1]);
      }
    } catch (error) {
      console.error("Failed to create company:", error);
    }
  };

  const updateCompany = async (name: string) => {
    try {
      if (!company) {
        throw new Error("No company to update");
      }

      await apiAccountConnector.updateCompany(company.id, name);
      await fetchCompanies();
    } catch (error) {
      console.error("Failed to update company:", error);
    }
  };

  const deleteCompany = async (password: string) => {
    try {
      if (!company) throw new Error("No company to delete");
      await apiAccountConnector.deleteCompany(company.id, password);

      await fetchCompanies();
      // After deleting a company, we fetch the list again to ensure we have the latest data
      const updated = await apiAccountConnector.getCompanies();
      setCompany(updated.length > 0 ? updated[0] : null);
    } catch (error) {
      console.error("Failed to delete company:", error);
    }
  };

  const value: CompanyContextType = {
    company,
    companies,
    fetchCompanies,
    setActiveCompany,
    fetchCompanyData,
    createCompany,
    updateCompany,
    deleteCompany,
  };

  return (
    <CompanyContext.Provider value={value}>{children}</CompanyContext.Provider>
  );
};
