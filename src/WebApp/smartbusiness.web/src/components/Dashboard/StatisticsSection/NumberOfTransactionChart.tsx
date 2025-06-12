import { Bar } from "react-chartjs-2";
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  BarElement,
  Title,
  Tooltip,
  Legend,
} from "chart.js";
import { parseISO, format } from "date-fns";
import type { Transaction } from "../../../models/transaction";

ChartJS.register(
  CategoryScale,
  LinearScale,
  BarElement,
  Title,
  Tooltip,
  Legend,
);

interface TransactionBarChartProps {
  transactions: Transaction[];
  groupBy: "day" | "month" | "year";
  barColor: string;
  serviceColor?: string;
  dateFrom: string;
  dateTo: string;
}

export const TransactionBarChart: React.FC<TransactionBarChartProps> = ({
  transactions,
  groupBy,
  barColor,
  serviceColor = "#6366f1",
}) => {
  // Dwie mapy: osobno dla produktów i usług
  const dataByGroupProduct: { [key: string]: number } = {};
  const dataByGroupService: { [key: string]: number } = {};

  transactions.forEach((t) => {
    const date = parseISO(t.createdAt);
    let key = "";

    if (groupBy === "day") {
      key = format(date, "yyyy-MM-dd");
    } else if (groupBy === "month") {
      key = format(date, "yyyy-MM");
    } else if (groupBy === "year") {
      key = format(date, "yyyy");
    }

    if (t.itemType === "product") {
      dataByGroupProduct[key] = (dataByGroupProduct[key] || 0) + 1;
    } else if (t.itemType === "service") {
      dataByGroupService[key] = (dataByGroupService[key] || 0) + 1;
    }
  });

  // Zbierz wszystkie klucze (daty) z obu map
  const allKeys = Array.from(
    new Set([
      ...Object.keys(dataByGroupProduct),
      ...Object.keys(dataByGroupService),
    ]),
  ).sort();

  const displayLabels = allKeys.map((k) => {
    if (groupBy === "day") {
      return format(parseISO(k), "dd.MM.yyyy");
    }
    if (groupBy === "month") {
      return format(parseISO(k + "-01"), "LLL yyyy");
    }
    if (groupBy === "year") {
      return k;
    }
    return k;
  });

  const data = {
    labels: displayLabels,
    datasets: [
      {
        label: "Produkty",
        data: allKeys.map((k) => dataByGroupProduct[k] || 0),
        backgroundColor: barColor,
      },
      {
        label: "Usługi",
        data: allKeys.map((k) => dataByGroupService[k] || 0),
        backgroundColor: serviceColor,
      },
    ],
  };

  return <Bar data={data} />;
};
