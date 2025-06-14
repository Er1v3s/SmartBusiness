import { Bar } from "react-chartjs-2";
import type { Transaction } from "../../../models/transaction";

interface TotalAmountByProductChartProps {
  transactions: Transaction[];
  barColor: string;
  serviceColor?: string;
}

function getLabel(t: Transaction | { itemName?: string; itemId: string }) {
  return "itemName" in t && typeof t.itemName === "string"
    ? t.itemName
    : t.itemId;
}

export const TotalAmountByProductChart: React.FC<
  TotalAmountByProductChartProps
> = ({ transactions, barColor, serviceColor = "#a21caf" }) => {
  const sumByProduct: { [label: string]: number } = {};
  const sumByService: { [label: string]: number } = {};
  const productLabels: string[] = [];
  const serviceLabels: string[] = [];

  transactions.forEach((t) => {
    const label = getLabel(t);
    if (t.itemType === "product") {
      sumByProduct[label] = (sumByProduct[label] || 0) + t.totalAmount;
      if (!productLabels.includes(label)) productLabels.push(label);
    } else if (t.itemType === "service") {
      sumByService[label] = (sumByService[label] || 0) + t.totalAmount;
      if (!serviceLabels.includes(label)) serviceLabels.push(label);
    }
  });

  const allLabels = Array.from(new Set([...productLabels, ...serviceLabels]));

  const data = {
    labels: allLabels,
    datasets: [
      {
        label: "Produkty",
        data: allLabels.map((name) => sumByProduct[name] || 0),
        backgroundColor: barColor,
      },
      {
        label: "Usługi",
        data: allLabels.map((name) => sumByService[name] || 0),
        backgroundColor: serviceColor,
      },
    ],
  };

  return <Bar data={data} />;
};
