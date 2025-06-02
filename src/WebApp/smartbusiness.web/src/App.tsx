// App.tsx
import React, { type JSX } from "react";
import {
  BrowserRouter as Router,
  Routes,
  Route,
  Navigate,
} from "react-router-dom";
import { Navigation } from "./components/Navigation/Navigation";
import { useAuth } from "./context/auth/AuthContext";
import { HomePage } from "./pages/HomePage";
import { LoginPage } from "./pages/Auth/LoginPage";
import { RegisterPage } from "./pages/Auth/RegisterPage";
import { DashboardPage } from "./pages/DashboardPage";
import { AuthProvider } from "./context/auth/AuthProvider";
import { AlertProvider } from "./context/alert/AlertContext";
import { ForgotPassword } from "./pages/Auth/ForgotPassword";
import { ResetPassword } from "./pages/Auth/ResetPassword";
import "./App.css";

// Private Route component checks if the user is authenticated
const PrivateRoute: React.FC<{ children: JSX.Element }> = ({ children }) => {
  const { isAuthenticated } = useAuth();
  return isAuthenticated ? children : <Navigate to="/login" replace />;
};

// PublicRoute component checks if the user is not authenticated
const PublicRoute: React.FC<{ children: JSX.Element }> = ({ children }) => {
  const { isAuthenticated } = useAuth();
  return !isAuthenticated ? children : <Navigate to="/dashboard" replace />;
};

// RedirectRoute component redirects based on authentication status
const RedirectRoute: React.FC = () => {
  const { isAuthenticated } = useAuth();
  return <Navigate to={isAuthenticated ? "/dashboard" : "/home"} replace />;
};

export const App: React.FC = () => {
  return (
    <Router>
      <Navigation />
      <Routes>
        {/* Redairections route */}
        <Route path="/" element={<RedirectRoute />} />

        {/* Private paths*/}
        <Route
          path="/dashboard"
          element={
            <PrivateRoute>
              <DashboardPage />
            </PrivateRoute>
          }
        />

        {/* Public paths */}
        <Route
          path="/home"
          element={
            <PublicRoute>
              <HomePage />
            </PublicRoute>
          }
        />
        <Route
          path="/login"
          element={
            <PublicRoute>
              <LoginPage />
            </PublicRoute>
          }
        />
        <Route
          path="/register"
          element={
            <PublicRoute>
              <RegisterPage />
            </PublicRoute>
          }
        />
        <Route
          path="/forgot-password"
          element={
            <PublicRoute>
              <ForgotPassword />
            </PublicRoute>
          }
        />
        <Route
          path="/reset-password"
          element={
            <PublicRoute>
              <ResetPassword />
            </PublicRoute>
          }
        />
        <Route
          path="/reset-password"
          element={
            <PublicRoute>
              <ResetPassword />
            </PublicRoute>
          }
        />

        {/* UNKNOWN PATH REDIRECT TO '/" */}
        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </Router>
  );
};

const AppWithProvider: React.FC = () => (
  <AuthProvider>
    <AlertProvider>
      <App />
    </AlertProvider>
  </AuthProvider>
);

export default AppWithProvider;
