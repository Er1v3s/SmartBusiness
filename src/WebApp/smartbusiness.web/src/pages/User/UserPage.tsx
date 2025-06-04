import React from "react";
import { useAuth } from "../../context/auth/AuthContext";
import { UserSidebarComponent } from "../../components/Dashboard/User/UserSidebarComponent";
import { Outlet } from "react-router-dom";

export const UserPage: React.FC = () => {
  const { user } = useAuth();

  return (
    <div className="flex h-[calc(100vh-(var(--spacing)*16))] bg-gradient-to-br from-gray-100 to-gray-300 dark:from-gray-800 dark:to-gray-700">
      {/* Sidebar */}
      <UserSidebarComponent />

      {/* Main content */}
      <main className="flex flex-1 items-start justify-center bg-transparent px-4 py-12 text-gray-700 dark:text-gray-100">
        <div className="w-full max-w-2xl">
          <Outlet context={{ user }} />
        </div>
      </main>
    </div>
  );
};
