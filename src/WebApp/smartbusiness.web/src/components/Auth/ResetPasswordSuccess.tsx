import { ShieldCheckIcon } from "lucide-react";
import { useNavigate } from "react-router-dom";

export const ResetPasswordSuccess: React.FC = () => {
  const navigate = useNavigate();
  return (
    <>
      <div className="mb-8 flex-1 items-center text-center">
        <div className="mb-4 flex justify-center">
          <ShieldCheckIcon className="mx-auto mb-4 h-12 w-12 text-center" />
        </div>
        <h2 className="mb-2 text-3xl font-bold text-white">
          Hasło zostało zmienione!
        </h2>
        <p className="text-gray-300">
          Możesz się teraz zalogować przy użyciu nowego hasła.
        </p>
      </div>

      <div className="flex justify-center">
        <button
          onClick={() => navigate("/login")}
          className={`} inline-flex items-center rounded-lg bg-cyan-500 px-4 py-2 text-sm font-semibold text-white shadow transition-colors duration-200 hover:bg-cyan-600`}
        >
          Wróć do logowania
        </button>
      </div>
    </>
  );
};
