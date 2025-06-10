import type { AccountContextType } from "../../models/account.ts";
import apiAccountConnector from "../../api/apiAccountConnector.ts";
import { AccountContext } from "./AccountContext.tsx";
import { removeAccessTokens } from "../auth/TokenManager.ts";

export const AccountProvider: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {
  const updateAccount = async (
    username: string,
    email: string,
  ): Promise<void> => {
    await apiAccountConnector.updateProfile(username, email);
  };

  const changePassword = async (
    currentPassword: string,
    newPassword: string,
  ): Promise<void> => {
    await apiAccountConnector.changePassword(currentPassword, newPassword);
  };

  const deleteAccount = async (password: string): Promise<void> => {
    await apiAccountConnector.deleteAccount(password);
    removeAccessTokens();
    // Force a re-render of the AuthContext to update the user state
    window.dispatchEvent(new Event("storage"));
  };

  // Context value
  const value: AccountContextType = {
    updateAccount,
    changePassword,
    deleteAccount,
  };

  return (
    <AccountContext.Provider value={value}>{children}</AccountContext.Provider>
  );
};
