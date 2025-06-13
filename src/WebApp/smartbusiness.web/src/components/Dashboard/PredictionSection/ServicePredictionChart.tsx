import { Line, Bar } from "react-chartjs-2";
import { Chart, Filler } from "chart.js";
Chart.register(Filler);
import type { SalesPredictionResponse } from "../../../models/prediction";

interface Props {
  data: SalesPredictionResponse;
  color?: string;
}

export const ServicePredictionChart: React.FC<Props> = ({
  data,
  color = "#2563eb",
}) => {
  const isBar = data.predictions.length < 3;
  const chartData = {
    labels: data.predictions.map((p) => p.target_month),
    datasets: [
      {
        label: "Predykcja: ",
        data: data.predictions.map((p) => p.prediction),
        borderColor: color,
        backgroundColor: color + "33",
        fill: true,
        tension: 0.3,
      },
    ],
  };
  const metricsBox = (
    <div
      style={{
        position: "absolute",
        top: 16,
        right: 16,
        background: "rgba(255,255,255,0.92)",
        borderRadius: 8,
        boxShadow: "0 2px 8px 0 rgba(0,0,0,0.07)",
        padding: "8px 14px",
        fontSize: 13,
        zIndex: 10,
        minWidth: 120,
        color: "#222",
      }}
      className="border border-gray-200 dark:border-gray-700 dark:bg-gray-800 dark:text-white"
    >
      <div style={{ fontWeight: 600, fontSize: 12, marginBottom: 2 }}>
        <h2>Jakość predykcji</h2>
      </div>
      <div>
        <b>Dopasowanie modelu:</b> {(data.metrics.r2 * 100).toFixed(0)}% <br />
        <sup>więcej = lepiej</sup>
      </div>
      <div>
        <b>Śr. błąd:</b> {data.metrics.mae.toFixed(2)} <br />
        <sup>mniej = lepiej</sup>
      </div>
    </div>
  );
  return (
    <div style={{ position: "relative", width: "100%", height: "100%" }}>
      {metricsBox}
      {isBar ? (
        <Bar
          data={chartData}
          options={{
            maintainAspectRatio: false,
            responsive: true,
            plugins: { legend: { position: "top" as const } },
          }}
        />
      ) : (
        <Line
          data={chartData}
          options={{
            maintainAspectRatio: false,
            responsive: true,
            plugins: { legend: { position: "top" as const } },
          }}
        />
      )}
    </div>
  );
};
