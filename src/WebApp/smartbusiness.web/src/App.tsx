import React from "react";
import { useState, useEffect } from "react";
import { Navigation } from "./components/Navigation/Navigation";
import { useAuth } from "./context/AuthContext";
import type { Page } from "./models";
import { HomePage } from "./pages/HomePage";
import { LoginPage } from "./pages/LoginPage";
import { RegisterPage } from "./pages/RegisterPage";
import { DashboardPage } from "./pages/DashboardPage";
import { AuthProvider } from "./context/AuthProvider";
import "./App.css"; // Import your global styles

// Main App Component
export const App: React.FC = () => {
  const [currentPage, setCurrentPage] = useState<Page>("home");
  const { isAuthenticated } = useAuth();

  // Auto-redirect to dashboard if authenticated
  useEffect(() => {
    if (isAuthenticated && currentPage !== "dashboard") {
      setCurrentPage("dashboard");
    }
  }, [isAuthenticated, currentPage]);

  // Protect dashboard route
  const handleNavigate = (page: Page) => {
    if (page === "dashboard" && !isAuthenticated) {
      setCurrentPage("login");
      return;
    }
    setCurrentPage(page);
  };

  const renderCurrentPage = () => {
    switch (currentPage) {
      case "login":
        return <LoginPage onNavigate={handleNavigate} />;
      case "register":
        return <RegisterPage onNavigate={handleNavigate} />;
      case "dashboard":
        return <DashboardPage onNavigate={handleNavigate} />;
      default:
        return <HomePage onNavigate={handleNavigate} />;
    }
  };

  return (
    <div>
      {currentPage !== "home" &&
        currentPage !== "login" &&
        currentPage !== "register" && (
          <Navigation currentPage={currentPage} onNavigate={handleNavigate} />
        )}
      {currentPage === "home" && (
        <Navigation currentPage={currentPage} onNavigate={handleNavigate} />
      )}
      {renderCurrentPage()}
    </div>
  );
};

const AppWithProvider: React.FC = () => (
  <AuthProvider>
    <App />
  </AuthProvider>
);

export default AppWithProvider;
