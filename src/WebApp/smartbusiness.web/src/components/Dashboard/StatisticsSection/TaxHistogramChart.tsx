import { Bar } from "react-chartjs-2";
import type { Transaction } from "../../../models/transaction";

interface TaxHistogramChartProps {
  transactions: Transaction[];
  barColor: string;
}

export const TaxHistogramChart: React.FC<TaxHistogramChartProps> = ({
  transactions,
  barColor,
}) => {
  const taxCounts: { [tax: number]: number } = {};

  transactions.forEach((t) => {
    taxCounts[t.tax] = (taxCounts[t.tax] || 0) + 1;
  });

  const taxLevels = Object.keys(taxCounts)
    .map(Number)
    .sort((a, b) => a - b);

  const data = {
    labels: taxLevels.map((t) => `${t}%`),
    datasets: [
      {
        label: "Liczba transakcji wg podatku",
        data: taxLevels.map((t) => taxCounts[t]),
        backgroundColor: barColor,
      },
    ],
  };

  return <Bar data={data} />;
};
