import React from "react";

interface TableColumn<T> {
  key: keyof T;
  label: string;
  align?: "left" | "right" | "center";
  render?: (row: T) => React.ReactNode;
}

interface GenericTableProps<T> {
  columns: TableColumn<T>[];
  data: T[];
  onRowClick?: (row: T) => void;
  renderActions?: (row: T) => React.ReactNode;
}

export function GenericTable<T extends { id: number | string }>({
  columns,
  data,
  onRowClick,
  renderActions,
}: GenericTableProps<T>) {
  return (
    <div className="overflow-x-auto rounded-lg bg-white shadow dark:bg-gray-800">
      <table className="min-w-full text-sm">
        <thead>
          <tr className="bg-gray-100 dark:bg-gray-700">
            {columns.map((col) => (
              <th
                key={String(col.key)}
                className={`px-4 py-2 text-${col.align || "left"}`}
              >
                {col.label}
              </th>
            ))}
          </tr>
        </thead>
        <tbody>
          {data.length === 0 ? (
            <tr>
              <td
                colSpan={columns.length}
                className="py-6 text-center text-gray-400"
              >
                Brak wyników
              </td>
            </tr>
          ) : (
            data.map((row) => (
              <tr
                key={row.id}
                className="cursor-pointer border-b border-gray-100 transition hover:bg-indigo-50/40 dark:border-gray-700 dark:hover:bg-gray-700/40"
                onClick={onRowClick ? () => onRowClick(row) : undefined}
              >
                {columns.map((col) => (
                  <td
                    key={String(col.key)}
                    className={`px-4 py-2${
                      col.align === "right"
                        ? "text-right"
                        : col.align === "center"
                          ? "text-center"
                          : ""
                    }${col.key === "name" ? "font-medium" : ""}`}
                  >
                    {col.render
                      ? col.render(row)
                      : (row[col.key] as React.ReactNode)}
                  </td>
                ))}
                {renderActions && (
                  <td className="px-4 py-2">{renderActions(row)}</td>
                )}
              </tr>
            ))
          )}
        </tbody>
      </table>
    </div>
  );
}
