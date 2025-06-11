import React, { useEffect, useState } from "react";
import { useCompany } from "../../../context/company/CompanyContext";
import { useProduct } from "../../../context/product/ProductContext";
import { useService } from "../../../context/service/ServiceContext";
import { useTransaction } from "../../../context/transaction/TransactionContext";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../../context/auth/AuthContext";
import type { Transaction } from "../../../models/transaction";

export const DashboardHomeSection: React.FC = () => {
  const { company } = useCompany();
  const { fetchProducts } = useProduct();
  const { fetchServices } = useService();
  const { fetchTransactions } = useTransaction();
  const { user } = useAuth();
  const [productCount, setProductCount] = useState(0);
  const [serviceCount, setServiceCount] = useState(0);
  const [transactionCount, setTransactionCount] = useState(0);
  const [recentTransactions, setRecentTransactions] = useState<Transaction[]>(
    [],
  );
  const [loadingRecent, setLoadingRecent] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    if (!company) return;
    fetchProducts({}).then((p) => setProductCount(p.length));
    fetchServices({}).then((s) => setServiceCount(s.length));
    fetchTransactions({}).then((t) => setTransactionCount(t.length));
    setLoadingRecent(true);
    fetchTransactions({})
      .then((t) => {
        // Sortuj po dacie malejąco i weź 5 najnowszych
        const sorted = t.sort(
          (a, b) =>
            new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime(),
        );
        setRecentTransactions(sorted.slice(0, 5));
      })
      .finally(() => setLoadingRecent(false));
  }, [company, fetchProducts, fetchServices, fetchTransactions]);

  return (
    <>
      {/* Welcome with username */}
      <div className="animate-fade-in mb-4 flex items-center gap-4">
        <div>
          <h1 className="mb-1 text-3xl font-bold text-gray-900 dark:text-gray-100">
            Witaj{user?.username ? `, ${user.username}` : ""}!
          </h1>
          <p className="text-gray-600 dark:text-gray-400">
            To jest Twój panel główny. Miłego dnia!
          </p>
        </div>
      </div>

      {/* Ostatnie transakcje */}
      <div className="animate-fade-in mb-8 rounded-lg border-2 border-gray-100 bg-white p-6 shadow-xl dark:border-gray-700 dark:bg-gray-800">
        <h2 className="mb-4 text-xl font-semibold text-gray-900 dark:text-gray-100">
          Ostatnie transakcje
        </h2>
        {loadingRecent ? (
          <div className="space-y-2">
            {[...Array(3)].map((_, i) => (
              <div
                key={i}
                className="h-6 w-full animate-pulse rounded bg-gray-200 dark:bg-gray-700"
              />
            ))}
          </div>
        ) : recentTransactions.length === 0 ? (
          <div className="text-gray-500 dark:text-gray-400">
            Brak transakcji.
          </div>
        ) : (
          <ul className="divide-y divide-gray-200 dark:divide-gray-700">
            {recentTransactions.map((t) => (
              <li key={t.id} className="flex items-center justify-between py-2">
                <span className="truncate text-gray-800 dark:text-gray-100">
                  {new Date(t.createdAt).toLocaleString()} – {t.productId} –{" "}
                  {t.totalAmount.toFixed(2)} zł
                </span>
                <span className="ml-2 rounded bg-indigo-100 px-2 py-1 text-xs font-semibold text-indigo-700 dark:bg-indigo-900 dark:text-indigo-200">
                  {t.quantity} szt.
                </span>
              </li>
            ))}
          </ul>
        )}
      </div>

      {/* Stats Cards with tooltips and animation */}
      <div className="animate-fade-in mb-8 grid grid-cols-1 gap-6 md:grid-cols-3">
        <div className="group relative flex flex-col items-center rounded-xl bg-gradient-to-br from-indigo-500 to-indigo-700 p-6 text-white shadow-lg transition-transform hover:scale-105">
          <span className="text-lg font-semibold">Produkty</span>
          <span className="mt-2 text-4xl font-extrabold">{productCount}</span>
        </div>
        <div className="group relative flex flex-col items-center rounded-xl bg-gradient-to-br from-purple-500 to-purple-700 p-6 text-white shadow-lg transition-transform hover:scale-105">
          <span className="text-lg font-semibold">Usługi</span>
          <span className="mt-2 text-4xl font-extrabold">{serviceCount}</span>
        </div>
        <div className="group relative flex flex-col items-center rounded-xl bg-gradient-to-br from-pink-500 to-pink-700 p-6 text-white shadow-lg transition-transform hover:scale-105">
          <span className="text-lg font-semibold">Transakcje</span>
          <span className="mt-2 text-4xl font-extrabold">
            {transactionCount}
          </span>
        </div>
      </div>

      {/* Quick Actions */}
      <div className="animate-fade-in mb-8 flex flex-wrap gap-4">
        <button
          className="min-w-[180px] flex-1 rounded-lg bg-indigo-600 px-6 py-4 text-lg font-semibold text-white shadow transition hover:bg-indigo-700"
          onClick={() => navigate("/dashboard/company/sale")}
        >
          Dodaj sprzedaż
        </button>
        <button
          className="min-w-[180px] flex-1 rounded-lg bg-purple-600 px-6 py-4 text-lg font-semibold text-white shadow transition hover:bg-purple-700"
          onClick={() => navigate("/dashboard/company/products")}
        >
          Lista produktów
        </button>
        <button
          className="min-w-[180px] flex-1 rounded-lg bg-pink-600 px-6 py-4 text-lg font-semibold text-white shadow transition hover:bg-pink-700"
          onClick={() => navigate("/dashboard/company/services")}
        >
          Lista usług
        </button>
        <button
          className="min-w-[180px] flex-1 rounded-lg bg-gray-800 px-6 py-4 text-lg font-semibold text-white shadow transition hover:bg-gray-900"
          onClick={() => navigate("/dashboard/company/stats")}
        >
          Statystyki firmy
        </button>
      </div>

      {/* Welcome Message */}
      <div className="animate-fade-in mt-8 rounded-lg bg-gradient-to-r from-indigo-500 to-purple-600 p-6 text-white shadow-xl">
        <h2 className="mb-2 text-2xl font-bold">Witaj w SmartBusiness!</h2>
        <span className="text-indigo-100">
          Aplikacja do zarządzania firmą, która ułatwia życie i zwiększa
          efektywność Twojego biznesu. Wykorzystaj pełen potencjał technologii,
          aby skupić się na tym, co najważniejsze - rozwijaniu swojej firmy.
        </span>
      </div>
    </>
  );
};
