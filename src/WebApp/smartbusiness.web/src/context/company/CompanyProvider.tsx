import { useState, useEffect, useCallback } from "react";
import type { Company, CompanyContextType } from "../../models/index.ts";
import apiConnector from "../../api/apiConnector.ts";
import { CompanyContext } from "./CompanyContext.tsx";

export const CompanyProvider: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {
  const [companies, setCompanies] = useState<Company[]>([]);
  const [company, setCompany] = useState<Company | null>(null);

  // Pobierz wszystkie firmy użytkownika
  const fetchCompanies = useCallback(async () => {
    try {
      const data = await apiConnector.getCompanies();
      setCompanies(data);
      if (!company && data.length > 0) setCompany(data[0]);
    } catch (error) {
      console.error("Failed to fetch companies:", error);
      // W przypadku błędu ustaw firmę na null
      setCompanies([]);
      setCompany(null);
    }
  }, [company]);

  // Ustaw aktywną firmę po id
  const setActiveCompany = async (companyId: string) => {
    const found = companies.find((c) => c.id === companyId);
    if (found) setCompany(found);
    // Jeśli chcesz pobrać szczegóły firmy z API:
    // const detailed = await apiConnector.getCompany(companyId);
    // setCompany(detailed);
  };

  useEffect(() => {
    fetchCompanies();
  }, [fetchCompanies]);

  const fetchCompanyData = async (companyId: string): Promise<Company> => {
    try {
      const companyData = await apiConnector.getCompany(companyId);
      setCompany(companyData);
      return companyData;
    } catch (error) {
      setCompany(null);
      throw error;
    }
  };

  const createCompany = async (name: string) => {
    try {
      await apiConnector.createCompany(name);
      await fetchCompanies();
      // Po utworzeniu nowej firmy ustaw ją jako aktywną (ostatnia na liście)
      const updated = await apiConnector.getCompanies();
      if (updated.length > 0) setCompany(updated[updated.length - 1]);
    } catch (error) {
      console.error("Failed to create company:", error);
    }
  };

  const updateCompany = async (name: string) => {
    try {
      if (!company) throw new Error("No company to update");
      await apiConnector.updateCompany(company.id, name);
      await fetchCompanies();
    } catch (error) {
      console.error("Failed to update company:", error);
    }
  };

  const deleteCompany = async () => {
    try {
      if (!company) throw new Error("No company to delete");
      await apiConnector.deleteCompany(company.id);
      await fetchCompanies();
      // Po usunięciu firmy ustaw pierwszą z listy jako aktywną (lub null jeśli brak)
      const updated = await apiConnector.getCompanies();
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
    isCompanySet: company !== null,
  };

  return (
    <CompanyContext.Provider value={value}>{children}</CompanyContext.Provider>
  );
};
