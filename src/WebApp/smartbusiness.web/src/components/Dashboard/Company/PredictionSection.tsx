import { useEffect, useState } from "react";
import { usePrediction } from "../../../context/prediction/PredictionContext";
import { useService } from "../../../context/service/ServiceContext";
import type { SalesPredictionResponse } from "../../../models/prediction";
import type { Service } from "../../../models/service";
import { ServicePredictionChart } from "../PredictionSection/ServicePredictionChart";

const chartOptions = [
  {
    key: "sales",
    label: "Przewidywany przychód brutto",
    fetch: "fetchSalesPrediction",
  },
  {
    key: "tax",
    label: "Przewidywany podatek",
    fetch: "fetchTaxPrediction",
  },
  {
    key: "net",
    label: "Przewidywany przychód netto",
    fetch: "fetchNetSalesPrediction",
  },
  {
    key: "service",
    label: "Przewidywana sprzedaż pojedynczej usługi",
    fetch: "fetchServiceSalesPrediction",
  },
];

const colorPalettes = [
  { key: "indigo", name: "Indygo", color: "#2563eb" },
  { key: "green", name: "Zielony", color: "#059669" },
  { key: "orange", name: "Pomarańczowy", color: "#ea580c" },
  { key: "violet", name: "Fiolet", color: "#a21caf" },
  { key: "cyan", name: "Cyjan", color: "#06b6d4" },
  { key: "rose", name: "Różowy", color: "#e11d48" },
];

