import clsx from "clsx";
import { useEffect, useState } from "react";
import { Check, Info, TriangleAlert, X } from "lucide-react";

export type AlertProps = {
  title: string;
  message: string;
  type: "success" | "error" | "warning" | "info";
  duration?: number;
};

const alertTypes: Record<string, string> = {
  success: "text-green-600",
  error: "text-red-600",
  warning: "text-yellow-600",
  info: "text-blue-600",
};

const alertIcons: Record<string, React.ReactNode> = {
  success: <Check />,
  error: <X />,
  warning: <TriangleAlert />,
  info: <Info />,
};

export const Alert: React.FC<AlertProps> = (props) => {
  const [visible, setVisible] = useState(false);
  const [shouldRender, setShouldRender] = useState(true);

  useEffect(() => {
    setShouldRender(true); // Always render on mount
    setVisible(false); // Start hidden
    const showTimeout = setTimeout(() => setVisible(true), 50);
    const autoHideTimeout = setTimeout(
      () => setVisible(false),
      props.duration ?? 3000,
    );
    return () => {
      clearTimeout(showTimeout);
      clearTimeout(autoHideTimeout);
    };
  }, [props.duration]);

  useEffect(() => {
    if (!visible) {
      // Fade out: wait for transition to finish before unmount
      const timeout = setTimeout(() => setShouldRender(false), 500);
      return () => clearTimeout(timeout);
    }
    // Fade in: ensure shouldRender is true
    setShouldRender(true);
  }, [visible]);

  if (!shouldRender) return null;

  return (
    <div
      className={clsx(
        "fixed top-0 right-0 left-0 z-50 flex justify-center transition-all duration-500 ease-in-out",
        visible ? "translate-y-5 opacity-100" : "-translate-y-full opacity-0",
      )}
    >
      <div className="mx-auto w-full max-w-md rounded-md border border-gray-300 bg-white p-4 shadow-lg dark:border-gray-600 dark:bg-gray-800">
        <div className="flex items-start gap-4">
          <div className={`flex-shrink-0 ${alertTypes[props.type]}`}>
            {alertIcons[props.type]}
          </div>
          <div className="flex-1">
            <strong className="font-medium text-gray-900 dark:text-white">
              {props.title}
            </strong>
            <p className="mt-0.5 text-sm text-gray-700 dark:text-gray-200">
              {props.message}
            </p>
          </div>
          <button
            onClick={() => setVisible(false)}
            className="-m-2 rounded-full p-2 text-gray-500 transition hover:bg-gray-100 dark:text-gray-400 dark:hover:bg-gray-700"
          >
            <span className="sr-only">Dismiss</span>
            <X className="h-5 w-5" />
          </button>
        </div>
      </div>
    </div>
  );
};
