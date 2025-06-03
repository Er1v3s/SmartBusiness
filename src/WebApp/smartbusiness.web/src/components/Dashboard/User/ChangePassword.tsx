import React, { useState } from "react";
import { useAlert } from "../../../context/alert/useAlert";
import type { ApiResponseError } from "../../../models/authErrors";
import { Lock, Eye, EyeOff } from "lucide-react";
import { useAccount } from "../../../context/account/AccountContext";

export const ChangePasswordComponent: React.FC = () => {
  const [currentPassword, setcurrentPassword] = useState("");
  const [newPassword, setNewPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const [currentPasswordError, setCurrentPasswordError] = useState<
    string | null
  >(null);
  const [newPasswordError, setNewPasswordError] = useState<string | null>(null);
  const [showCurrentPassword, setShowCurrentPassword] = useState(false);
  const [showNewPassword, setShowNewPassword] = useState(false);
  const { showAlert } = useAlert();
  const { changePassword } = useAccount();

  const validatePassword = (password: string) => {
    if (password.length < 8) return "Hasło musi mieć co najmniej 8 znaków.";
    if (!/[A-Z]/.test(password))
      return "Hasło musi zawierać co najmniej jedną wielką literę.";
    if (!/[a-z]/.test(password))
      return "Hasło musi zawierać co najmniej jedną małą literę.";
    if (!/[0-9]/.test(password))
      return "Hasło musi zawierać co najmniej jedną cyfrę.";
    if (!/[!@#$%^&*(),.?":{}|<>]/.test(password))
      return "Hasło musi zawierać co najmniej jeden znak specjalny.";
    if (password.length > 100)
      return "Hasło nie może być dłuższe niż 100 znaków.";
    return "";
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    let hasError = false;
    setCurrentPasswordError(null);
    setNewPasswordError(null);
    if (!currentPassword) {
      setCurrentPasswordError("Podaj stare hasło.");
      hasError = true;
    }
    const newPassErr = validatePassword(newPassword);
    if (newPassErr) {
      setNewPasswordError(newPassErr);
      hasError = true;
    }
    if (hasError) return;
    setLoading(true);
    try {
      await changePassword(currentPassword, newPassword);
      showAlert({
        title: "Hasło zmienione!",
        message: "Twoje hasło zostało zaktualizowane.",
        type: "success",
      });
      setcurrentPassword("");
      setNewPassword("");
    } catch (err) {
      const error = err as ApiResponseError;
      console.log(error);
      showAlert({
        title: error.title,
        message: error.detail,
        type: "error",
      });
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="space-y-4">
      <h2 className="text-2xl font-bold text-indigo-700 dark:text-indigo-200">
        Zmień hasło
      </h2>
      <form
        onSubmit={handleSubmit}
        className="space-y-6 rounded-xl border border-gray-200 bg-gray-50 p-6 shadow-xl dark:border-gray-800 dark:bg-gray-900"
      >
        <div>
          <label>
            <span className="text-sm font-medium text-gray-800 dark:text-gray-100">
              Stare hasło
            </span>
            <div className="relative">
              <Lock className="absolute top-1/2 left-3 h-5 w-5 -translate-y-1/2 transform text-gray-400" />
              <input
                type={showCurrentPassword ? "text" : "password"}
                name="newPassword"
                value={currentPassword}
                onChange={(e) => setcurrentPassword(e.target.value)}
                className="w-full rounded-lg border border-gray-400 bg-white/5 p-3 pr-10 pl-10 text-gray-800 placeholder-gray-500 shadow-md focus:border-transparent focus:ring-2 focus:ring-indigo-500 focus:outline-none dark:border-white/20 dark:text-gray-100"
                placeholder="••••••••"
                required
              />
              <button
                type="button"
                onClick={() => setShowCurrentPassword(!showCurrentPassword)}
                className="absolute top-1/2 right-3 -translate-y-1/2 transform text-gray-400 hover:text-gray-800 dark:hover:text-white"
              >
                {showCurrentPassword ? (
                  <EyeOff className="h-5 w-5" />
                ) : (
                  <Eye className="h-5 w-5" />
                )}
              </button>
            </div>
          </label>
          {currentPasswordError && (
            <p className="mt-1 text-xs text-red-600 dark:text-red-400">
              {currentPasswordError}
            </p>
          )}
        </div>

        <div>
          <label>
            <span className="text-sm font-medium text-gray-800 dark:text-gray-100">
              Nowe hasło
            </span>
            <div className="relative">
              <Lock className="absolute top-1/2 left-3 h-5 w-5 -translate-y-1/2 transform text-gray-400" />
              <input
                type={showNewPassword ? "text" : "password"}
                name="newPassword"
                value={newPassword}
                onChange={(e) => setNewPassword(e.target.value)}
                className="w-full rounded-lg border border-gray-400 bg-white/5 p-3 pr-10 pl-10 text-gray-800 placeholder-gray-500 shadow-md focus:border-transparent focus:ring-2 focus:ring-indigo-500 focus:outline-none dark:border-white/20 dark:text-gray-100"
                placeholder="••••••••"
                required
              />
              <button
                type="button"
                onClick={() => setShowNewPassword(!showNewPassword)}
                className="absolute top-1/2 right-3 -translate-y-1/2 transform text-gray-400 hover:text-gray-800 dark:hover:text-white"
              >
                {showNewPassword ? (
                  <EyeOff className="h-5 w-5" />
                ) : (
                  <Eye className="h-5 w-5" />
                )}
              </button>
            </div>
          </label>
          {newPasswordError && (
            <p className="mt-1 text-xs text-red-600 dark:text-red-400">
              {newPasswordError}
            </p>
          )}
        </div>

        <button
          type="submit"
          className="rounded bg-gradient-to-r from-indigo-600 to-indigo-500 px-4 py-2 font-semibold text-white shadow transition hover:from-indigo-700 hover:to-indigo-600 focus:ring-2 focus:ring-indigo-400 focus:outline-none"
          disabled={loading}
        >
          {loading ? "Zapisywanie..." : "Zmień hasło"}
        </button>
      </form>
    </div>
  );
};
