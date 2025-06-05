import React from "react";

interface GenericModalProps {
  open: boolean;
  title?: string;
  onClose: () => void;
  children: React.ReactNode;
  actions?: React.ReactNode;
}

const GenericModal: React.FC<GenericModalProps> = ({
  open,
  title,
  onClose,
  children,
  actions,
}) => {
  if (!open) return null;
  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/60 backdrop-blur-sm">
      <div className="relative w-full max-w-md rounded-lg bg-white p-6 shadow-lg dark:bg-gray-800">
        <button
          className="absolute top-3 right-3 text-gray-400 hover:text-gray-700 dark:hover:text-white"
          onClick={onClose}
          aria-label="Zamknij"
        >
          <span className="text-2xl">×</span>
        </button>
        {title && (
          <h2 className="mb-4 text-lg font-semibold text-gray-800 dark:text-gray-100">
            {title}
          </h2>
        )}
        <div>{children}</div>
        {actions && (
          <div className="mt-6 flex justify-between gap-2">{actions}</div>
        )}
      </div>
    </div>
  );
};

export default GenericModal;
