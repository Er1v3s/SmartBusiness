import { useAuth } from "../../context/AuthContext";

import { AuthenticatedDashboard } from "./AuthenticatedDashboard";
import { NotAuthenticatedDashboard } from "./NotAuthenticatedDashboard";

// Navigation Component
export const Navigation: React.FC = () => {
  const { isAuthenticated } = useAuth();
  if (isAuthenticated) {
    return <AuthenticatedDashboard />;
  }

  return <NotAuthenticatedDashboard />;
};
