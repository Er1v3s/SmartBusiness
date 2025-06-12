import { Bar } from "react-chartjs-2";
import type { Transaction } from "../../../models/transaction";

interface TransactionCountByProductChartProps {
  transactions: Transaction[];
  barColor: string;
  serviceColor?: string;
}

function getLabel(t: Transaction | { itemName?: string; itemId: string }) {
  return "itemName" in t && typeof t.itemName === "string"
    ? t.itemName
    : t.itemId;
}

export const TransactionCountByProductChart: React.FC<
  TransactionCountByProductChartProps
> = ({ transactions, barColor, serviceColor = "#a21caf" }) => {
  const countByProduct: { [label: string]: number } = {};
  const countByService: { [label: string]: number } = {};
  const productLabels: string[] = [];
  const serviceLabels: string[] = [];

  transactions.forEach((t) => {
    const label = getLabel(t);
    if (t.itemType === "product") {
      countByProduct[label] = (countByProduct[label] || 0) + 1;
      if (!productLabels.includes(label)) productLabels.push(label);
    } else if (t.itemType === "service") {
      countByService[label] = (countByService[label] || 0) + 1;
      if (!serviceLabels.includes(label)) serviceLabels.push(label);
    }
  });

  const allLabels = Array.from(new Set([...productLabels, ...serviceLabels]));

  const data = {
    labels: allLabels,
    datasets: [
      {
        label: "Produkty",
        data: allLabels.map((name) => countByProduct[name] || 0),
        backgroundColor: barColor,
      },
      {
        label: "Usługi",
        data: allLabels.map((name) => countByService[name] || 0),
        backgroundColor: serviceColor,
      },
    ],
  };

  return <Bar data={data} />;
};