export const PredictionSection = () => {
  const {
    fetchSalesPrediction,
    fetchTaxPrediction,
    fetchNetSalesPrediction,
    fetchServiceSalesPrediction,
  } = usePrediction();
  const { fetchServices } = useService();
  const [serviceId, setServiceId] = useState("");
  const [monthsAhead, setMonthsAhead] = useState(6);
  const [selectedChart, setSelectedChart] = useState(chartOptions[0].key);
  const [paletteKey, setPaletteKey] = useState(colorPalettes[0].key);
  const [servicePrediction, setServicePrediction] =
    useState<SalesPredictionResponse | null>(null);
  const [serviceLoading, setServiceLoading] = useState(false);
  const [serviceError, setServiceError] = useState<string | null>(null);
  const [services, setServices] = useState<Service[]>([]);
  const [servicesLoading, setServicesLoading] = useState(false);
  const [servicesError, setServicesError] = useState<string | null>(null);

  // Fetch services when switching to single service chart
  useEffect(() => {
    if (selectedChart === "service") {
      setServicesLoading(true);
      setServicesError(null);
      fetchServices({})
        .then(setServices)
        .catch(() => setServicesError("Błąd ładowania usług."))
        .finally(() => setServicesLoading(false));
    }
  }, [selectedChart, fetchServices]);

  useEffect(() => {
    setServicePrediction(null);
    setServiceError(null);
    setServiceLoading(false);
    let fetchPromise;
    if (selectedChart === "service") {
      if (!serviceId) return;
      setServiceLoading(true);
      fetchPromise = fetchServiceSalesPrediction(serviceId, monthsAhead);
    } else if (selectedChart === "sales") {
      setServiceLoading(true);
      fetchPromise = fetchSalesPrediction(monthsAhead);
    } else if (selectedChart === "tax") {
      setServiceLoading(true);
      fetchPromise = fetchTaxPrediction(monthsAhead);
    } else if (selectedChart === "net") {
      setServiceLoading(true);
      fetchPromise = fetchNetSalesPrediction(monthsAhead);
    }
    if (fetchPromise) {
      fetchPromise
        .then((res: SalesPredictionResponse) => setServicePrediction(res))
        .catch(() => setServiceError("Błąd ładowania predykcji."))
        .finally(() => setServiceLoading(false));
    }
  }, [
    serviceId,
    monthsAhead,
    selectedChart,
    fetchServiceSalesPrediction,
    fetchSalesPrediction,
    fetchTaxPrediction,
    fetchNetSalesPrediction,
  ]);

  const palette =
    colorPalettes.find((p) => p.key === paletteKey) || colorPalettes[0];

  return (
    <div className="flex h-full w-full flex-col">
      <div className="mb-6 flex flex-wrap items-center gap-4">
        <label className="flex flex-col items-start gap-1 text-sm font-semibold text-gray-700 sm:flex-row sm:items-center sm:gap-2 dark:text-gray-200">
          Wybierz wykres:
          <select
            value={selectedChart}
            onChange={(e) => setSelectedChart(e.target.value)}
            className="w-full min-w-0 rounded-md border-2 border-gray-300 bg-white px-4 py-2 text-base shadow-sm focus:border-blue-500 focus:ring-2 focus:ring-blue-500 sm:w-auto dark:border-gray-600 dark:bg-gray-700 dark:text-white dark:focus:border-blue-500 dark:focus:ring-blue-500"
          >
            {chartOptions.map((opt) => (
              <option key={opt.key} value={opt.key}>
                {opt.label}
              </option>
            ))}
          </select>
        </label>
        {selectedChart === "service" && (
          <label className="flex flex-col items-start gap-1 text-sm sm:flex-row sm:items-center sm:gap-2">
            Usługa:
            {servicesLoading ? (
              <span className="text-gray-500">Ładowanie...</span>
            ) : servicesError ? (
              <span className="text-red-500">{servicesError}</span>
            ) : (
              <select
                value={serviceId}
                onChange={(e) => setServiceId(e.target.value)}
                className="w-full min-w-0 rounded-md border-2 border-gray-300 bg-white px-2 py-1 text-base shadow-sm focus:border-blue-500 focus:ring-2 focus:ring-blue-500 sm:w-56 dark:border-gray-600 dark:bg-gray-700 dark:text-white dark:focus:border-blue-500 dark:focus:ring-blue-500"
              >
                <option value="">Wybierz usługę...</option>
                {services.map((s) => (
                  <option key={s.id} value={s.id}>
                    {s.name}
                  </option>
                ))}
              </select>
            )}
          </label>
        )}
        <label className="flex flex-col items-start gap-1 text-sm sm:flex-row sm:items-center sm:gap-2">
          Miesięcy do przodu:
          <input
            type="number"
            min={1}
            max={12}
            value={monthsAhead}
            onChange={(e) =>
              setMonthsAhead(Math.max(1, Math.min(12, Number(e.target.value))))
            }
            className="w-full min-w-0 rounded-md border-2 border-gray-300 bg-white px-2 py-1 text-base shadow-sm focus:border-blue-500 focus:ring-2 focus:ring-blue-500 sm:w-20 dark:border-gray-600 dark:bg-gray-700 dark:text-white dark:focus:border-blue-500 dark:focus:ring-blue-500"
          />
        </label>
        <label className="flex flex-col items-start gap-1 text-sm sm:flex-row sm:items-center sm:gap-2">
          Kolor wykresu:
          <select
            value={paletteKey}
            onChange={(e) => setPaletteKey(e.target.value)}
            className="w-full min-w-0 rounded-md border-2 border-gray-300 bg-white px-4 py-2 text-base shadow-sm focus:border-blue-500 focus:ring-2 focus:ring-blue-500 sm:min-w-[120px] dark:border-gray-600 dark:bg-gray-700 dark:text-white dark:focus:border-blue-500 dark:focus:ring-blue-500"
          >
            {colorPalettes.map((p) => (
              <option key={p.key} value={p.key}>
                {p.name}
              </option>
            ))}
          </select>
        </label>
      </div>
      <div className="min-h-0 flex-1">
        <div className="h-full">
          {serviceLoading ? (
            <div>Ładowanie predykcji...</div>
          ) : serviceError ? (
            <div className="text-red-500">{serviceError}</div>
          ) : servicePrediction ? (
            <ServicePredictionChart
              data={servicePrediction}
              color={palette.color}
            />
          ) : selectedChart === "service" ? (
            <div className="text-gray-500">
              Wybierz usługę, aby zobaczyć predykcję.
            </div>
          ) : null}
        </div>
      </div>
    </div>
  );
};
