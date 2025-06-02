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
    <button
      aria-label="Przełącz motyw"
      className="text-gray-900 transition hover:scale-110 dark:text-yellow-200"
      onClick={() => setTheme(theme === "dark" ? "light" : "dark")}
      title={theme === "dark" ? "Tryb jasny" : "Tryb ciemny"}
      type="button"
    >
      {theme === "dark" ? (
        <Sun className="h-8 w-8" />
      ) : (
        <Moon className="h-8 w-8" />
      )}
    </button>
  );
};
