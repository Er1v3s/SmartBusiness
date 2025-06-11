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
import type { Transaction } from "../../../models/transaction";

interface AvgTransactionValueChartProps {
  transactions: Transaction[];
  groupBy: "day" | "month" | "year";
  barColor: string;
}

export const AvgTransactionValueChart: React.FC<
  AvgTransactionValueChartProps
> = ({ transactions, groupBy, barColor }) => {
  const sumByGroup: { [key: string]: number } = {};
  const countByGroup: { [key: string]: number } = {};

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

    sumByGroup[key] = (sumByGroup[key] || 0) + t.totalAmount;
    countByGroup[key] = (countByGroup[key] || 0) + 1;
  });

  const keys = Object.keys(sumByGroup).sort();

  const displayLabels = keys.map((k) => {
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
        label: "Średnia wartość transakcji",
        data: keys.map((k) =>
          countByGroup[k] ? sumByGroup[k] / countByGroup[k] : 0,
        ),
        borderColor: barColor,
        backgroundColor: barColor + "33",
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
