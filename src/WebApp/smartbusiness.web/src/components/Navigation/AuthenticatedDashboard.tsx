import { useAuth } from "../../context/auth/AuthContext";
import logo_no_text from "../../assets/logo_no_text.svg";
import logo_no_text_white from "../../assets/logo_no_text_white.svg";
import logo_text from "../../assets/logo_text.svg";
import logo_text_white from "../../assets/logo_text_white.svg";
import { NavLink, useNavigate } from "react-router-dom";
import { User } from "lucide-react";
import { ThemeToggleButton } from "../General/ThemeToggleButton";
import { CompanySelectionComponent } from "../Dashboard/Company/CompanySelectionComponent";
import { useAlert } from "../../context/alert/useAlert";

export const AuthenticatedDashboard: React.FC = () => {
  const { user, logout } = useAuth();
  const navigate = useNavigate();
  const { showAlert } = useAlert();

  const handleLogout = () => {
    logout();
    showAlert({
      title: "Wylogowano!",
      message: "Użytkownik został wylogowany. Do zobaczenia następnym razem!",
      type: "success",
      duration: 5000,
    });
    navigate("/home");
  };

  return (
    <nav className="sticky top-0 z-50 h-16 bg-white/80 shadow-md backdrop-blur dark:bg-gray-900">
      <div className="shadow-sm dark:border-b">
        <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">
          <div className="flex h-16 items-center justify-between">
            <NavLink to="/dashboard">
              <div className="flex items-center space-x-2">
                <img
                  src={logo_no_text}
                  alt="Logo"
                  className="block h-8 w-auto scale-125 dark:hidden"
                />
                <img
                  src={logo_no_text_white}
                  alt="Logo"
                  className="hidden h-8 w-auto scale-125 dark:block"
                />
                <img
                  src={logo_text}
                  alt="Logo"
                  className="block h-8 w-auto scale-125 pl-5 dark:hidden"
                />
                <img
                  src={logo_text_white}
                  alt="Logo"
                  className="hidden h-8 w-auto scale-125 pl-5 dark:block"
                />
              </div>
            </NavLink>

            <div className="flex items-center space-x-4">
              <CompanySelectionComponent />

              <ThemeToggleButton />
              <NavLink to="/dashboard/user">
                <User className="hover:purple-600 h-8 w-8 cursor-pointer text-indigo-500 hover:scale-110" />
              </NavLink>
              <span className="text-gray-700 dark:text-gray-200">
                Witaj, {user?.username}!
              </span>
              <button
                onClick={handleLogout}
                className="cursor-pointer px-4 py-2 text-red-700 transition-colors hover:scale-110 hover:text-red-700"
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
