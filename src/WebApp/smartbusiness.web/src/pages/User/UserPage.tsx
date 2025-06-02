import React, { useState } from "react";
import { useAuth } from "../../context/auth/AuthContext";
import { SummaryComponent } from "../../components/Dashboard/User/Summary";
import { EditProfileComponent } from "../../components/Dashboard/User/EditProfile";
import { ChangePasswordComponent } from "../../components/Dashboard/User/ChangePassword";
import { StatsComponent } from "../../components/Dashboard/User/Stats";
import { DeleteAccountComponent } from "../../components/Dashboard/User/DeleteAccount";
import { House, Pencil, Lock, ChartNoAxesCombined, Trash2 } from "lucide-react";

// Widoki panelu
const VIEWS = {
  SUMMARY: "Podsumowanie",
  EDIT_PROFILE: "Edycja profilu",
  CHANGE_PASSWORD: "Zmień hasło",
  STATS: "Statystyki",
  DELETE_ACCOUNT: "Usuń konto",
};

// Sidebar menu
const menu = [
  { label: VIEWS.SUMMARY, icon: <House /> },
  { label: VIEWS.EDIT_PROFILE, icon: <Pencil /> },
  { label: VIEWS.CHANGE_PASSWORD, icon: <Lock /> },
  { label: VIEWS.STATS, icon: <ChartNoAxesCombined /> },
  { label: VIEWS.DELETE_ACCOUNT, icon: <Trash2 /> },
];

export const UserPage: React.FC = () => {
  const { user } = useAuth();
  const [view, setView] = useState(VIEWS.SUMMARY);

  return (
    <div className="flex h-[calc(100vh-(var(--spacing)*16))] bg-gradient-to-br from-gray-100 to-gray-300 dark:from-gray-900 dark:to-gray-700">
      {/* Sidebar */}
      <aside className="w-64 flex-col gap-2 border-r border-gray-200 bg-white/80 px-4 py-10 text-gray-800 shadow-lg dark:border-gray-950 dark:bg-gray-900 dark:text-gray-100 dark:shadow-black/30">
        <div className="mb-8 flex items-center justify-between">
          <h1 className="text-2xl font-extrabold tracking-tight text-indigo-500 dark:text-indigo-300">
            Panel użytkownika
          </h1>
        </div>
        {menu.map((item) => (
          <button
            key={item.label}
            onClick={() => setView(item.label)}
            className={`flex items-center gap-3 rounded-lg px-4 py-3 text-lg font-medium transition-all hover:bg-indigo-900/40 ${
              view === item.label
                ? "bg-cyan-200 font-bold text-cyan-900 dark:bg-indigo-800 dark:text-indigo-100"
                : "text-gray-700 dark:text-gray-200"
            }`}
          >
            <span className="text-xl">{item.icon}</span> {item.label}
          </button>
        ))}
      </aside>
      {/* Main content */}
      <main className="flex flex-1 items-start justify-center bg-transparent px-4 py-12 text-gray-700 dark:text-gray-100">
        <div className="w-full max-w-2xl">
          {view === VIEWS.SUMMARY && user && <SummaryComponent user={user} />}
          {view === VIEWS.EDIT_PROFILE && user && (
            <EditProfileComponent user={user} />
          )}
          {view === VIEWS.CHANGE_PASSWORD && <ChangePasswordComponent />}
          {view === VIEWS.STATS && user && <StatsComponent user={user} />}
          {view === VIEWS.DELETE_ACCOUNT && <DeleteAccountComponent />}
        </div>
      </main>
    </div>
  );
};
