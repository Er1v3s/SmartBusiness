import React, { type JSX } from "react";
import {
  BrowserRouter as Router,
  Routes,
  Route,
  Navigate,
} from "react-router-dom";
import "./App.css";
import { Outlet } from "react-router-dom";
import { Navigation } from "./components/Navigation/Navigation";
import { useAuth } from "./context/auth/AuthContext";
import { AuthProvider } from "./context/auth/AuthProvider";
import { TransactionProvider } from "./context/transaction/TransactionProvider";
import { ServiceProvider } from "./context/service/ServiceProvider";
import { AlertProvider } from "./context/alert/AlertContext";
import { AccountProvider } from "./context/account/AccountProvider";
import { CompanyProvider } from "./context/company/CompanyProvider";
import { ProductProvider } from "./context/product/ProductProvider";
import { HomePage } from "./pages/HomePage";
import { LoginPage } from "./pages/Auth/LoginPage";
import { RegisterPage } from "./pages/Auth/RegisterPage";
import { DashboardPage } from "./pages/DashboardPage";
import { UserPage } from "./pages/User/UserPage";
import { CompanyPage } from "./pages/Company/CompanyPage";
import { ForgotPassword } from "./pages/Auth/ForgotPassword";
import { ResetPassword } from "./pages/Auth/ResetPassword";
import { SummaryComponent } from "./components/Dashboard/User/Summary";
import { ChangePasswordComponent } from "./components/Dashboard/User/ChangePassword";
import { StatsComponent } from "./components/Dashboard/User/Stats";
import { DeleteAccountComponent } from "./components/Dashboard/User/DeleteAccount";
import { EditProfileComponent } from "./components/Dashboard/User/EditProfile";
import { DashboardHomeSection } from "./components/Dashboard/Company/DashboardHomeSection";
import { CalendarSection } from "./components/Dashboard/Company/CalendarSection";
import { StatisticsSection } from "./components/Dashboard/Company/StatisticsSection";
import { TransactionsSection } from "./components/Dashboard/Company/TransactionsSection";
import { RegisterSaleSection } from "./components/Dashboard/Company/RegisterSaleSection";
import { ServicesSection } from "./components/Dashboard/Company/ServicesSection";
import { ProductsSection } from "./components/Dashboard/Company/ProductsSection";
import { CompanySummary } from "./components/Dashboard/Company/CompanySettings/CompanySummary";
import { CompanyAdd } from "./components/Dashboard/Company/CompanySettings/CompanyAdd";
import { CompanyList } from "./components/Dashboard/Company/CompanySettings/CompanyList";
import { CompanyDelete } from "./components/Dashboard/Company/CompanySettings/CompanyDelete";

// Private Route component checks if the user is authenticated
const PrivateRoute: React.FC<{ children: JSX.Element }> = ({ children }) => {
  const { isAuthenticated } = useAuth();
  return isAuthenticated ? children : <Navigate to="/login" replace />;
};

// PublicRoute component checks if the user is not authenticated
const PublicRoute: React.FC<{ children: JSX.Element }> = ({ children }) => {
  const { isAuthenticated } = useAuth();
  return !isAuthenticated ? (
    children
  ) : (
    <Navigate to="/dashboard/summary" replace />
  );
};

// RedirectRoute component redirects based on authentication status
const RedirectRoute: React.FC = () => {
  const { isAuthenticated } = useAuth();
  return (
    <Navigate to={isAuthenticated ? "/dashboard/summary" : "/home"} replace />
  );
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
              <Route path="summary" element={<DashboardHomeSection />} />
              <Route path="calendar" element={<CalendarSection />} />
              <Route path="sales-panel" element={<RegisterSaleSection />} />
              <Route path="services" element={<ServicesSection />} />
              <Route path="products" element={<ProductsSection />} />
              <Route path="transactions" element={<TransactionsSection />} />
              <Route path="statistics" element={<StatisticsSection />} />
            </Route>

            <Route path="dashboard/company/settings" element={<CompanyPage />}>
              <Route index element={<CompanySummary />} />
              <Route path="summary" element={<CompanySummary />} />
              <Route path="add" element={<CompanyAdd />} />
              <Route path="list" element={<CompanyList />} />
              <Route path="delete" element={<CompanyDelete />} />
            </Route>

            <Route path="dashboard/user/settings" element={<UserPage />}>
              <Route index path="summary" element={<SummaryComponent />} />
              <Route path="summary" element={<SummaryComponent />} />
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
            {/* fallback when page does not exist */}
            <Route path="*" element={<Navigate to="/home" replace />} />
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
          <ProductProvider>
            <ServiceProvider>
              <TransactionProvider>
                <App />
              </TransactionProvider>
            </ServiceProvider>
          </ProductProvider>
        </AlertProvider>
      </CompanyProvider>
    </AccountProvider>
  </AuthProvider>
);

export default AppWithProvider;
