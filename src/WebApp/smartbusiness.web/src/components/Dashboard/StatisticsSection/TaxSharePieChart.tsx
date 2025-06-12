import { Pie } from "react-chartjs-2";
import { Chart as ChartJS, ArcElement, Tooltip, Legend } from "chart.js";
ChartJS.register(ArcElement, Tooltip, Legend);
import type { Transaction } from "../../../models/transaction";

interface TaxSharePieChartProps {
  transactions: Transaction[];
}

export const TaxSharePieChart: React.FC<TaxSharePieChartProps> = ({
  transactions,
}) => {
  const totalAmount = transactions.reduce((sum, t) => sum + t.totalAmount, 0);
  const totalTax = transactions.reduce(
    (sum, t) => sum + (t.totalAmount - t.totalAmountMinusTax),
    0,
  );

  const data = {
    labels: ["Podatek", "Sprzedaż netto"],
    datasets: [
      {
        data: [totalTax, totalAmount - totalTax],
        backgroundColor: ["#f59e42", "#16a34a"],
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
