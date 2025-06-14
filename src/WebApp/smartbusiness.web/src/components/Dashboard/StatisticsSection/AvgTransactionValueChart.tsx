import { Line } from "react-chartjs-2";
import {
  Chart as ChartJS,
  LineElement,
  PointElement,
  LinearScale,
  CategoryScale,
  Tooltip,
  Legend,
} from "chart.js";
ChartJS.register(
  LineElement,
  PointElement,
  LinearScale,
  CategoryScale,
  Tooltip,
  Legend,
);
import { parseISO, format } from "date-fns";
import type { EnrichedTransaction } from "../../../models/transaction";

interface AvgTransactionValueChartProps {
  transactions: EnrichedTransaction[];
  groupBy: "day" | "month" | "year";
  barColor: string;
  serviceColor?: string;
}

export const AvgTransactionValueChart: React.FC<
  AvgTransactionValueChartProps
> = ({ transactions, groupBy, barColor, serviceColor = "#a21caf" }) => {
  // Dwie serie: produkty i usługi
  const sumByGroupProduct: { [key: string]: number } = {};
  const countByGroupProduct: { [key: string]: number } = {};
  const sumByGroupService: { [key: string]: number } = {};
  const countByGroupService: { [key: string]: number } = {};

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
      sumByGroupProduct[key] = (sumByGroupProduct[key] || 0) + t.totalAmount;
      countByGroupProduct[key] = (countByGroupProduct[key] || 0) + 1;
    } else if (t.itemType === "service") {
      sumByGroupService[key] = (sumByGroupService[key] || 0) + t.totalAmount;
      countByGroupService[key] = (countByGroupService[key] || 0) + 1;
    }
  });

  const allKeys = Array.from(
    new Set([
      ...Object.keys(sumByGroupProduct),
      ...Object.keys(sumByGroupService),
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
        label: "Średnia wartość transakcji (produkty)",
        data: allKeys.map((k) =>
          countByGroupProduct[k]
            ? sumByGroupProduct[k] / countByGroupProduct[k]
            : 0,
        ),
        borderColor: barColor,
        backgroundColor: barColor + "33",
        fill: true,
        tension: 0.3,
      },
      {
        label: "Średnia wartość transakcji (usługi)",
        data: allKeys.map((k) =>
          countByGroupService[k]
            ? sumByGroupService[k] / countByGroupService[k]
            : 0,
        ),
        borderColor: serviceColor,
        backgroundColor: serviceColor + "33",
        fill: true,
        tension: 0.3,
      },
    ],
  };

  const options = {
    maintainAspectRatio: false,
    responsive: true,
    plugins: {
      legend: { position: "top" as const },
    },
  };

  return <Line data={data} options={options} />;
};
