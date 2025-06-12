import { Bar } from "react-chartjs-2";
import { parseISO, format } from "date-fns";
import type { Transaction } from "../../../models/transaction";

interface TaxOverTimeChartProps {
  transactions: Transaction[];
  groupBy: "day" | "month" | "year";
  barColor: string;
}

export const TaxOverTimeChart: React.FC<TaxOverTimeChartProps> = ({
  transactions,
  groupBy,
  barColor,
}) => {
  const dataByGroup: { [key: string]: number } = {};

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

    const taxValue = t.totalAmount - t.totalAmountMinusTax;
    dataByGroup[key] = (dataByGroup[key] || 0) + taxValue;
  });

  const keys = Object.keys(dataByGroup).sort();

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
        label: "Suma podatku do zapłacenia",
        data: keys.map((k) => dataByGroup[k]),
        backgroundColor: barColor,
      },
    ],
  };
  return <Bar data={data} />;
};
