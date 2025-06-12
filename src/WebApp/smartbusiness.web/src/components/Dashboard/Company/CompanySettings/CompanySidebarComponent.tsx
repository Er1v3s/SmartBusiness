import { NavLink, useLocation } from "react-router-dom";

export const CompanySidebarComponent = () => {
  const location = useLocation();

  return (
    <aside className="w-64 flex-col gap-2 border-r border-gray-200 bg-white/80 px-4 py-10 text-gray-800 shadow-lg dark:border-gray-950 dark:bg-gray-900 dark:text-gray-100 dark:shadow-black/30">
      <div className="mb-8 flex items-center justify-between">
        <h1 className="text-2xl font-extrabold tracking-tight text-indigo-500">
          Panel firmy
        </h1>
      </div>

      <NavLink
        to="summary"
        className={({ isActive }) =>
          `flex w-full min-w-[180px] items-center gap-3 rounded-lg px-4 py-3 text-lg font-medium transition-colors duration-150 hover:bg-indigo-50 hover:text-indigo-700 dark:hover:bg-indigo-900 dark:hover:text-indigo-200 ${
            isActive || location.pathname === "/dashboard/company"
              ? "bg-indigo-100 font-bold text-indigo-700 dark:bg-indigo-900 dark:font-bold dark:text-indigo-200"
              : "text-gray-700 dark:text-gray-200"
          }`
        }
      >
        Podsumowanie
      </NavLink>
      <NavLink
        to="add"
        className={({ isActive }) =>
          `flex w-full min-w-[180px] items-center gap-3 rounded-lg px-4 py-3 text-lg font-medium transition-colors duration-150 hover:bg-indigo-50 hover:text-indigo-700 dark:hover:bg-indigo-900 dark:hover:text-indigo-200 ${
            isActive
              ? "bg-indigo-100 font-bold text-indigo-700 dark:bg-indigo-900 dark:font-bold dark:text-indigo-200"
              : "text-gray-700 dark:text-gray-200"
          }`
        }
      >
        Dodaj firmę
      </NavLink>
      <NavLink
        to="list"
        className={({ isActive }) =>
          `flex w-full min-w-[180px] items-center gap-3 rounded-lg px-4 py-3 text-lg font-medium transition-colors duration-150 hover:bg-indigo-50 hover:text-indigo-700 dark:hover:bg-indigo-900 dark:hover:text-indigo-200 ${
            isActive
              ? "bg-indigo-100 font-bold text-indigo-700 dark:bg-indigo-900 dark:font-bold dark:text-indigo-200"
              : "text-gray-700 dark:text-gray-200"
          }`
        }
      >
        Lista firm
      </NavLink>
      <NavLink
        to="delete"
        className={({ isActive }) =>
          `flex w-full min-w-[180px] items-center gap-3 rounded-lg px-4 py-3 text-lg font-medium transition-colors duration-150 hover:bg-indigo-50 hover:text-indigo-700 dark:hover:bg-indigo-900 dark:hover:text-indigo-200 ${
            isActive
              ? "bg-indigo-100 font-bold text-indigo-700 dark:bg-indigo-900 dark:font-bold dark:text-indigo-200"
              : "text-gray-700 dark:text-gray-200"
          }`
        }
      >
        Usuń firmę
      </NavLink>
    </aside>
  );
};
