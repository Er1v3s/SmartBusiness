import { useEffect, useState } from "react";
import { usePrediction } from "../../../context/prediction/PredictionContext";
import { Line } from "react-chartjs-2";
import type { SalesPredictionResponse } from "../../../models/prediction";

export const PredictedNetSalesChart: React.FC<{ monthsAhead: number }> = ({
  monthsAhead,
}) => {
  const { fetchNetSalesPrediction } = usePrediction();
  const [data, setData] = useState<SalesPredictionResponse | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    setLoading(true);
    setError(null);
    fetchNetSalesPrediction(monthsAhead)
      .then(setData)
      .catch(() => setError("Błąd ładowania predykcji."))
      .finally(() => setLoading(false));
  }, [fetchNetSalesPrediction, monthsAhead]);

  if (loading) return <div>Ładowanie predykcji...</div>;
  if (error) return <div className="text-red-500">{error}</div>;
  if (!data) return null;

  return (
    <Line
      data={{
        labels: data.predictions.map((p) => p.target_month),
        datasets: [
          {
            label: "Predykcja sprzedaży netto",
            data: data.predictions.map((p) => p.prediction),
            borderColor: "#10b981",
            backgroundColor: "#10b98133",
            fill: true,
            tension: 0.3,
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
