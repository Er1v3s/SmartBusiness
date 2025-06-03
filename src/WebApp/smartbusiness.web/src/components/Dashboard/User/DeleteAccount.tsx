import { useState } from "react";
import type { ApiResponseError } from "../../../models/authErrors";
import { Eye, EyeOff, Lock } from "lucide-react";
import { useAlert } from "../../../context/alert/useAlert";
import { useAccount } from "../../../context/account/AccountContext";
import { useNavigate } from "react-router-dom";

export const DeleteAccountComponent: React.FC = () => {
  const [loading, setLoading] = useState(false);
  const [password, setPassword] = useState("");
  const [passwordError, setPasswordError] = useState<string | null>(null);
  const [showPassword, setShowPassword] = useState(false);

  const { showAlert } = useAlert();
  const { deleteAccount } = useAccount();
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setPasswordError(null);
    if (!password) {
      showAlert({
        title: "Błąd",
        message: "Podaj hasło, aby usunąć konto.",
        type: "error",
      });
      return;
    }
    setLoading(true);
    try {
      await deleteAccount(password);
      showAlert({
        title: "Konto usunięte!",
        message: "Twoje konto zostało trwale usunięte.",
        type: "success",
        duration: 5000,
      });
      setPassword("");
      navigate("/");
    } catch (err) {
      const error = err as ApiResponseError;
      showAlert({
        title: error.title || "Błąd podczas usuwania konta",
        message: error.detail || "Spróbuj ponownie później.",
        type: "error",
      });
      setPassword("");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="space-y-4">
      <h2 className="text-2xl font-bold text-indigo-700 dark:text-indigo-200">
        Usuń konto
      </h2>
      <form
        onSubmit={handleSubmit}
        className="space-y-6 rounded-xl border border-gray-200 bg-gray-50 p-6 shadow-xl dark:border-gray-800 dark:bg-gray-900"
      >
        <div>
          <label>
            <span className="text-sm font-medium text-gray-800 dark:text-gray-100">
              Hasło
            </span>
            <div className="relative">
              <Lock className="absolute top-1/2 left-3 h-5 w-5 -translate-y-1/2 transform text-gray-400" />
              <input
                type={showPassword ? "text" : "password"}
                name="newPassword"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                className="w-full rounded-lg border border-gray-400 bg-white/5 p-3 pr-10 pl-10 text-gray-800 placeholder-gray-500 shadow-md focus:border-transparent focus:ring-2 focus:ring-indigo-500 focus:outline-none dark:border-white/20 dark:text-gray-100"
                placeholder="••••••••"
              />
              <button
                type="button"
                onClick={() => setShowPassword(!showPassword)}
                className="absolute top-1/2 right-3 -translate-y-1/2 transform text-gray-400 hover:text-gray-800 dark:hover:text-white"
              >
                {showPassword ? (
                  <EyeOff className="h-5 w-5" />
                ) : (
                  <Eye className="h-5 w-5" />
                )}
              </button>
            </div>
          </label>
          {passwordError && (
            <p className="mt-1 text-xs text-red-600 dark:text-red-400">
              {passwordError}
            </p>
          )}
        </div>

        <button
          type="submit"
          className="rounded bg-gradient-to-r from-red-700 to-red-600 px-4 py-2 font-semibold text-white shadow transition hover:from-red-800 hover:to-red-700 focus:ring-2 focus:ring-red-400 focus:outline-none"
          disabled={loading}
        >
          {loading ? "Zapisywanie..." : "Usuń konto"}
        </button>
      </form>
    </div>
  );
};
