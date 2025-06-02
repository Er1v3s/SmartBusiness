import { useAuth } from "../../context/auth/AuthContext";
import logo_no_text from "../../assets/logo_no_text.svg";
import logo_text from "../../assets/logo_text.svg";
import { NavLink, useNavigate } from "react-router-dom";
import { User } from "lucide-react";
import { ThemeToggleButton } from "../General/ThemeToggleButton";

export const AuthenticatedDashboard: React.FC = () => {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate("/home");
  };

  return (
    <nav className="sticky top-0 z-50 h-16 bg-white/80 shadow-md backdrop-blur dark:bg-gray-900/80">
      <div className="border-b bg-white shadow-sm">
        <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">
          <div className="flex h-16 items-center justify-between">
            <NavLink to="/dashboard">
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
            </NavLink>

            <div className="flex items-center space-x-4">
              <ThemeToggleButton />
              <NavLink to="/dashboard/user">
                <User className="hover:purple-600 h-8 w-8 cursor-pointer text-indigo-500 hover:scale-110" />
              </NavLink>
              <span className="text-gray-700 dark:text-gray-200">
                Witaj, {user?.username}!
              </span>
              <button
                onClick={handleLogout}
                className="cursor-pointer px-4 py-2 text-red-600 transition-colors hover:text-red-700"
              >
                Wyloguj się
              </button>
            </div>
          </div>
        </div>
      </div>
    </nav>
  );
};
