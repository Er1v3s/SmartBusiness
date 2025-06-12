import { Bar } from "react-chartjs-2";
import { parseISO, format } from "date-fns";
import type { Transaction } from "../../../models/transaction";

interface TotalVsNetAmountChartProps {
  transactions: Transaction[];
  groupBy: "day" | "month" | "year";
  barColor: string;
}

export const TotalVsNetAmountChart: React.FC<TotalVsNetAmountChartProps> = ({
  transactions,
  groupBy,
  barColor,
}) => {
  const totalByGroup: { [key: string]: number } = {};
  const netByGroup: { [key: string]: number } = {};

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

    totalByGroup[key] = (totalByGroup[key] || 0) + t.totalAmount;
    netByGroup[key] = (netByGroup[key] || 0) + t.totalAmountMinusTax;
  });

  const keys = Object.keys(totalByGroup).sort();

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
        label: "Brutto (TotalAmount)",
        data: keys.map((k) => totalByGroup[k]),
        backgroundColor: barColor,
      },
      {
        label: "Netto (TotalAmountMinusTax)",
        data: keys.map((k) => netByGroup[k]),
        backgroundColor: "#16a34a",
      },
    ],
  };

  return <Bar data={data} />;
};
