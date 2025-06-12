import { Pie } from "react-chartjs-2";
import { Chart as ChartJS, ArcElement, Tooltip, Legend } from "chart.js";
ChartJS.register(ArcElement, Tooltip, Legend);
import type { EnrichedTransaction } from "../../../models/transaction";

interface ProductSharePieChartProps {
  transactions: EnrichedTransaction[];
}

export const ProductSharePieChart: React.FC<ProductSharePieChartProps> = ({
  transactions,
}) => {
  const sumByProduct: { [productId: string]: number } = {};
  transactions.forEach((t) => {
    sumByProduct[t.itemName] = (sumByProduct[t.itemId] || 0) + t.totalAmount;
  });

  const productIds = Object.keys(sumByProduct);
  const data = {
    labels: productIds,
    datasets: [
      {
        data: productIds.map((id) => sumByProduct[id]),
        backgroundColor: [
          "#2563eb",
          "#22d3ee",
          "#f59e42",
          "#f43f5e",
          "#a21caf",
          "#16a34a",
          "#fea308",
          "#dea308",
        ],
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
