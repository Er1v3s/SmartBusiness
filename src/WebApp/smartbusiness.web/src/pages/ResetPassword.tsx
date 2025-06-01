import { useNavigate, useSearchParams } from "react-router-dom";
import { Lock, Eye, EyeOff, ShieldAlert, ShieldCheckIcon } from "lucide-react";
import { useEffect, useState } from "react";
import { useAlert } from "../context/alert/useAlert";
import type { ApiResponseError } from "../models/authErrors";
import { useAuth } from "../context/AuthContext";

export const ResetPassword = () => {
  const [form, setForm] = useState({
    password: "",
  });
  const [searchParams] = useSearchParams();
  const [showPassword, setShowPassword] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState("");
  const [hasTyped, setHasTyped] = useState(false);
  const [success, setSuccess] = useState(false);

  const navigate = useNavigate();
  const { resetPassword } = useAuth();
  const { showAlert } = useAlert();

  useEffect(() => {
    if (!hasTyped) return;

    if (form.password.length < 8) {
      setError("Hasło musi mieć co najmniej 8 znaków.");
      setIsLoading(false);
      return;
    }

    if (!/[A-Z]/.test(form.password)) {
      setError("Hasło musi zawierać co najmniej jedną wielką literę.");
      setIsLoading(false);
      return;
    }

    if (!/[a-z]/.test(form.password)) {
      setError("Hasło musi zawierać co najmniej jedną małą literę.");
      setIsLoading(false);
      return;
    }

    if (!/[0-9]/.test(form.password)) {
      setError("Hasło musi zawierać co najmniej jedną cyfrę.");
      setIsLoading(false);
      return;
    }

    if (!/[!@#$%^&*(),.?":{}|<>]/.test(form.password)) {
      setError("Hasło musi zawierać co najmniej jeden znak specjalny.");
      setIsLoading(false);
      return;
    }

    if (form.password.length > 100) {
      setError("Hasło nie może być dłuższe niż 100 znaków.");
      setIsLoading(false);
      return;
    }

    if (form.password.trim() === "") {
      setError("Hasło nie może być puste.");
      setIsLoading(false);
      return;
    }

    setError("");
  }, [form.password, hasTyped]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    setError("");

    const token = searchParams.get("token") || "";

    if (!token) {
      setError("Brak tokenu resetującego hasło.");
      setIsLoading(false);
      return;
    }

    try {
      await resetPassword(token, form.password);
      showAlert({
        title: "Udało Ci się zmienić hasło!",
        message: "Możesz się ponownie zalogować.",
        type: "success",
        duration: 3000,
      });
      setSuccess(true);
    } catch (err) {
      const error = err as ApiResponseError;
      console.error(error);
      switch (error.status.toString()) {
        case "400":
          setError(
            "Próba zmiany hasła nie przebiegła pomyślnie. Spróbuj ponownie",
          );
          break;
        case "500":
          setError("Wystąpił błąd serwera. Spróbuj ponownie później.");
          break;
        default:
          setError("Wystąpił nieznany błąd. Spróbuj ponownie.");
          break;
      }
    } finally {
      setIsLoading(false);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setForm((prev) => ({
      ...prev,
      [name]: value,
    }));

    if (!hasTyped && value.trim() !== "") {
      setHasTyped(true);
    } else if (hasTyped && value.trim() === "") {
      setHasTyped(false);
      setError("");
    }
  };

  return (
    <div className="flex min-h-screen items-center justify-center bg-gradient-to-br from-indigo-900 via-purple-900 to-pink-800 p-px">
      <div className="w-full max-w-md">
        <div className="rounded-2xl border border-white/20 bg-white/10 p-8 shadow-2xl backdrop-blur-lg">
          {!success ? (
            <>
              <div className="mb-8 flex-1 items-center text-center">
                <div className="mb-4 flex justify-center">
                  <ShieldAlert className="mx-auto mb-4 h-12 w-12 text-center" />
                </div>
                <h2 className="mb-2 text-3xl font-bold text-white">
                  Resetowania hasła
                </h2>
                <p className="text-gray-300">
                  Wprowadź nowe hasło, tym razem go nie zapomnij 😉
                </p>
              </div>

              {error && (
                <div className="mb-6 rounded-lg border border-red-500/50 bg-red-500/20 p-3">
                  <p className="text-sm text-red-200">{error}</p>
                </div>
              )}

              <div className="space-y-6">
                <label className="mb-2 block text-sm font-medium text-gray-200">
                  Hasło
                </label>
                <div className="relative">
                  <Lock className="absolute top-1/2 left-3 h-5 w-5 -translate-y-1/2 transform text-gray-400" />
                  <input
                    type={showPassword ? "text" : "password"}
                    name="password"
                    value={form.password}
                    onChange={handleChange}
                    className="w-full rounded-lg border border-white/20 bg-white/5 py-3 pr-12 pl-10 text-white placeholder-gray-400 focus:border-transparent focus:ring-2 focus:ring-cyan-500 focus:outline-none"
                    placeholder="••••••••"
                    required
                  />
                  <button
                    type="button"
                    onClick={() => setShowPassword(!showPassword)}
                    className="absolute top-1/2 right-3 -translate-y-1/2 transform text-gray-400 hover:text-white"
                  >
                    {showPassword ? (
                      <EyeOff className="h-5 w-5" />
                    ) : (
                      <Eye className="h-5 w-5" />
                    )}
                  </button>
                </div>

                <div className="flex items-center justify-between">
                  <button
                    onClick={() => navigate("/login")}
                    className="text-sm text-gray-300 hover:text-white"
                  >
                    Wróć do logowania
                  </button>
                  <button
                    onClick={handleSubmit}
                    disabled={isLoading}
                    className={`inline-flex items-center rounded-lg bg-cyan-500 px-4 py-2 text-sm font-semibold text-white shadow transition-colors duration-200 hover:bg-cyan-600 ${
                      isLoading ? "cursor-not-allowed opacity-50" : ""
                    }`}
                  >
                    {isLoading ? "Przetwarzanie..." : "Zmień hasło"}
                  </button>
                </div>
              </div>
            </>
          ) : (
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
          )}
        </div>
      </div>
    </div>
  );
};
