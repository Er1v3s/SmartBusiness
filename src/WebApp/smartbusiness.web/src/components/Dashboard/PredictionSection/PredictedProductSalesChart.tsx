import { useEffect, useState } from "react";
import { usePrediction } from "../../../context/prediction/PredictionContext";
import { Bar } from "react-chartjs-2";
import type { SalesPredictionResponse } from "../../../models/prediction";

export const PredictedProductSalesChart: React.FC<{
  monthsAhead: number;
  itemId: string;
}> = ({ monthsAhead, itemId }) => {
  const { fetchServiceSalesPrediction } = usePrediction();
  const [data, setData] = useState<SalesPredictionResponse | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (itemId) {
      setLoading(true);
      setError(null);
      fetchServiceSalesPrediction(itemId, monthsAhead)
        .then(setData)
        .catch(() => setError("Błąd ładowania predykcji."))
        .finally(() => setLoading(false));
    }
  }, [fetchServiceSalesPrediction, itemId, monthsAhead]);

  if (!itemId) return <div>Wybierz produkt lub usługę.</div>;
  if (loading) return <div>Ładowanie predykcji...</div>;
  if (error) return <div className="text-red-500">{error}</div>;
  if (!data) return null;

  return (
    <Bar
      data={{
        labels: data.predictions.map((p) => p.target_month),
        datasets: [
          {
            label: "Predykcja sprzedaży produktu/usługi",
            data: data.predictions.map((p) => p.prediction),
            backgroundColor: "#f59e42",
          },
        ],
      }}
      options={{
        maintainAspectRatio: false,
        responsive: true,
        plugins: { legend: { position: "top" as const } },
      }}
    />
  );
};
