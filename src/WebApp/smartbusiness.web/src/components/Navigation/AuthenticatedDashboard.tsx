import { useAuth } from "../../context/AuthContext";
import type { Page } from "../../models";
import logo_no_text from "../../assets/logo_no_text.svg";
import logo_text from "../../assets/logo_text.svg";

export const AuthenticatedDashboard: React.FC<{
  onNavigate: (page: Page) => void;
}> = ({ onNavigate }) => {
  const { user, logout } = useAuth();

  const handleLogout = () => {
    logout();
    onNavigate("home");
  };

  return (
    <nav className="border-b bg-white shadow-sm">
      <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">
        <div className="flex h-16 items-center justify-between">
          <div className="flex items-center space-x-2">
            <img
              src={logo_no_text}
              alt="Logo"
              className="h-8 w-auto scale-150"
            />
            <img
              src={logo_text}
              alt="Logo"
              className="h-8 w-auto scale-150 pl-5"
            />
          </div>

          <div className="flex items-center space-x-4">
            <span className="text-gray-700">Witaj, {user?.username}!</span>
            <button
              onClick={handleLogout}
              className="cursor-pointer px-4 py-2 text-red-600 transition-colors hover:text-red-700"
            >
              Wyloguj się
            </button>
          </div>
        </div>
      </div>
    </nav>
  );
};
