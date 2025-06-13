import { useEffect, useState } from "react";
import { usePrediction } from "../../../context/prediction/PredictionContext";
import type { SalesPredictionResponse } from "../../../models/prediction";

interface Props {
  monthsAhead: number;
  itemId?: string;
  chartKey: string;
}

export const PredictionMetricsCard: React.FC<Props> = ({
  monthsAhead,
  itemId,
  chartKey,
}) => {
  const {
    fetchSalesPrediction,
    fetchNetSalesPrediction,
    fetchTaxPrediction,
    fetchServiceSalesPrediction,
  } = usePrediction();
  const [metrics, setMetrics] = useState<{
    model: string;
    r2: number;
    mae: number;
  } | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    let fetchFn: Promise<SalesPredictionResponse> | null = null;
    setLoading(true);
    setError(null);
    setMetrics(null);
    if (chartKey === "sales") fetchFn = fetchSalesPrediction(monthsAhead);
    else if (chartKey === "netSales")
      fetchFn = fetchNetSalesPrediction(monthsAhead);
    else if (chartKey === "tax") fetchFn = fetchTaxPrediction(monthsAhead);
    else if (chartKey === "product" && itemId)
      fetchFn = fetchServiceSalesPrediction(itemId, monthsAhead);
    if (!fetchFn) {
      setLoading(false);
      return;
    }
    fetchFn
      .then((res) => setMetrics(res.metrics))
      .catch(() => setError("Błąd ładowania metryk."))
      .finally(() => setLoading(false));
  }, [
    chartKey,
    monthsAhead,
    itemId,
    fetchSalesPrediction,
    fetchNetSalesPrediction,
    fetchTaxPrediction,
    fetchServiceSalesPrediction,
  ]);

  if (loading)
    return (
      <div className="rounded-lg border bg-white p-4 shadow dark:bg-gray-800 dark:text-white">
        Ładowanie metryk...
      </div>
    );
  if (error)
    return (
      <div className="rounded-lg border bg-white p-4 text-red-500 shadow dark:bg-gray-800 dark:text-white">
        {error}
      </div>
    );
  if (!metrics) return null;

  return (
    <div className="max-w-xs min-w-[260px] rounded-lg border bg-white p-4 shadow dark:bg-gray-800 dark:text-white">
      <div className="mb-2 font-semibold">Metryki modelu predykcyjnego</div>
      <div className="flex flex-col gap-1">
        <span>
          <b>Model:</b> {metrics.model}
        </span>
        <span>
          <b>R²:</b> {metrics.r2.toFixed(3)}
        </span>
        <span>
          <b>MAE:</b> {metrics.mae.toFixed(2)}
        </span>
      </div>
      <div className="mt-2 text-xs text-gray-500 dark:text-gray-400">
        Im wyższe R² (max 1), tym lepsze dopasowanie. Im niższe MAE, tym
        mniejszy błąd predykcji.
      </div>
    </div>
  );
};
