import React from "react";
import { useState } from "react";
import { Mail, Lock, Eye, EyeOff, User, UserPlus } from "lucide-react";
import type { RegisterForm } from "../models";
import { useNavigate } from "react-router-dom";
import type { ApiResponseError } from "../models/authErrors";
import { useAuth } from "../context/AuthContext";

// Register Page Component
export const RegisterPage: React.FC = () => {
  const [form, setForm] = useState<RegisterForm>({
    username: "",
    email: "",
    password: "",
  });
  const [showPassword, setShowPassword] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState("");

  const navigate = useNavigate();
  const { register } = useAuth();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    setError("");

    try {
      await register(form.username, form.email, form.password);
      sessionStorage.setItem("showRegisterAlert", "true");
      navigate("/dashboard");
    } catch (err) {
      console.log(err);
      const error = err as ApiResponseError;
      switch (error.status.toString()) {
        case "400":
          if (error.errors) {
            error.errors.reverse().forEach((err) => {
              setError(`${err.property}: ${err.errorMessage}`);
            });
          }
          break;
        case "404":
          setError(
            "Użytkownik o podanym adresie e-mail nie został znaleziony.",
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
  };

  return (
    <div className="flex min-h-screen items-center justify-center bg-gradient-to-br from-indigo-900 via-purple-900 to-pink-800 p-4">
      <div className="w-full max-w-md">
        <div className="rounded-2xl border border-white/20 bg-white/10 p-8 shadow-2xl backdrop-blur-lg">
          <div className="mb-8 text-center">
            <UserPlus className="mx-auto mb-4 h-12 w-12 text-white" />
            <h2 className="mb-2 text-3xl font-bold text-white">Utwórz konto</h2>
            <p className="text-gray-300">Dołącz do naszej platformy</p>
          </div>

          {error && (
            <div className="mb-6 rounded-lg border border-red-500/50 bg-red-500/20 p-3">
              <p className="text-sm text-red-200">{error}</p>
            </div>
          )}

          <div className="space-y-6">
            <div>
              <label className="mb-2 block text-sm font-medium text-gray-200">
                Nazwa użytkownika
              </label>
              <div className="relative">
                <User className="absolute top-1/2 left-3 h-5 w-5 -translate-y-1/2 transform text-gray-400" />
                <input
                  type="text"
                  name="username"
                  value={form.username}
                  onChange={handleChange}
                  className="w-full rounded-lg border border-white/20 bg-white/5 py-3 pr-4 pl-10 text-white placeholder-gray-400 focus:border-transparent focus:ring-2 focus:ring-cyan-500 focus:outline-none"
                  placeholder="nazwa_uzytkownika"
                  required
                />
              </div>
            </div>

            <div>
              <label className="mb-2 block text-sm font-medium text-gray-200">
                Email
              </label>
              <div className="relative">
                <Mail className="absolute top-1/2 left-3 h-5 w-5 -translate-y-1/2 transform text-gray-400" />
                <input
                  type="email"
                  name="email"
                  value={form.email}
                  onChange={handleChange}
                  className="w-full rounded-lg border border-white/20 bg-white/5 py-3 pr-4 pl-10 text-white placeholder-gray-400 focus:border-transparent focus:ring-2 focus:ring-cyan-500 focus:outline-none"
                  placeholder="twoj@email.com"
                  required
                />
              </div>
            </div>

            <div>
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
            </div>

            <button
              onClick={handleSubmit}
              disabled={isLoading}
              className="w-full cursor-pointer rounded-lg bg-gradient-to-r from-cyan-500 to-purple-600 px-4 py-3 font-semibold text-white transition-all duration-200 hover:from-cyan-600 hover:to-purple-700 focus:ring-2 focus:ring-cyan-500 focus:ring-offset-2 focus:ring-offset-transparent focus:outline-none disabled:cursor-not-allowed disabled:opacity-50"
            >
              {isLoading ? "Rejestracja..." : "Zarejestruj się"}
            </button>
          </div>

          <div className="mt-8 text-center">
            <p className="text-gray-300">
              Masz już konto?{" "}
              <button
                onClick={() => navigate("/login")}
                className="cursor-pointer font-medium text-cyan-400 transition-colors hover:text-cyan-300"
              >
                Zaloguj się
              </button>
            </p>
          </div>
        </div>
      </div>
    </div>
  );
};
