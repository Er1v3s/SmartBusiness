import { NavLink } from "react-router-dom";
import {
  Calendar,
  BarChart2,
  Settings,
  Home,
  ShoppingCart,
  PackageOpen,
  Handshake,
  ChevronRight,
  ChevronLeft,
  Banknote,
  Bot,
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
  const isDisabled = !company;

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
      className={`fixed left-0 z-40 flex h-[calc(100vh-var(--spacing)*16))] flex-col border-r border-gray-200 bg-white/90 px-4 py-8 shadow-lg transition-all duration-200 dark:border-gray-900 dark:bg-gray-900/90 ${
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
        {/* DASHBOARD */}
        <NavLink
          to="summary"
          className={({ isActive }) =>
            `flex items-center rounded-lg px-4 py-3 text-base font-medium transition-colors duration-200 ${
              isActive
                ? "bg-indigo-100 text-indigo-700 dark:bg-gray-800 dark:text-indigo-200"
                : "text-gray-700 hover:bg-indigo-50 hover:text-indigo-700 dark:text-gray-200 dark:hover:bg-gray-800 dark:hover:text-indigo-200"
            } ${isCollapsed ? "justify-center px-0" : "gap-3 px-4"} ${
              isDisabled ? "pointer-events-none opacity-50" : ""
            }`
          }
          tabIndex={isDisabled ? -1 : undefined}
          aria-disabled={isDisabled}
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

        {/* CALENDAR */}
        <NavLink
          to="calendar"
          className={({ isActive }) =>
            `flex items-center rounded-lg px-4 py-3 text-base font-medium transition-colors duration-200 ${
              isActive
                ? "bg-indigo-100 text-indigo-700 dark:bg-gray-800 dark:text-indigo-200"
                : "text-gray-700 hover:bg-indigo-50 hover:text-indigo-700 dark:text-gray-200 dark:hover:bg-gray-800 dark:hover:text-indigo-200"
            } ${isCollapsed ? "justify-center px-0" : "gap-3 px-4"} ${
              isDisabled ? "pointer-events-none opacity-50" : ""
            }`
          }
          tabIndex={isDisabled ? -1 : undefined}
          aria-disabled={isDisabled}
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

        {/* SALE PANEL */}
        <NavLink
          to="sales-panel"
          className={({ isActive }) =>
            `flex items-center rounded-lg px-4 py-3 text-base font-medium transition-colors duration-200 ${
              isActive
                ? "bg-indigo-100 text-indigo-700 dark:bg-gray-800 dark:text-indigo-200"
                : "text-gray-700 hover:bg-indigo-50 hover:text-indigo-700 dark:text-gray-200 dark:hover:bg-gray-800 dark:hover:text-indigo-200"
            } ${isCollapsed ? "justify-center px-0" : "gap-3 px-4"} ${
              isDisabled ? "pointer-events-none opacity-50" : ""
            }`
          }
          tabIndex={isDisabled ? -1 : undefined}
          aria-disabled={isDisabled}
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

        {/* SERVICE SECTION */}
        <NavLink
          to="services"
          className={({ isActive }) =>
            `flex items-center rounded-lg px-4 py-3 text-base font-medium transition-colors duration-200 ${
              isActive
                ? "bg-indigo-100 text-indigo-700 dark:bg-gray-800 dark:text-indigo-200"
                : "text-gray-700 hover:bg-indigo-50 hover:text-indigo-700 dark:text-gray-200 dark:hover:bg-gray-800 dark:hover:text-indigo-200"
            } ${isCollapsed ? "justify-center px-0" : "gap-3 px-4"} ${
              isDisabled ? "pointer-events-none opacity-50" : ""
            }`
          }
          tabIndex={isDisabled ? -1 : undefined}
          aria-disabled={isDisabled}
        >
          <span className="flex h-6 w-6 items-center justify-center">
            <Handshake className="h-5 w-5" />
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
            Usługi
          </span>
        </NavLink>

        {/* PRODUCTS SECTION */}
        <NavLink
          to="products"
          className={({ isActive }) =>
            `flex items-center rounded-lg px-4 py-3 text-base font-medium transition-colors duration-200 ${
              isActive
                ? "bg-indigo-100 text-indigo-700 dark:bg-gray-800 dark:text-indigo-200"
                : "text-gray-700 hover:bg-indigo-50 hover:text-indigo-700 dark:text-gray-200 dark:hover:bg-gray-800 dark:hover:text-indigo-200"
            } ${isCollapsed ? "justify-center px-0" : "gap-3 px-4"} ${
              isDisabled ? "pointer-events-none opacity-50" : ""
            }`
          }
          tabIndex={isDisabled ? -1 : undefined}
          aria-disabled={isDisabled}
        >
          <span className="flex h-6 w-6 items-center justify-center">
            <PackageOpen className="h-5 w-5" />
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
            Produkty
          </span>
        </NavLink>

        {/* TRANSACTION SECTION */}
        <NavLink
          to="transactions"
          className={({ isActive }) =>
            `flex items-center rounded-lg px-4 py-3 text-base font-medium transition-colors duration-200 ${
              isActive
                ? "bg-indigo-100 text-indigo-700 dark:bg-gray-800 dark:text-indigo-200"
                : "text-gray-700 hover:bg-indigo-50 hover:text-indigo-700 dark:text-gray-200 dark:hover:bg-gray-800 dark:hover:text-indigo-200"
            } ${isCollapsed ? "justify-center px-0" : "gap-3 px-4"} ${
              isDisabled ? "pointer-events-none opacity-50" : ""
            }`
          }
          tabIndex={isDisabled ? -1 : undefined}
          aria-disabled={isDisabled}
        >
          <span className="flex h-6 w-6 items-center justify-center">
            <Banknote className="h-5 w-5" />
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
            Transakcje
          </span>
        </NavLink>

        {/* STATISTICS SECTION */}
        <NavLink
          to="statistics"
          className={({ isActive }) =>
            `flex items-center rounded-lg px-4 py-3 text-base font-medium transition-colors duration-200 ${
              isActive
                ? "bg-indigo-100 text-indigo-700 dark:bg-gray-800 dark:text-indigo-200"
                : "text-gray-700 hover:bg-indigo-50 hover:text-indigo-700 dark:text-gray-200 dark:hover:bg-gray-800 dark:hover:text-indigo-200"
            } ${isCollapsed ? "justify-center px-0" : "gap-3 px-4"} ${
              isDisabled ? "pointer-events-none opacity-50" : ""
            }`
          }
          tabIndex={isDisabled ? -1 : undefined}
          aria-disabled={isDisabled}
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

        {/* PREDICTIONS SECTION */}
        <NavLink
          to="predictions"
          className={({ isActive }) =>
            `flex items-center rounded-lg px-4 py-3 text-base font-medium transition-colors duration-200 ${
              isActive
                ? "bg-indigo-100 text-indigo-700 dark:bg-gray-800 dark:text-indigo-200"
                : "text-gray-700 hover:bg-indigo-50 hover:text-indigo-700 dark:text-gray-200 dark:hover:bg-gray-800 dark:hover:text-indigo-200"
            } ${isCollapsed ? "justify-center px-0" : "gap-3 px-4"} ${
              isDisabled ? "pointer-events-none opacity-50" : ""
            }`
          }
          tabIndex={isDisabled ? -1 : undefined}
          aria-disabled={isDisabled}
        >
          <span className="flex h-6 w-6 items-center justify-center">
            <Bot className="h-5 w-5" />
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
            Predykcje (AI)
          </span>
        </NavLink>
      </nav>

      {/* COMPANY SETTINGS SECTION */}
      <div className="mt-auto flex flex-col gap-2">
        <NavLink
          to="company/settings/summary"
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

        {/* HIDE SEIDEBAR */}
        <button
          type="button"
          aria-label={isCollapsed ? "Rozwiń sidebar" : "Zwiń sidebar"}
          onClick={() => setIsCollapsed(!isCollapsed)}
          className="mt-2 flex w-full items-center justify-center rounded-lg px-2 py-2 text-indigo-700 transition-colors hover:bg-indigo-50 dark:text-indigo-200 dark:hover:bg-gray-800"
        >
          {isCollapsed ? <ChevronRight /> : <ChevronLeft />}
        </button>
      </div>
    </aside>
  );
};
