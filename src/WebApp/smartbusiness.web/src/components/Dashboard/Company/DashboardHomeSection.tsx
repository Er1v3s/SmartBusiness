import React from "react";
import { useCompany } from "../../../context/company/CompanyContext";

export const DashboardHomeSection: React.FC = () => {
  const { company } = useCompany();
  return (
    <>
      {/* Header Section */}
      <div className="mb-8">
        <h1 className="mb-2 text-3xl font-bold text-gray-900 dark:text-gray-100">
          Dashboard
        </h1>
        <p className="text-gray-600 dark:text-gray-400">
          Zarządzaj swoim kontem i aplikacją
        </p>
      </div>

      {/* Stats Cards */}
      <div className="mb-8 grid grid-cols-1 gap-6 md:grid-cols-3">
        {/* Some content in the future */}
      </div>
      {/* Company Info Card */}
      <div className="rounded-xl border border-gray-200 bg-gray-100 p-6 shadow-xl dark:border-gray-900 dark:bg-gray-800">
        <h2 className="mb-4 text-xl font-semibold text-gray-900 dark:text-gray-100">
          Informacje o firmie
        </h2>
        {company ? (
          <div className="space-y-3">
            <div className="flex justify-between">
              <span className="text-gray-600 dark:text-gray-300">
                Nazwa firmy:
              </span>
              <span className="font-medium text-gray-900 dark:text-gray-100">
                {company.name}
              </span>
            </div>
            <div className="flex justify-between">
              <span className="text-gray-600 dark:text-gray-300">
                ID firmy:
              </span>
              <span className="font-medium text-gray-900 dark:text-gray-100">
                {company.id}
              </span>
            </div>
          </div>
        ) : (
          <div className="text-gray-500 dark:text-gray-400">
            Brak wybranej firmy.
          </div>
        )}
      </div>

      {/* Welcome Message */}
      <div className="mt-8 rounded-lg bg-gradient-to-r from-indigo-500 to-purple-600 p-6 text-white shadow">
        <h2 className="mb-2 text-2xl font-bold">Witaj w SecureApp!</h2>
        <span className="text-indigo-100">
          Twoje konto zostało pomyślnie uwierzytelnione przy użyciu JWT Token.
          Możesz teraz korzystać ze wszystkich funkcji aplikacji.
        </span>
      </div>
    </>
  );
};
