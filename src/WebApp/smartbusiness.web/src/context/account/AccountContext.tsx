import { createContext, useContext } from "react";
import type { AccountContextType } from "../../models";

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
