import React from "react";
import { ArrowLeft, ArrowRight } from "lucide-react";

interface GenericPaginationProps {
  page: number;
  pageCount: number;
  pageSize: number;
  pageSizeOptions?: number[];
  onPageChange: (page: number) => void;
  onPageSizeChange: (size: number) => void;
}

const GenericPagination: React.FC<GenericPaginationProps> = ({
  page,
  pageCount,
  pageSize,
  pageSizeOptions = [5, 10, 15],
  onPageChange,
  onPageSizeChange,
}) => (
  <div className="relative mt-4 flex min-w-[180px] flex-wrap items-center justify-center gap-4">
    <div className="mx-auto flex items-center gap-4">
      <button
        className="rounded bg-gray-100 px-2 py-1 text-gray-700 shadow disabled:opacity-30 dark:bg-gray-800 dark:text-gray-100"
        onClick={() => onPageChange(Math.max(1, page - 1))}
        disabled={page === 1}
        aria-label="Poprzednia strona"
      >
        <ArrowLeft />
      </button>
      <span className="text-sm font-medium text-gray-800 select-none dark:text-gray-100">
        {page}/{pageCount}
      </span>
      <button
        className="rounded bg-gray-100 px-2 py-1 text-gray-700 shadow disabled:opacity-30 dark:bg-gray-800 dark:text-gray-100"
        onClick={() => onPageChange(Math.min(pageCount, page + 1))}
        disabled={page === pageCount}
        aria-label="Następna strona"
      >
        <ArrowRight />
      </button>
    </div>
    <div className="absolute right-0 flex items-center gap-1 pl-6 text-xs text-gray-800 dark:text-gray-100">
      <span>Ilość na stronę:</span>
      <select
        value={pageSize}
        onChange={(e) => onPageSizeChange(Number(e.target.value))}
        className="input w-16 bg-gray-100 px-1 py-0.5 text-xs text-gray-800 shadow dark:bg-gray-800 dark:text-gray-100"
      >
        {pageSizeOptions.map((val) => (
          <option key={val} value={val}>
            {val}
          </option>
        ))}
      </select>
    </div>
  </div>
);

export default GenericPagination;
