import { useTransaction } from "../../../context/transaction/TransactionContext";
import { useEffect, useState, useRef } from "react";
import { TransactionBarChart } from "../StatisticsSection/NumberOfTransactionChart";
import { TotalAmountOverTimeChart } from "../StatisticsSection/TotalAmountOverTimeChart";
import { TotalAmountMinusTaxOverTimeChart } from "../StatisticsSection/TotalAmountMinusTaxOverTimeChart";
import { TaxOverTimeChart } from "../StatisticsSection/TaxOverTimeChart";
import { TotalVsNetAmountChart } from "../StatisticsSection/TotalVsNetAmountChart";
import { TaxSharePieChart } from "../StatisticsSection/TaxSharePieChart";
import { AvgTransactionValueChart } from "../StatisticsSection/AvgTransactionValueChart";
import { TransactionCountByProductChart } from "../StatisticsSection/TransactionCountByProductChart";
import { TotalAmountByProductChart } from "../StatisticsSection/TotalAmountByProductChart";
import { TaxHistogramChart } from "../StatisticsSection/TaxHistogramChart";
import { ProductSharePieChart } from "../StatisticsSection/ProductSharePieChart";
import type { Transaction } from "../../../models/transaction";

const chartOptions = [
  {
    key: "countByMonth",
    label: "Liczba transakcji w czasie",
    component: TransactionBarChart,
  },
  {
    key: "totalAmountOverTime",
    label: "Suma sprzedaży brutto w czasie",
    component: TotalAmountOverTimeChart,
  },
  {
    key: "TotalAmountMinusTaxOverTimeChart",
    label: "Suma sprzedaży netto w czasie",
    component: TotalAmountMinusTaxOverTimeChart,
  },
  {
    key: "totalVsNet",
    label: "Brutto vs Netto w czasie",
    component: TotalVsNetAmountChart,
  },
  {
    key: "taxOverTime",
    label: "Podatek do zapłaty w czasie",
    component: TaxOverTimeChart,
  },
  {
    key: "taxShare",
    label: "Udział podatku w sprzedaży (kołowy)",
    component: TaxSharePieChart,
  },
  {
    key: "avgValue",
    label: "Średnia wartość transakcji w czasie",
    component: AvgTransactionValueChart,
  },
  {
    key: "countByProduct",
    label: "Liczba transakcji wg produktu",
    component: TransactionCountByProductChart,
  },
  {
    key: "amountByProduct",
    label: "Suma sprzedaży wg produktu",
    component: TotalAmountByProductChart,
  },
  {
    key: "taxHistogram",
    label: "Histogram podatku",
    component: TaxHistogramChart,
  },
  {
    key: "productShare",
    label: "Udział produktów w sprzedaży (kołowy)",
    component: ProductSharePieChart,
  },
];

