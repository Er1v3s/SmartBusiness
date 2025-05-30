import { NavLink, useNavigate } from "react-router-dom";
import logo_no_text_white from "../../assets/logo_no_text_white.svg";
import logo_text_white from "../../assets/logo_text_white.svg";

export const NotAuthenticatedDashboard: React.FC = () => {
  const navigate = useNavigate();

  return (
    <nav className="relative z-10 flex items-center justify-between p-6 lg:px-8">
      <NavLink to="/home">
        <div className="flex items-center space-x-2">
          <img
            src={logo_no_text_white}
            alt="Logo"
            className="h-8 w-auto scale-150 text-white"
          />
          <img
            src={logo_text_white}
            alt="Logo"
            className="h-8 w-auto scale-150 pl-5 text-white"
          />
        </div>
      </NavLink>

      <div className="flex space-x-4">
        <button
          onClick={() => navigate("/login")}
          className="cursor-pointer rounded-lg border border-white/20 px-4 py-2 text-white transition duration-200 hover:bg-white/10"
        >
          Zaloguj się
        </button>
        <button
          onClick={() => navigate("/register")}
          className="cursor-pointer rounded-lg bg-white px-4 py-2 font-medium text-indigo-900 transition-colors duration-200 hover:bg-gray-100"
        >
          Zarejestruj się
        </button>
      </div>
    </nav>
  );
};
