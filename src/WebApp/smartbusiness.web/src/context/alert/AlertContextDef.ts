import { createContext } from "react";
import type { AlertProps } from "../../components/General/Alert";

export interface AlertContextType {
  showAlert: (props: AlertProps) => void;
}

export const AlertContext = createContext<AlertContextType | undefined>(undefined);
