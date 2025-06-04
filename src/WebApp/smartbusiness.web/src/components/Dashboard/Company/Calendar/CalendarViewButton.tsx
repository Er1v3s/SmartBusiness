interface CalendarViewButtonProps {
  active: boolean;
  onClick: () => void;
  children: React.ReactNode;
}

export const CalendarViewButton = ({
  active,
  onClick,
  children,
}: CalendarViewButtonProps) => {
  return (
    <button
      className={`focus:outline-none" rounded bg-gradient-to-r from-indigo-600 to-indigo-500 px-4 py-2 font-semibold text-white shadow transition hover:from-indigo-700 hover:to-indigo-600 focus:ring-2 focus:ring-indigo-400 ${
        active
          ? "bg-indigo-600 text-white shadow"
          : "border border-gray-200 bg-white text-indigo-700 hover:bg-indigo-50 dark:border-gray-700 dark:bg-gray-800 dark:text-indigo-200 dark:hover:bg-gray-900"
      } `}
      onClick={onClick}
    >
      {children}
    </button>
  );
};
