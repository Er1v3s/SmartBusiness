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
import { AccountProvider } from "./context/account/AccountProvider";
import { AlertProvider } from "./context/alert/AlertContext";
import { ForgotPassword } from "./pages/Auth/ForgotPassword";
import { ResetPassword } from "./pages/Auth/ResetPassword";
import "./App.css";
import { UserPage } from "./pages/User/UserPage";
import { CompanyProvider } from "./context/company/CompanyProvider";
import { Outlet } from "react-router-dom";
import { SummaryComponent } from "./components/Dashboard/User/Summary";
import { ChangePasswordComponent } from "./components/Dashboard/User/ChangePassword";
import { StatsComponent } from "./components/Dashboard/User/Stats";
import { DeleteAccountComponent } from "./components/Dashboard/User/DeleteAccount";
import { EditProfileComponent } from "./components/Dashboard/User/EditProfile";
import { DashboardHomeSection } from "./components/Dashboard/Company/DashboardHomeSection";
import { CalendarSection } from "./components/Dashboard/Company/CalendarSection";

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
      <div className="flex min-h-screen flex-col">
        <Navigation />
        <Routes>
          {/* Redairections route */}
          <Route path="/" element={<RedirectRoute />} />

          {/* Private paths */}
          <Route
            element={
              <PrivateRoute>
                <Outlet />
              </PrivateRoute>
            }
          >
            <Route path="dashboard" element={<DashboardPage />}>
              <Route index element={<DashboardHomeSection />} />
              <Route path="company/calendar" element={<CalendarSection />} />
              <Route
                path="company/sale"
                element={<div>Rejestracja sprzedaży (TODO: komponent)</div>}
              />
              <Route
                path="company/stats"
                element={<div>Statystyki firmy (TODO: komponent)</div>}
              />
              <Route
                path="company/settings"
                element={<div>Ustawienia firmy (TODO: komponent)</div>}
              />
            </Route>

            <Route path="dashboard/user" element={<UserPage />}>
              <Route index element={<SummaryComponent />} />
              <Route path="edit-profile" element={<EditProfileComponent />} />
              <Route
                path="change-password"
                element={<ChangePasswordComponent />}
              />
              <Route path="stats" element={<StatsComponent />} />
              <Route
                path="delete-account"
                element={<DeleteAccountComponent />}
              />
            </Route>
          </Route>

          {/* Public paths */}
          <Route
            element={
              <PublicRoute>
                <Outlet />
              </PublicRoute>
            }
          >
            <Route path="home" element={<HomePage />} />
            <Route path="login" element={<LoginPage />} />
            <Route path="register" element={<RegisterPage />} />
            <Route path="forgot-password" element={<ForgotPassword />} />
            <Route path="reset-password" element={<ResetPassword />} />
          </Route>

          {/* UNKNOWN PATH REDIRECT TO '/' */}
          <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
      </div>
    </Router>
  );
};

const AppWithProvider: React.FC = () => (
  <AuthProvider>
    <AccountProvider>
      <CompanyProvider>
        <AlertProvider>
          <App />
        </AlertProvider>
      </CompanyProvider>
    </AccountProvider>
  </AuthProvider>
);

export default AppWithProvider;
