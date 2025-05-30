import clsx from "clsx";
import { useEffect, useState } from "react";
import { alertIcons, alertTypes, type AlertProps } from "./alertProps";

export const Alert: React.FC<AlertProps> = (props) => {
  const [visible, setVisible] = useState(false);
  const [shouldRender, setShouldRender] = useState(true);

  useEffect(() => {
    const showTimeout = setTimeout(() => setVisible(true), 50);
    const autoHideTimeout = setTimeout(() => setVisible(false), props.duration);

    return () => {
      clearTimeout(showTimeout);
      clearTimeout(autoHideTimeout);
    };
  }, [props.duration]);

  useEffect(() => {
    if (!visible) {
      const timeout = setTimeout(() => setShouldRender(false), 500);
      return () => clearTimeout(timeout);
    }
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
            <svg
              className="h-5 w-5"
              viewBox="0 0 24 24"
              stroke="currentColor"
              fill="none"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth={2}
                d="M6 18L18 6M6 6l12 12"
              />
            </svg>
          </button>
        </div>
      </div>
    </div>
  );
};
