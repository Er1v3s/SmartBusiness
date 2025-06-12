import { Bar } from "react-chartjs-2";
import type { Transaction } from "../../../models/transaction";

interface TransactionCountByProductChartProps {
  transactions: Transaction[];
  barColor: string;
}

export const TransactionCountByProductChart: React.FC<
  TransactionCountByProductChartProps
> = ({ transactions, barColor }) => {
  const countByProduct: { [productId: string]: number } = {};

  transactions.forEach((t) => {
    countByProduct[t.productId] = (countByProduct[t.productId] || 0) + 1;
  });

  const productIds = Object.keys(countByProduct);

  const data = {
    labels: productIds,
    datasets: [
      {
        label: "Liczba transakcji wg produktu",
        data: productIds.map((id) => countByProduct[id]),
        backgroundColor: barColor,
      },
    ],
  };

  return <Bar data={data} />;
};
