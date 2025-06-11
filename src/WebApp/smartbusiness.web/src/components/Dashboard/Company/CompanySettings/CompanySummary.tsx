import React, { useEffect, useState } from "react";
import { useCompany } from "../../../../context/company/CompanyContext";
import { useProduct } from "../../../../context/product/ProductContext";
import { useService } from "../../../../context/service/ServiceContext";
import { useTransaction } from "../../../../context/transaction/TransactionContext";

export const CompanySummary: React.FC = () => {
  const { company } = useCompany();
  const { fetchProducts } = useProduct();
  const { fetchServices } = useService();
  const { fetchTransactions } = useTransaction();
  const [productCount, setProductCount] = useState(0);
  const [serviceCount, setServiceCount] = useState(0);
  const [transactionCount, setTransactionCount] = useState(0);

  useEffect(() => {
    if (!company) return;
    fetchProducts({}).then((p) => setProductCount(p.length));
    fetchServices({}).then((s) => setServiceCount(s.length));
    fetchTransactions({}).then((t) => setTransactionCount(t.length));
  }, [company, fetchProducts, fetchServices, fetchTransactions]);

  return (
    <div className="space-y-4">
      <h2 className="text-2xl font-bold text-gray-800 dark:text-gray-100">
        Podsumowanie firmy
      </h2>
      <div className="rounded-lg border-2 border-gray-100 bg-white p-6 shadow-xl dark:border-gray-700 dark:bg-gray-800">
        {company ? (
          <div className="space-y-2">
            <div>
              <span className="font-semibold">Nazwa:</span>{" "}
              <span className="font-semibold text-indigo-600 dark:text-indigo-300">
                {company.name}
              </span>
            </div>
            <div>
              <span className="font-semibold">Data utworzenia:</span>{" "}
              <span className="font-semibold text-indigo-600 dark:text-indigo-300">
                {company.createdAt
                  ? new Date(company.createdAt).toLocaleDateString()
                  : "Brak daty"}
              </span>
            </div>
            <div>
              <span className="font-semibold">Liczba produktów:</span>{" "}
              <span className="font-semibold text-indigo-600 dark:text-indigo-300">
                {productCount}
              </span>
            </div>
            <div>
              <span className="font-semibold">Liczba usług:</span>{" "}
              <span className="font-semibold text-indigo-600 dark:text-indigo-300">
                {serviceCount}
              </span>
            </div>
            <div>
              <span className="font-semibold">Liczba transakcji:</span>{" "}
              <span className="font-semibold text-indigo-600 dark:text-indigo-300">
                {transactionCount}
              </span>
            </div>
          </div>
        ) : (
          <div className="text-gray-500 dark:text-gray-400">
            Brak wybranej firmy.
          </div>
        )}
      </div>
    </div>
  );
};
