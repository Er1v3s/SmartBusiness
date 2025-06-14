import React, { useState } from "react";
import { Outlet } from "react-router-dom";
import { CompanySidebar } from "../components/Dashboard/Company/CompanySidebar";

export const DashboardPage: React.FC = () => {
  const [isSidebarCollapsed, setIsSidebarCollapsed] = useState(false);
  return (
    <div className="relative h-[calc(100vh-(var(--spacing)*16))] bg-gradient-to-br from-gray-100 to-gray-300 text-gray-900 dark:from-gray-800 dark:to-gray-700 dark:text-gray-100">
      {/* Sidebar (fixed) */}
      <CompanySidebar
        isCollapsed={isSidebarCollapsed}
        setIsCollapsed={setIsSidebarCollapsed}
      />

      {/* Main Content */}
      <div
        className={`h-full overflow-y-auto px-4 py-8 transition-all duration-200 sm:px-6 lg:px-8 ${
          isSidebarCollapsed ? "ml-16" : "ml-64"
        }`}
      >
        <Outlet />
      </div>
    </div>
  );
};
