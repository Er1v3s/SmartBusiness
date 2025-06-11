import React from "react";
import { Outlet } from "react-router-dom";
import { CompanySidebarComponent } from "../../components/Dashboard/Company/CompanySettings/CompanySidebarComponent";
import { useCompany } from "../../context/company/CompanyContext";

export const CompanyPage: React.FC = () => {
  const { company } = useCompany();

  return (
    <div className="flex h-[calc(100vh-(var(--spacing)*16))] bg-gradient-to-br from-gray-100 to-gray-300 dark:from-gray-800 dark:to-gray-700">
      {/* Sidebar */}
      <CompanySidebarComponent />

      {/* Main content */}
      <main className="flex flex-1 items-start justify-center bg-transparent px-4 py-12 text-gray-700 dark:text-gray-100">
        <div className="w-full max-w-2xl">
          <Outlet context={{ company }} />
        </div>
      </main>
    </div>
  );
};
