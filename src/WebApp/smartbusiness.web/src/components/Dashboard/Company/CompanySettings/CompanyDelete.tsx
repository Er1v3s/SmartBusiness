import React, { useState } from "react";
import { useCompany } from "../../../../context/company/CompanyContext";
import { useAlert } from "../../../../context/alert/useAlert";
import { Eye, EyeOff, Lock } from "lucide-react";
import { ButtonError } from "../../../General/Buttons";
import { useAuth } from "../../../../context/auth/AuthContext";
import type { ApiResponseError } from "../../../../models/authErrors";

export const CompanyDelete: React.FC = () => {
  const { deleteCompany, fetchCompanies, company } = useCompany();
  const { loginUsingRefreshToken } = useAuth();
  const { showAlert } = useAlert();
  const [password, setPassword] = useState("");
  const [showPassword, setShowPassword] = useState(false);
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!company) return;
    if (!password) {
      showAlert({
        title: "Błąd",
        message: "Podaj hasło, aby usunąć firmę.",
        type: "error",
        duration: 3000,
      });
      return;
    }
    setLoading(true);
    try {
      await deleteCompany(password);

      showAlert({
        title: "Usunięto!",
        message: "Twoja firma została trwale usunięta.",
        type: "success",
        duration: 5000,
      });
      await loginUsingRefreshToken();
      await fetchCompanies();
      setPassword("");
    } catch (err) {
      const error = err as ApiResponseError;

      showAlert({
        title: error.title || "Błąd podczas usuwania firmy",
        message: error.detail || "Spróbuj ponownie później.",
        type: "error",
        duration: 3000,
      });
      setPassword("");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="space-y-4">
      <h2 className="text-2xl font-bold text-gray-800 dark:text-gray-100">
        Usuń firmę
      </h2>
      <div className="rounded-lg border-2 border-gray-100 bg-white p-6 shadow-xl dark:border-gray-700 dark:bg-gray-800">
        {company ? (
          <form onSubmit={handleSubmit} className="space-y-4">
            <div>
              <label className="block text-gray-700 dark:text-gray-200">
                Hasło do konta
              </label>
              <div className="relative">
                <Lock className="absolute top-1/2 left-3 h-5 w-5 -translate-y-1/2 transform text-gray-400" />
                <input
                  type={showPassword ? "text" : "password"}
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
            </div>
            <ButtonError
              text={loading ? "Usuwanie..." : "Usuń firmę"}
              type="submit"
              disabled={loading}
            />
          </form>
        ) : (
          <div className="text-gray-500 dark:text-gray-400">
            Brak wybranej firmy.
          </div>
        )}
      </div>
    </div>
  );
};
