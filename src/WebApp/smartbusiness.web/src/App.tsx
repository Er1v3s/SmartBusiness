// App.tsx
import React, { type JSX } from "react";
import {
  BrowserRouter as Router,
  Routes,
  Route,
  Navigate,
} from "react-router-dom";
import { Navigation } from "./components/Navigation/Navigation";
import { useAuth } from "./context/AuthContext";
import { HomePage } from "./pages/HomePage";
import { LoginPage } from "./pages/LoginPage";
import { RegisterPage } from "./pages/RegisterPage";
import { DashboardPage } from "./pages/DashboardPage";
import { AuthProvider } from "./context/AuthProvider";
import "./App.css";
import { ForgotPassword } from "./pages/ForgotPassword";

// Komponent zabezpieczający routing: jeśli użytkownik nie jest zalogowany przekierowuje do /login.
const PrivateRoute: React.FC<{ children: JSX.Element }> = ({ children }) => {
  const { isAuthenticated } = useAuth();
  return isAuthenticated ? children : <Navigate to="/login" replace />;
};

export const App: React.FC = () => {
  return (
    <Router>
      {/* Nawigacja – warto zmodyfikować komponent Navigation, by korzystał z Link lub NavLink z react-router-dom */}
      <Navigation />
      <Routes>
        <Route path="/" element={<HomePage />} />
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route path="/forgot-password" element={<ForgotPassword />} />
        <Route
          path="/dashboard"
          element={
            <PrivateRoute>
              <DashboardPage />
            </PrivateRoute>
          }
        />
        {/* Jeśli trafi się na nieznaną ścieżkę, przekierowuje do strony głównej */}
        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </Router>
  );
};

const AppWithProvider: React.FC = () => (
  <AuthProvider>
    <App />
  </AuthProvider>
);

export default AppWithProvider;
