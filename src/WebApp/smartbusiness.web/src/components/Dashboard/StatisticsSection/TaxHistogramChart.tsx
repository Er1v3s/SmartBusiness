import { Bar } from "react-chartjs-2";
import type { Transaction } from "../../../models/transaction";

interface TaxHistogramChartProps {
  transactions: Transaction[];
  barColor: string;
  serviceColor?: string;
}

export const TaxHistogramChart: React.FC<TaxHistogramChartProps> = ({
  transactions,
  barColor,
  serviceColor = "#a21caf",
}) => {
  const taxCountsProduct: { [tax: number]: number } = {};
  const taxCountsService: { [tax: number]: number } = {};

  transactions.forEach((t) => {
    if (t.itemType === "product") {
      taxCountsProduct[t.tax] = (taxCountsProduct[t.tax] || 0) + 1;
    } else if (t.itemType === "service") {
      taxCountsService[t.tax] = (taxCountsService[t.tax] || 0) + 1;
    }
  });

  const allTaxLevels = Array.from(
    new Set([
      ...Object.keys(taxCountsProduct).map(Number),
      ...Object.keys(taxCountsService).map(Number),
    ]),
  ).sort((a, b) => a - b);

  const data = {
    labels: allTaxLevels.map((t) => `${t}%`),
    datasets: [
      {
        label: "Produkty",
        data: allTaxLevels.map((t) => taxCountsProduct[t] || 0),
        backgroundColor: barColor,
      },
      {
        label: "Usługi",
        data: allTaxLevels.map((t) => taxCountsService[t] || 0),
        backgroundColor: serviceColor,
      },
    ],
  };

  return <Bar data={data} />;
};
