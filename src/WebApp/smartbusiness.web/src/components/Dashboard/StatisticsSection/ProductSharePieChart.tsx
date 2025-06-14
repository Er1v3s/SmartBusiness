import { Pie } from "react-chartjs-2";
import { Chart as ChartJS, ArcElement, Tooltip, Legend } from "chart.js";
ChartJS.register(ArcElement, Tooltip, Legend);
import type { EnrichedTransaction } from "../../../models/transaction";

interface ProductSharePieChartProps {
  transactions: EnrichedTransaction[];
  barColor?: string;
  serviceColor?: string;
}

export const ProductSharePieChart: React.FC<ProductSharePieChartProps> = ({
  transactions,
  barColor = "#2563eb",
  serviceColor = "#a21caf",
}) => {
  let productSum = 0;
  let serviceSum = 0;
  transactions.forEach((t) => {
    if (t.itemType === "product") productSum += t.totalAmount;
    else if (t.itemType === "service") serviceSum += t.totalAmount;
  });

  const data = {
    labels: ["Produkty", "Usługi"],
    datasets: [
      {
        data: [productSum, serviceSum],
        backgroundColor: [barColor, serviceColor],
      },
    ],
  };

  const options = {
    maintainAspectRatio: false,
    responsive: true,
    plugins: {
      legend: { position: "bottom" as const },
    },
  };

  return <Pie data={data} options={options} />;
};
