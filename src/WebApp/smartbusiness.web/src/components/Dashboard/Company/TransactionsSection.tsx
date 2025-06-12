import { useEffect, useState, useCallback } from "react";
import { useTransaction } from "../../../context/transaction/TransactionContext";
import GenericPagination from "./ProductServicesSection/GenericPagination";
import GenericSearchFilterBar from "./ProductServicesSection/GenericSearchFilterBar";
import { GenericTable } from "./ProductServicesSection/GenericTable";
import GenericModal from "./ProductServicesSection/GenericModal";
import type {
  EnrichedTransaction,
  Transaction,
} from "../../../models/transaction";
import type { ApiResponseError } from "../../../models/authErrors";

// Poprawka typów kolumn (key: keyof Transaction, align: 'left' | 'right' | 'center')
const columns = [
  {
    key: "itemName" as const,
    label: "Nazwa",
    align: "left" as const,
  },
  {
    key: "itemType" as const,
    label: "Typ",
    align: "left" as const,
  },
  {
    key: "quantity" as const,
    label: "Ilość",
    align: "left" as const,
  },
  {
    key: "totalAmount" as const,
    label: "Kwota brutto",
    align: "left" as const,
    render: (t: Transaction) => `${t.totalAmount.toFixed(2)} zł`,
  },
  {
    key: "tax" as const,
    label: "Podatek",
    align: "left" as const,
    render: (t: Transaction) => `${t.tax}%`,
  },
  {
    key: "totalAmountMinusTax" as const,
    label: "Kwota netto",
    align: "left" as const,
    render: (t: Transaction) => `${t.totalAmountMinusTax.toFixed(2)} zł`,
  },
  {
    key: "createdAt" as const,
    label: "Data",
    align: "left" as const,
    render: (t: Transaction) => new Date(t.createdAt).toLocaleString(),
  },
];

export const TransactionsSection = () => {
  const { fetchEnrichtedTransactions, deleteTransaction } = useTransaction();
  const [transactions, setTransactions] = useState<EnrichedTransaction[]>([]);
  const [search, setSearch] = useState("");
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [modalOpen, setModalOpen] = useState(false);
  const [toDeleteId, setToDeleteId] = useState<string | null>(null);

  const fetchAll = useCallback(async () => {
    try {
      const data = await fetchEnrichtedTransactions({});
      setTransactions(data);
    } catch (err) {
      const error = err as ApiResponseError;

      console.error("Błąd podczas pobierania transakcji:", error.title);

      // Możesz dodać obsługę błędów jeśli chcesz
    }
  }, [fetchEnrichtedTransactions]);

  useEffect(() => {
    fetchAll();
  }, [fetchAll]);

  // Proste filtrowanie po stringach
  const filtered = transactions
    .filter((t) => {
      if (!search) return true;
      const s = search.toLowerCase();
      return (
        t.itemName.toLowerCase().includes(s) ||
        t.itemType.toLowerCase().includes(s) ||
        String(t.quantity).includes(s) ||
        String(t.totalAmount).includes(s) ||
        String(t.tax).includes(s) ||
        String(t.totalAmountMinusTax).includes(s) ||
        new Date(t.createdAt).toLocaleString().toLowerCase().includes(s)
      );
    })
    .sort(
      (a, b) =>
        new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime(),
    );

  // Paginacja
  const paginated = filtered.slice((page - 1) * pageSize, page * pageSize);
  const pageCount = Math.max(1, Math.ceil(filtered.length / pageSize));

  const handleDelete = async () => {
    if (!toDeleteId) return;
    await deleteTransaction(toDeleteId);
    setModalOpen(false);
    setToDeleteId(null);
    fetchAll();
  };

  return (
    <div className="flex h-full w-full flex-col">
      <div className="mb-6 flex shrink-0 flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
        <GenericSearchFilterBar
          search={search}
          onSearchChange={setSearch}
          filterValue=""
          filterOptions={[]}
          onFilterChange={() => {}}
          showFilter={false}
        />
      </div>
      <div className="flex-1">
        <GenericTable
          columns={columns}
          data={paginated}
          onRowClick={(row: EnrichedTransaction) => {
            setToDeleteId(row.id);
            setModalOpen(true);
          }}
        />
        <GenericPagination
          page={page}
          pageCount={pageCount}
          pageSize={pageSize}
          onPageChange={setPage}
          onPageSizeChange={(val) => {
            setPageSize(val);
            setPage(1);
          }}
        />
      </div>
      <GenericModal
        open={modalOpen}
        title="Potwierdź usunięcie"
        onClose={() => setModalOpen(false)}
        actions={[
          <button
            key="delete"
            className="mr-2 rounded bg-red-500 px-4 py-2 text-white"
            onClick={handleDelete}
          >
            Usuń
          </button>,
          <button
            key="cancel"
            className="rounded bg-gray-200 px-4 py-2"
            onClick={() => setModalOpen(false)}
          >
            Anuluj
          </button>,
        ]}
      >
        Czy na pewno chcesz usunąć transakcję?
      </GenericModal>
    </div>
  );
};
