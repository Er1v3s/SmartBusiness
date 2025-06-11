import React, { useEffect } from "react";
import { useCompany } from "../../../../context/company/CompanyContext";

export const CompanyList: React.FC = () => {
  const { companies, fetchCompanies } = useCompany();

  useEffect(() => {
    fetchCompanies();
  }, [fetchCompanies]);

  return (
    <div className="space-y-4">
      <h2 className="text-2xl font-bold text-gray-800 dark:text-gray-100">
        Lista firm
      </h2>
      <div className="rounded-lg border-2 border-gray-100 bg-white p-6 shadow-xl dark:border-gray-700 dark:bg-gray-800">
        <ul className="space-y-2">
          {companies && companies.length > 0 ? (
            companies.map((c) => (
              <li
                key={c.id}
                className="flex items-center justify-between rounded border p-2 dark:bg-gray-800"
              >
                <div className="flex-1">
                  <div className="font-semibold text-gray-800 dark:text-gray-200">
                    {c.name}
                  </div>
                  <div className="text-sm text-gray-600 dark:text-gray-400">
                    Id: {c.id}
                  </div>
                </div>
              </li>
            ))
          ) : (
            <li className="text-gray-500 dark:text-gray-400">Brak firm.</li>
          )}
        </ul>
      </div>
    </div>
  );
};
