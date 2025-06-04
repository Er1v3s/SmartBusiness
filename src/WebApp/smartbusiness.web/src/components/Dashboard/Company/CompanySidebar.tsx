import { NavLink } from "react-router-dom";
import {
  Calendar,
  BarChart2,
  Settings,
  Home,
  ShoppingCart,
} from "lucide-react";
import { useCompany } from "../../../context/company/CompanyContext";
import { useEffect, useState } from "react";

interface CompanySidebarProps {
  isCollapsed: boolean;
  setIsCollapsed: (collapsed: boolean) => void;
}

export const CompanySidebar: React.FC<CompanySidebarProps> = ({
  isCollapsed,
  setIsCollapsed,
}) => {
  const { company } = useCompany();
  const [showNav, setShowNav] = useState(!isCollapsed);

  useEffect(() => {
    let timeout: number;
    if (!isCollapsed) {
      // Delay showing the navigation items to allow for smooth transition
      timeout = setTimeout(() => setShowNav(true), 120);
    } else {
      setShowNav(false);
    }
    return () => clearTimeout(timeout);
  }, [isCollapsed]);

  return (
    <aside
      className={`fixed top-0 left-0 z-40 flex h-full flex-col border-r border-gray-200 bg-white/90 px-4 py-8 shadow-lg transition-all duration-200 dark:border-gray-900 dark:bg-gray-900/90 ${
        isCollapsed ? "w-16 px-2" : "w-64 px-4"
      }`}
    >
      <div
        className={`mb-10 flex items-center gap-2 ${
          isCollapsed ? "justify-center" : ""
        }`}
      >
        <span
          className={`truncate text-xl font-bold text-indigo-700 transition-opacity duration-200 dark:text-indigo-200 ${
            isCollapsed ? "w-0 opacity-0" : "w-auto opacity-100"
          }`}
        >
          {company?.name || "Brak firmy"}
        </span>
      </div>
      <nav className="flex flex-1 flex-col gap-2 transition-opacity duration-200">
        <NavLink
          to="/dashboard"
          end
          className={({ isActive }) =>
            `flex items-center rounded-lg px-4 py-3 text-base font-medium transition-colors duration-200 ${
              isActive
                ? "bg-indigo-100 text-indigo-700 dark:bg-gray-800 dark:text-indigo-200"
                : "text-gray-700 hover:bg-indigo-50 hover:text-indigo-700 dark:text-gray-200 dark:hover:bg-gray-800 dark:hover:text-indigo-200"
            } ${isCollapsed ? "justify-center px-0" : "gap-3 px-4"}`
          }
        >
          <span className="flex h-6 w-6 items-center justify-center">
            <Home className="h-5 w-5" />
          </span>
          <span
            className={`transition-all duration-200 ${
              !isCollapsed && showNav
                ? "w-auto translate-x-0 opacity-100 delay-100"
                : "w-0 translate-x-2 overflow-hidden opacity-0 delay-0"
            }`}
            style={{
              display: !isCollapsed && showNav ? "inline-block" : "none",
            }}
          >
            Dashboard
          </span>
        </NavLink>
        <NavLink
          to="/dashboard/company/calendar"
          className={({ isActive }) =>
            `flex items-center rounded-lg px-4 py-3 text-base font-medium transition-colors duration-200 ${
              isActive
                ? "bg-indigo-100 text-indigo-700 dark:bg-gray-800 dark:text-indigo-200"
                : "text-gray-700 hover:bg-indigo-50 hover:text-indigo-700 dark:text-gray-200 dark:hover:bg-gray-800 dark:hover:text-indigo-200"
            } ${isCollapsed ? "justify-center px-0" : "gap-3 px-4"}`
          }
        >
          <span className="flex h-6 w-6 items-center justify-center">
            <Calendar className="h-5 w-5" />
          </span>
          <span
            className={`transition-all duration-200 ${
              !isCollapsed && showNav
                ? "w-auto translate-x-0 opacity-100 delay-100"
                : "w-0 translate-x-2 overflow-hidden opacity-0 delay-0"
            }`}
            style={{
              display: !isCollapsed && showNav ? "inline-block" : "none",
            }}
          >
            Kalendarz
          </span>
        </NavLink>
        <NavLink
          to="/dashboard/company/sale"
          className={({ isActive }) =>
            `flex items-center rounded-lg px-4 py-3 text-base font-medium transition-colors duration-200 ${
              isActive
                ? "bg-indigo-100 text-indigo-700 dark:bg-gray-800 dark:text-indigo-200"
                : "text-gray-700 hover:bg-indigo-50 hover:text-indigo-700 dark:text-gray-200 dark:hover:bg-gray-800 dark:hover:text-indigo-200"
            } ${isCollapsed ? "justify-center px-0" : "gap-3 px-4"}`
          }
        >
          <span className="flex h-6 w-6 items-center justify-center">
            <ShoppingCart className="h-5 w-5" />
          </span>
          <span
            className={`transition-all duration-200 ${
              !isCollapsed && showNav
                ? "w-auto translate-x-0 opacity-100 delay-100"
                : "w-0 translate-x-2 overflow-hidden opacity-0 delay-0"
            }`}
            style={{
              display: !isCollapsed && showNav ? "inline-block" : "none",
            }}
          >
            Rejestruj sprzedaż
          </span>
        </NavLink>
        <NavLink
          to="/dashboard/company/stats"
          className={({ isActive }) =>
            `flex items-center rounded-lg px-4 py-3 text-base font-medium transition-colors duration-200 ${
              isActive
                ? "bg-indigo-100 text-indigo-700 dark:bg-gray-800 dark:text-indigo-200"
                : "text-gray-700 hover:bg-indigo-50 hover:text-indigo-700 dark:text-gray-200 dark:hover:bg-gray-800 dark:hover:text-indigo-200"
            } ${isCollapsed ? "justify-center px-0" : "gap-3 px-4"}`
          }
        >
          <span className="flex h-6 w-6 items-center justify-center">
            <BarChart2 className="h-5 w-5" />
          </span>
          <span
            className={`transition-all duration-200 ${
              !isCollapsed && showNav
                ? "w-auto translate-x-0 opacity-100 delay-100"
                : "w-0 translate-x-2 overflow-hidden opacity-0 delay-0"
            }`}
            style={{
              display: !isCollapsed && showNav ? "inline-block" : "none",
            }}
          >
            Statystyki
          </span>
        </NavLink>
      </nav>
      <div className="mt-auto flex flex-col gap-2">
        <NavLink
          to="/dashboard/company/settings"
          className={({ isActive }) =>
            `flex items-center rounded-lg px-4 py-3 text-base font-medium transition-colors duration-200 ${
              isActive
                ? "bg-indigo-100 text-indigo-700 dark:bg-gray-800 dark:text-indigo-200"
                : "text-gray-700 hover:bg-indigo-50 hover:text-indigo-700 dark:text-gray-200 dark:hover:bg-gray-800 dark:hover:text-indigo-200"
            } ${isCollapsed ? "justify-center px-0" : "gap-3 px-4"}`
          }
        >
          <span className="flex h-6 w-6 items-center justify-center">
            <Settings className="h-5 w-5" />
          </span>
          <span
            className={`transition-all duration-200 ${
              isCollapsed
                ? "w-0 translate-x-2 overflow-hidden opacity-0 delay-0"
                : "w-auto translate-x-0 opacity-100 delay-100"
            }`}
            style={{ display: isCollapsed ? "none" : "inline-block" }}
          >
            Ustawienia firmy
          </span>
        </NavLink>
        <button
          type="button"
          aria-label={isCollapsed ? "Rozwiń sidebar" : "Zwiń sidebar"}
          onClick={() => setIsCollapsed(!isCollapsed)}
          className="mt-2 flex w-full items-center justify-center rounded-lg px-2 py-2 text-indigo-700 transition-colors hover:bg-indigo-50 dark:text-indigo-200 dark:hover:bg-gray-800"
        >
          {isCollapsed ? (
            <svg
              xmlns="http://www.w3.org/2000/svg"
              className="h-5 w-5"
              fill="none"
              viewBox="0 0 24 24"
              stroke="currentColor"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth={2}
                d="M9 5l7 7-7 7"
              />
            </svg>
          ) : (
            <svg
              xmlns="http://www.w3.org/2000/svg"
              className="h-5 w-5"
              fill="none"
              viewBox="0 0 24 24"
              stroke="currentColor"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth={2}
                d="M15 19l-7-7 7-7"
              />
            </svg>
          )}
        </button>
      </div>
    </aside>
  );
};
