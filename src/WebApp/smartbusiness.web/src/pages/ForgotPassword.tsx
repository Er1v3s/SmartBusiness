import React from "react";
import { useAuth } from "../context/AuthContext";
import { useState } from "react";
import { Mail } from "lucide-react";
import { useNavigate } from "react-router-dom";
import type { AlertProps } from "../components/General/alertProps";
import { Alert } from "../components/General/Alert";

// Login Page Component
export const ForgotPassword: React.FC = () => {
  const [form, setForm] = useState({
    email: "",
  });
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState("");

  const navigate = useNavigate();
  const { sendResetLink } = useAuth();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    setError("");

    try {
      await sendResetLink(form.email);
      showAlertMessage();
    } catch (err) {
      console.error(err);
      setError("Wystąpił błąd podczas wysyłania linku resetującego hasło.");
    } finally {
      setIsLoading(false);
    }
  };

  const [showAlert, setShowAlert] = useState<boolean>(false);
  const [alertData, setAlertData] = useState<AlertProps>();

  const showAlertMessage = () => {
    setShowAlert(true);
    setAlertData({
      title: "Link resetujący hasło został wysłany!",
      message: "Sprawdź swoją skrzynkę pocztową.",
      type: "success",
      duration: 3000,
    });
    setTimeout(() => {
      setShowAlert(false);
    }, 3500);
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setForm((prev) => ({
      ...prev,
      [name]: value,
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value, type, checked } = e.target;
    setForm((prev) => ({
      ...prev,
      [name]: type === "checkbox" ? checked : value,
    }));
  };

  return (
    <div className="flex min-h-screen items-center justify-center bg-gradient-to-br from-indigo-900 via-purple-900 to-pink-800 p-px">
      {showAlert && alertData != null && (
        <Alert
          title={alertData.title}
          message={alertData.message}
          type={alertData.type}
          duration={alertData.duration}
        />
      )}

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

          {error && (
            <div className="mb-6 rounded-lg border border-red-500/50 bg-red-500/20 p-3">
              <p className="text-sm text-red-200">{error}</p>
            </div>
          )}

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
                />
              </div>
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
                {isLoading ? "Przetwarzanie..." : "Wyślij link resetujący"}
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};