export const StatisticsSection = () => {
  const { fetchTransactions } = useTransaction();
  const [transactions, setTransactions] = useState<Transaction[]>([]);
  const [loading, setLoading] = useState(true);
  const [selectedChart, setSelectedChart] = useState(chartOptions[0].key);
  const selectRef = useRef<HTMLSelectElement>(null);

  // Panel opcji - stan globalny
  const today = new Date();
  const defaultFrom = new Date(today);
  defaultFrom.setDate(today.getDate() - 31);
  const [dateFrom, setDateFrom] = useState<string>(
    defaultFrom.toISOString().slice(0, 10),
  );
  const [dateTo, setDateTo] = useState<string>(
    today.toISOString().slice(0, 10),
  );
  const [groupBy, setGroupBy] = useState<"day" | "month" | "year">("day");
  const [barColor, setBarColor] = useState<string>("#2563eb");

  useEffect(() => {
    fetchTransactions({})
      .then(setTransactions)
      .finally(() => setLoading(false));
  }, [fetchTransactions]);

  const ChartComponent = chartOptions.find(
    (c) => c.key === selectedChart,
  )?.component;

  // Options panel
  const renderOptionsPanel = (
    <div className="flex flex-wrap items-center gap-4">
      <label className="flex items-center gap-2 text-sm">
        Od:
        <input
          type="date"
          value={dateFrom}
          onChange={(e) => setDateFrom(e.target.value)}
          className="min-w-[140px] rounded-md border border-gray-300 px-4 py-2 text-base shadow-sm focus:border-blue-500 focus:ring-2 focus:ring-blue-500 dark:border-gray-600 dark:bg-gray-700 dark:text-white dark:focus:border-blue-500 dark:focus:ring-blue-500"
        />
      </label>
      <label className="flex items-center gap-2 text-sm">
        Do:
        <input
          type="date"
          value={dateTo}
          onChange={(e) => setDateTo(e.target.value)}
          className="min-w-[140px] rounded-md border border-gray-300 px-4 py-2 text-base shadow-sm focus:border-blue-500 focus:ring-2 focus:ring-blue-500 dark:border-gray-600 dark:bg-gray-700 dark:text-white dark:focus:border-blue-500 dark:focus:ring-blue-500"
        />
      </label>
      <label className="flex items-center gap-2 text-sm">
        Grupuj wg:
        <select
          value={groupBy}
          onChange={(e) =>
            setGroupBy(e.target.value as "day" | "month" | "year")
          }
          className="min-w-[120px] rounded-md border border-gray-300 px-4 py-2 text-base shadow-sm focus:border-blue-500 focus:ring-2 focus:ring-blue-500 dark:border-gray-600 dark:bg-gray-700 dark:text-white dark:focus:border-blue-500 dark:focus:ring-blue-500"
        >
          <option value="day">Dzień</option>
          <option value="month">Miesiąc</option>
          <option value="year">Rok</option>
        </select>
      </label>
      <label className="flex items-center gap-2 text-sm">
        Kolor:
        <input
          type="color"
          value={barColor}
          onChange={(e) => setBarColor(e.target.value)}
          className="min-w-[60px] rounded-md border border-gray-300 px-2 py-2 text-base shadow-sm focus:border-blue-500 focus:ring-2 focus:ring-blue-500 dark:border-gray-600 dark:bg-gray-700 dark:focus:border-blue-500 dark:focus:ring-blue-500"
          style={{ height: 40 }}
        />
      </label>
    </div>
  );

  // Filtrowanie transakcji po dacie
  const filteredTransactions = transactions.filter((t) => {
    const date = new Date(t.createdAt);
    const from = dateFrom ? new Date(dateFrom) : null;
    const to = dateTo ? new Date(dateTo) : null;
    if (from && date < from) return false;
    if (to && date > to) return false;
    return true;
  });

  return (
    <div className="flex h-full w-full flex-col">
      <div className="mb-6 flex shrink-0 flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
        <div className="flex items-center gap-4">
          <label
            htmlFor="chart-select"
            className="font-semibold text-gray-700 dark:text-gray-200"
          >
            Wybierz wykres:
          </label>
          <select
            id="chart-select"
            ref={selectRef}
            value={selectedChart}
            onChange={(e) => setSelectedChart(e.target.value)}
            className="min-w-[260px] rounded-md border border-gray-300 px-4 py-2 text-base shadow-sm focus:border-blue-500 focus:ring-2 focus:ring-blue-500 dark:border-gray-600 dark:bg-gray-700 dark:text-white dark:focus:border-blue-500 dark:focus:ring-blue-500"
          >
            {chartOptions.map((opt) => (
              <option key={opt.key} value={opt.key}>
                {opt.label}
              </option>
            ))}
          </select>
        </div>
        {renderOptionsPanel}
      </div>
      <div className="min-h-0 flex-1">
        {loading ? (
          <div>Ładowanie danych...</div>
        ) : transactions.length > 0 ? (
          <div className="h-full">
            {ChartComponent && (
              <ChartComponent
                transactions={filteredTransactions}
                groupBy={groupBy}
                barColor={barColor}
                dateFrom={dateFrom}
                dateTo={dateTo}
              />
            )}
          </div>
        ) : (
          <div className="flex h-full items-center justify-center">
            <h1 className="text-center text-5xl text-gray-500">
              Brak wyników do wyświetlenia
            </h1>
          </div>
        )}
      </div>
    </div>
  );
};
