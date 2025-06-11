import { useState } from "react";
import { useCompany } from "../../../context/company/CompanyContext";
import { useNavigate } from "react-router-dom";

export const CompanySelectionComponent = () => {
  const [isOpen, setIsOpen] = useState(false);
  const { companies, company, setActiveCompany } = useCompany();
  const navigate = useNavigate();

  const handleSelect = async (id: string) => {
    await setActiveCompany(id);
    navigate("/dashboard");
    setIsOpen(false);
  };

  return (
    <div className="relative inline-flex">
      <span className="inline-flex divide-x divide-gray-300 overflow-hidden rounded border border-gray-300 bg-white shadow-sm dark:divide-gray-600 dark:border-gray-600 dark:bg-gray-800">
        <button
          type="button"
          className="px-3 py-1.5 text-sm font-medium text-gray-700 transition-colors hover:bg-gray-50 hover:text-gray-900 focus:relative dark:text-gray-200 dark:hover:bg-gray-700 dark:hover:text-white"
          onClick={() => setIsOpen((prev) => !prev)}
        >
          {company ? company.name : "Wybierz firmę"}
        </button>
        <button
          type="button"
          className="px-3 py-1.5 text-sm font-medium text-gray-700 transition-colors hover:bg-gray-50 hover:text-gray-900 focus:relative dark:text-gray-200 dark:hover:bg-gray-700 dark:hover:text-white"
          aria-label="Menu"
          onClick={() => setIsOpen((prev) => !prev)}
        >
          <svg
            xmlns="http://www.w3.org/2000/svg"
            fill="none"
            viewBox="0 0 24 24"
            strokeWidth="1.5"
            stroke="currentColor"
            className="size-4"
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              d="m19.5 8.25-7.5 7.5-7.5-7.5"
            />
          </svg>
        </button>
      </span>

      <div
        role="menu"
        className={`absolute end-0 top-12 z-auto w-56 divide-y divide-gray-200 overflow-hidden rounded border border-gray-300 bg-white shadow-sm dark:divide-gray-700 dark:border-gray-600 dark:bg-gray-800 ${
          !isOpen ? "hidden" : ""
        }`}
      >
        <div>
          {companies.length === 0 && (
            <p className="block px-3 py-2 text-sm text-gray-500 dark:text-gray-400">
              Brak firm
            </p>
          )}
          {companies.map((c) => (
            <button
              key={c.id}
              className={`block w-full px-3 py-2 text-left text-sm font-medium transition-colors ${
                company?.id === c.id
                  ? "bg-indigo-100 text-indigo-700 dark:bg-gray-900 dark:text-indigo-200"
                  : "text-gray-700 hover:bg-indigo-50 hover:text-indigo-700 dark:text-gray-200 dark:hover:bg-gray-700 dark:hover:text-indigo-200"
              }`}
              onClick={() => handleSelect(c.id)}
              role="menuitem"
            >
              {c.name}
            </button>
          ))}
        </div>
      </div>
    </div>
  );
};
