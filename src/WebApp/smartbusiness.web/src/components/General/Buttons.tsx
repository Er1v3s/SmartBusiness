import type { MouseEventHandler } from "react";

type ButtonProps = {
  text: string;
  type?: "button" | "submit" | "reset";
  onClick?: MouseEventHandler<HTMLButtonElement>;
  disabled?: boolean;
};

export const ButtonSuccess: React.FC<ButtonProps> = ({
  text,
  type = "button",
  onClick,
  disabled = false,
}) => {
  return (
    <button
      type={type}
      className="min-w-24 rounded bg-gradient-to-r from-indigo-600 to-indigo-500 px-4 py-2 font-semibold text-gray-100 shadow transition hover:from-indigo-700 hover:to-indigo-600 focus:ring-2 focus:ring-indigo-400 focus:outline-none disabled:cursor-not-allowed disabled:opacity-60"
      onClick={onClick}
      disabled={disabled}
    >
      {text}
    </button>
  );
};

export const ButtonError: React.FC<ButtonProps> = ({
  text,
  type = "button",
  onClick,
  disabled = false,
}) => {
  return (
    <button
      type={type}
      className="min-w-24 rounded bg-gradient-to-r from-red-700 to-red-400 px-4 py-2 font-semibold text-gray-100 shadow transition hover:from-red-600 hover:to-red-500 focus:ring-2 focus:ring-red-400 focus:outline-none disabled:cursor-not-allowed disabled:opacity-60"
      onClick={onClick}
      disabled={disabled}
    >
      {text}
    </button>
  );
};

export const ButtonNeutral: React.FC<ButtonProps> = ({
  text,
  type = "button",
  onClick,
  disabled = false,
}) => {
  return (
    <button
      type={type}
      className="min-w-24 rounded bg-gradient-to-r from-gray-600 to-gray-500 px-4 py-2 font-semibold text-gray-100 shadow transition hover:from-gray-700 hover:to-gray-600 focus:ring-2 focus:ring-gray-400 focus:outline-none disabled:cursor-not-allowed disabled:opacity-60"
      onClick={onClick}
      disabled={disabled}
    >
      {text}
    </button>
  );
};
