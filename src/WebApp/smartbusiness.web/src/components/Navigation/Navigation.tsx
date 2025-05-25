import type { Page } from "../../models";

import { AuthenticatedDashboard } from "./AuthenticatedDashboard";
import { NotAuthenticatedDashboard } from "./NotAuthenticatedDashboard";

// Navigation Component
export const Navigation: React.FC<{
  currentPage: Page;
  onNavigate: (page: Page) => void;
}> = ({ currentPage, onNavigate }) => {
  if (currentPage === "dashboard") {
    return <AuthenticatedDashboard onNavigate={onNavigate} />;
  }

  return <NotAuthenticatedDashboard onNavigate={onNavigate} />;
};
