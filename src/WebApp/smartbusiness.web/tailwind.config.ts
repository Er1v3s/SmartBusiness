import type { Config } from "tailwindcss";

export default {
  content: [
    "./index.html", 
    "./src/**/*.{js,ts,jsx,tsx}",
    "./src/**/*.{html,js,ts,jsx,tsx}",],
  theme: {
    extend: {},
  },
  darkMode: 'class',
  plugins: [],
} satisfies Config;
