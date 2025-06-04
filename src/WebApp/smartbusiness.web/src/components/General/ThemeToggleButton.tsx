import React, { useEffect, useState } from "react";
import { Sun, Moon } from "lucide-react";

// Set the theme based on system preferences or localStorage
function getInitialTheme(): "dark" | "light" {
  if (typeof window === "undefined") return "light";
  const saved = localStorage.getItem("theme");
  if (saved === "dark" || saved === "light") return saved;
  if (window.matchMedia("(prefers-color-scheme: dark)").matches) return "dark";
  return "light";
}

export const ThemeToggleButton: React.FC = () => {
  const [theme, setTheme] = useState<"dark" | "light">(() => getInitialTheme());

  // Set the class on <html> immediately after theme change
  useEffect(() => {
    const root = window.document.documentElement;
    if (theme === "dark") {
      root.classList.add("dark");
      localStorage.setItem("theme", "dark");
    } else {
      root.classList.remove("dark");
      localStorage.setItem("theme", "light");
    }
  }, [theme]);

  // Synchronize with system preferences if user hasn't manually set a theme
  useEffect(() => {
    const mq = window.matchMedia("(prefers-color-scheme: dark)");
    const handleChange = () => {
      const saved = localStorage.getItem("theme");
      if (!saved) {
        setTheme(mq.matches ? "dark" : "light");
      }
    };
    mq.addEventListener("change", handleChange);
    return () => mq.removeEventListener("change", handleChange);
  }, []);

  // After initial render, ensure the theme matches the saved preference
  useEffect(() => {
    const root = window.document.documentElement;
    const saved = localStorage.getItem("theme");
    if (saved === "dark" && !root.classList.contains("dark")) {
      root.classList.add("dark");
    }
    if (saved === "light" && root.classList.contains("dark")) {
      root.classList.remove("dark");
    }
  }, []);

  return (
    <label
      htmlFor="AcceptConditions"
      className="relative block h-8 w-14 rounded-full bg-gray-300 transition-colors [-webkit-tap-highlight-color:_transparent] has-checked:bg-gray-800 dark:bg-gray-600 dark:has-checked:bg-gray-800"
    >
      <input
        type="checkbox"
        id="AcceptConditions"
        className="peer sr-only"
        onChange={() => setTheme(theme === "dark" ? "light" : "dark")}
        checked={theme === "dark"}
      />

      <span className="absolute inset-y-0 start-0 m-1 size-6 cursor-pointer rounded-full transition-[inset-inline-start] peer-checked:start-6">
        {theme === "dark" ? (
          <Moon className="h-6 w-6 text-gray-200" />
        ) : (
          <Sun className="h-6 w-6 text-yellow-200" />
        )}
      </span>
    </label>
  );
};
