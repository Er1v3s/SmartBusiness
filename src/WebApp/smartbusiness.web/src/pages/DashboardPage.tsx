import React, { useEffect, useState } from "react";
import { Shield, User, Zap } from "lucide-react";
import { useAuth } from "../context/AuthContext";
import { NavLink } from "react-router-dom";
import { Alert } from "../components/General/Alert";
import type { AlertProps } from "../components/General/alertProps";

// Dashboard Page Component
export const DashboardPage: React.FC = () => {
  const { user } = useAuth();
  const [showAlert, setShowAlert] = useState<boolean>(false);
  const [alertData, setAlertData] = useState<AlertProps>();

  useEffect(() => {
    const loginAlert = sessionStorage.getItem("showLoginAlert");
    const registerAlert = sessionStorage.getItem("showRegisterAlert");

    if (loginAlert === "true") {
      setAlertData({
        title: "Zalogowano!",
        message: "Twoje konto zostało pomyślnie uwierzytelnione.",
        type: "success",
        duration: 5000,
      });
      setShowAlert(true);
      sessionStorage.removeItem("showLoginAlert");
    } else if (registerAlert === "true") {
      setAlertData({
        title: "Zarejestrowano!",
        message: "Twoje konto zostało pomyślnie utworzone.",
        type: "info",
        duration: 5000,
      });
      setShowAlert(true);
      sessionStorage.removeItem("showRegisterAlert");
    }
  }, []);

  return (
    <div className="min-h-screen bg-gray-50">
      {showAlert && alertData != null && (
        <Alert
          title={alertData.title}
          message={alertData.message}
          type={alertData.type}
          duration={alertData.duration}
        />
      )}
      
      {/* Main Content */}
      <div className="mx-auto max-w-7xl px-4 py-8 sm:px-6 lg:px-8">
        <div className="mb-8">
          <h1 className="mb-2 text-3xl font-bold text-gray-900">Dashboard</h1>
          <p className="text-gray-600">Zarządzaj swoim kontem i aplikacją</p>
        </div>

        {/* Stats Cards */}
        <div className="mb-8 grid grid-cols-1 gap-6 md:grid-cols-3">
          <div className="rounded-lg bg-white p-6 shadow">
            <div className="flex items-center">
              <div className="rounded-lg bg-indigo-100 p-2">
                <User className="h-6 w-6 text-indigo-600" />
              </div>
              <div className="ml-4">
                <p className="text-sm font-medium text-gray-600">Profil</p>
                <p className="text-2xl font-semibold text-gray-900">Aktywny</p>
              </div>
            </div>
          </div>

          <div className="rounded-lg bg-white p-6 shadow">
            <div className="flex items-center">
              <div className="rounded-lg bg-green-100 p-2">
                <Shield className="h-6 w-6 text-green-600" />
              </div>
              <div className="ml-4">
                <p className="text-sm font-medium text-gray-600">
                  Bezpieczeństwo
                </p>
                <p className="text-2xl font-semibold text-gray-900">Wysokie</p>
              </div>
            </div>
          </div>

          <div className="rounded-lg bg-white p-6 shadow">
            <div className="flex items-center">
              <div className="rounded-lg bg-purple-100 p-2">
                <Zap className="h-6 w-6 text-purple-600" />
              </div>
              <div className="ml-4">
                <p className="text-sm font-medium text-gray-600">Status</p>
                <p className="text-2xl font-semibold text-gray-900">Online</p>
              </div>
            </div>
          </div>
        </div>

        {/* User Info Card */}
        <div className="rounded-lg bg-white p-6 shadow">
          <h2 className="mb-4 text-xl font-semibold text-gray-900">
            Informacje o koncie
          </h2>
          <div className="space-y-3">
            <div className="flex justify-between">
              <span className="text-gray-600">Nazwa użytkownika:</span>
              <span className="font-medium text-gray-900">
                {user?.username}
              </span>
            </div>
            <div className="flex justify-between">
              <span className="text-gray-600">Email:</span>
              <span className="font-medium text-gray-900">{user?.email}</span>
            </div>
            <div className="flex justify-between">
              <span className="text-gray-600">ID użytkownika:</span>
              <span className="font-medium text-gray-900">{user?.id}</span>
            </div>
          </div>
        </div>

        {/* Welcome Message */}
        <div className="mt-8 rounded-lg bg-gradient-to-r from-indigo-500 to-purple-600 p-6 text-white shadow">
          <h2 className="mb-2 text-2xl font-bold">Witaj w SecureApp!</h2>
          <NavLink to="/login" className="text-indigo-100">
            Twoje konto zostało pomyślnie uwierzytelnione przy użyciu JWT Token.
            Możesz teraz korzystać ze wszystkich funkcji aplikacji.
          </NavLink>
        </div>
      </div>
    </div>
  );
};
