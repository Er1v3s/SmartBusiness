import React, { useState, useCallback, type ReactNode } from "react";
import { Alert } from "../../components/General/Alert";
import type { AlertProps } from "../../components/General/alertProps";
import { AlertContext } from "./AlertContextDef";

export const AlertProvider: React.FC<{ children: ReactNode }> = ({
  children,
}) => {
  const [alertData, setAlertData] = useState<AlertProps | null>(null);
  const [show, setShow] = useState(false);

  const showAlert = useCallback((props: AlertProps) => {
    setAlertData(props);
    setShow(true);
    setTimeout(() => setShow(false), (props.duration ?? 3000) + 500); // 500ms for fade out
  }, []);

  return (
    <AlertContext.Provider value={{ showAlert }}>
      {children}
      {show && alertData && (
        <Alert
          title={alertData.title}
          message={alertData.message}
          type={alertData.type}
          duration={alertData.duration}
        />
      )}
    </AlertContext.Provider>
  );
};
