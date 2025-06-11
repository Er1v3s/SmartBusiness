import { Bar } from "react-chartjs-2";
import type { Transaction } from "../../../models/transaction";

interface TotalAmountByProductChartProps {
  transactions: Transaction[];
  barColor: string;
}

export const TotalAmountByProductChart: React.FC<
  TotalAmountByProductChartProps
> = ({ transactions, barColor }) => {
  const sumByProduct: { [productId: string]: number } = {};

  transactions.forEach((t) => {
    sumByProduct[t.productId] =
      (sumByProduct[t.productId] || 0) + t.totalAmount;
  });

  const productIds = Object.keys(sumByProduct);

  const data = {
    labels: productIds,
    datasets: [
      {
        label: "Suma sprzedaży wg produktu",
        data: productIds.map((id) => sumByProduct[id]),
        backgroundColor: barColor,
      },
    ],
  };

  return <Bar data={data} />;
};
