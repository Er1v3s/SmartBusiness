import { createContext, useContext } from "react";
import type { AccountContextType } from "../../models/account";

export const AccountContext = createContext<AccountContextType | undefined>(
  undefined,
);

export const useAccount = () => {
  const context = useContext(AccountContext);
  if (context === undefined) {
    throw new Error("useAccount must be used within an AccountProvider");
  }
  return context;
};
