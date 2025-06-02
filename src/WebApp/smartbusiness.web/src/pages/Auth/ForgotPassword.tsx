import React, { useState, useEffect, useRef } from "react";
import { Mail } from "lucide-react";
import { useNavigate } from "react-router-dom";
import { useAlert } from "../../context/alert/useAlert";
import { useAuth } from "../../context/auth/AuthContext";

// Login Page Component
export const ForgotPassword: React.FC = () => {
  const [form, setForm] = useState({
    email: "",
  });

  const navigate = useNavigate();
  const { sendResetLink } = useAuth();
  const { showAlert } = useAlert();

  const [isLoading, setIsLoading] = useState(false);
  // cooldown state
  const [cooldown, setCooldown] = useState(0);
  const cooldownRef = useRef<ReturnType<typeof setTimeout> | null>(null);

  useEffect(() => {
    if (cooldown > 0) {
      cooldownRef.current = setTimeout(() => setCooldown(cooldown - 1), 1000);
    }
    return () => {
      if (cooldownRef.current) clearTimeout(cooldownRef.current);
    };
  }, [cooldown]);

  const handleSubmit = async (e?: React.FormEvent) => {
    if (e) e.preventDefault();
    if (cooldown > 0) return;
    setIsLoading(true);
    try {
      await sendResetLink(form.email);
      showAlert({
        title: "Link resetujący hasło został wysłany!",
        message: "Sprawdź swoją skrzynkę pocztową.",
        type: "success",
      });

      // Block further submissions for 30 seconds
      setCooldown(30);
    } catch {
      showAlert({
        title: "Błąd",
        message: "Wystąpił błąd podczas wysyłania linku resetującego hasło.",
        type: "error",
      });
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
    <div className="flex min-h-screen items-center justify-center bg-gradient-to-br from-indigo-900 via-purple-900 to-pink-800 p-px">
      <div className="w-full max-w-md">
        <div className="rounded-2xl border border-white/20 bg-white/10 p-8 shadow-2xl backdrop-blur-lg">
          <div className="mb-8 flex-1 items-center text-center">
            <div className="mb-4 flex justify-center">
              <Mail className="mx-auto mb-4 h-12 w-12 text-center" />
            </div>
            <h2 className="mb-2 text-3xl font-bold text-white">
              Zapomniałeś hasła?
            </h2>
            <p className="text-gray-300">
              Nie martw się! Na twój adres email wyślimy Ci link do zmiany hasła
            </p>
          </div>

          <div className="space-y-6">
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
                  disabled={isLoading || cooldown > 0}
                />
              </div>
            </div>

            {cooldown > 0 && (
              <div className="text-center text-sm font-semibold text-cyan-300">
                Możesz wysłać ponownie za {cooldown} sekund
              </div>
            )}

            <div className="flex items-center justify-between">
              <button
                onClick={() => navigate("/login")}
                className="text-sm text-gray-300 hover:text-white"
                type="button"
              >
                Wróć do logowania
              </button>
              {cooldown > 0 ? (
                <button
                  disabled
                  className="inline-flex cursor-not-allowed items-center rounded-lg bg-gray-400 px-4 py-2 text-sm font-semibold text-white opacity-50 shadow"
                >
                  Wyślij ponownie
                </button>
              ) : (
                <button
                  onClick={handleSubmit}
                  disabled={isLoading}
                  className={`inline-flex items-center rounded-lg bg-cyan-500 px-4 py-2 text-sm font-semibold text-white shadow transition-colors duration-200 hover:bg-cyan-600 ${
                    isLoading ? "cursor-not-allowed opacity-50" : ""
                  }`}
                >
                  {isLoading ? "Przetwarzanie..." : "Wyślij link resetujący"}
                </button>
              )}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};
