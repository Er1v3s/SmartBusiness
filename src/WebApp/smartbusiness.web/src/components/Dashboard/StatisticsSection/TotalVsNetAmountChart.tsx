import { Bar } from "react-chartjs-2";
import { parseISO, format } from "date-fns";
import type { Transaction } from "../../../models/transaction";

interface TotalVsNetAmountChartProps {
  transactions: Transaction[];
  groupBy: "day" | "month" | "year";
  barColor: string;
  serviceColor?: string;
}

export const TotalVsNetAmountChart: React.FC<TotalVsNetAmountChartProps> = ({
  transactions,
  groupBy,
  barColor,
  serviceColor = "#a21caf",
}) => {
  // Split into two series: products and services, for both Brutto and Netto
  const totalByGroupProduct: { [key: string]: number } = {};
  const netByGroupProduct: { [key: string]: number } = {};
  const totalByGroupService: { [key: string]: number } = {};
  const netByGroupService: { [key: string]: number } = {};

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
      totalByGroupProduct[key] =
        (totalByGroupProduct[key] || 0) + t.totalAmount;
      netByGroupProduct[key] =
        (netByGroupProduct[key] || 0) + t.totalAmountMinusTax;
    } else if (t.itemType === "service") {
      totalByGroupService[key] =
        (totalByGroupService[key] || 0) + t.totalAmount;
      netByGroupService[key] =
        (netByGroupService[key] || 0) + t.totalAmountMinusTax;
    }
  });

  const allKeys = Array.from(
    new Set([
      ...Object.keys(totalByGroupProduct),
      ...Object.keys(totalByGroupService),
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
        label: "Brutto (Produkty)",
        data: allKeys.map((k) => totalByGroupProduct[k] || 0),
        backgroundColor: barColor,
      },
      {
        label: "Brutto (Usługi)",
        data: allKeys.map((k) => totalByGroupService[k] || 0),
        backgroundColor: serviceColor,
      },
      {
        label: "Netto (Produkty)",
        data: allKeys.map((k) => netByGroupProduct[k] || 0),
        backgroundColor: barColor + "99",
      },
      {
        label: "Netto (Usługi)",
        data: allKeys.map((k) => netByGroupService[k] || 0),
        backgroundColor: serviceColor + "99",
      },
    ],
  };

  return <Bar data={data} />;
};
