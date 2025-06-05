import React from "react";
import { Search } from "lucide-react";

interface GenericSearchFilterBarProps {
  search: string;
  onSearchChange: (v: string) => void;
  filterLabel?: string;
  filterValue: string;
  filterOptions: string[];
  onFilterChange: (v: string) => void;
  showFilter: boolean;
  onToggleFilter: () => void;
}

const GenericSearchFilterBar: React.FC<GenericSearchFilterBarProps> = ({
  search,
  onSearchChange,
  filterLabel = "Filtruj",
  filterValue,
  filterOptions,
  onFilterChange,
  showFilter,
  onToggleFilter,
}) => (
  <div className="mb-4 flex flex-wrap items-center gap-2">
    <div className="flex flex-1 items-center gap-2">
      <label htmlFor="GenericSearchInput">
        <div className="relative">
          <input
            id="GenericSearchInput"
            type="search"
            placeholder="Szukaj..."
            value={search}
            onChange={(e) => onSearchChange(e.target.value)}
            className="rounded-lg border-2 border-gray-300 bg-white p-3 text-gray-800 shadow-sm md:min-w-md dark:border-gray-700 dark:bg-gray-800 dark:text-gray-100"
          />
          <span className="absolute inset-y-0 right-2 grid w-8 place-content-center">
            <button
              type="button"
              aria-label="Submit"
              className="rounded-full p-1.5 text-gray-700 transition-colors hover:bg-gray-100 dark:text-gray-200 dark:hover:bg-gray-800"
            >
              <Search />
            </button>
          </span>
        </div>
      </label>
    </div>
    <button
      type="button"
      className="rounded bg-gray-100 px-3 py-2 text-sm text-gray-700 shadow hover:bg-gray-200 dark:bg-gray-700 dark:text-gray-100 dark:hover:bg-gray-600"
      onClick={onToggleFilter}
    >
      {filterLabel}
    </button>
    {showFilter && (
      <select
        value={filterValue}
        onChange={(e) => onFilterChange(e.target.value)}
        className="input rounded-lg border-2 border-gray-300 bg-gray-50 px-3 py-2 text-gray-800 dark:border-gray-700 dark:bg-gray-800 dark:text-gray-200"
      >
        {filterOptions.map((opt) => (
          <option key={opt}>{opt}</option>
        ))}
      </select>
    )}
  </div>
);

export default GenericSearchFilterBar;
