import React, { useEffect, useState } from "react";
import { Sun, Moon } from "lucide-react";

// Ustal motyw na podstawie preferencji systemu lub localStorage
function getInitialTheme(): "dark" | "light" {
  if (typeof window === "undefined") return "light";
  const saved = localStorage.getItem("theme");
  if (saved === "dark" || saved === "light") return saved;
  if (window.matchMedia("(prefers-color-scheme: dark)").matches) return "dark";
  return "light";
}

export const ThemeToggleButton: React.FC = () => {
  const [theme, setTheme] = useState<"dark" | "light">(() => getInitialTheme());

  // Ustaw klasę na <html> natychmiast po zmianie theme
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

  // Synchronizacja z systemem, jeśli użytkownik nie wybrał ręcznie
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

  // Po załadowaniu komponentu, sprawdź czy theme w localStorage jest zgodny z klasą na <html>
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
      className="rounded-full p-2 text-indigo-500 transition hover:bg-indigo-100 dark:text-yellow-300 dark:hover:bg-indigo-900"
      onClick={() => setTheme(theme === "dark" ? "light" : "dark")}
      title={theme === "dark" ? "Tryb jasny" : "Tryb ciemny"}
      type="button"
    >
      {theme === "dark" ? (
        <Sun className="h-6 w-6" />
      ) : (
        <Moon className="h-6 w-6" />
      )}
    </button>
  );
};
