import { ArrowRight } from "lucide-react";
import React from "react";
import { useAuth } from "../context/auth/AuthContext";
import { useNavigate } from "react-router-dom";
import { HomeAboutSection } from "../components/Home/HomeAboutSection";
import { HomeFeatures } from "../components/Home/HomeFeatures";
import { HomeTechnologies } from "../components/Home/HomeTechnologies";
import { HomeTechDescriptionSection } from "../components/Home/HomeTechDescriptionSection";

export const HomePage: React.FC = () => {
  const { isAuthenticated } = useAuth();
  const navigate = useNavigate();

  return (
    <div className="min-h-screen bg-gradient-to-br from-indigo-900 via-purple-900 to-pink-800">
      {/* Hero Section */}
      <div className="relative z-10 mx-auto max-w-4xl px-6 pt-15 pb-15 text-center">
        <h1 className="mb-8 text-5xl leading-tight font-bold text-white lg:text-7xl">
          Bezpieczna platforma dla
          <span className="bg-gradient-to-r from-cyan-400 to-purple-400 bg-clip-text text-transparent">
            {" "}
            Twojego biznesu
          </span>
        </h1>
        <p className="mx-auto mb-12 max-w-3xl text-xl leading-relaxed text-gray-300 lg:text-2xl">
          Nowoczesne rozwiązanie zapewniające najwyższy poziom bezpieczeństwa i
          funkcjonalności dla Twoich projektów
        </p>
        <button
          onClick={() => navigate(isAuthenticated ? "/dashboard" : "/login")}
          className="group inline-flex transform cursor-pointer items-center rounded-full bg-gradient-to-r from-cyan-500 to-purple-600 px-8 py-4 text-lg font-semibold text-white shadow-2xl transition-all duration-200 hover:scale-105 hover:from-cyan-600 hover:to-purple-700"
        >
          Get Started
          <ArrowRight className="ml-2 h-5 w-5 transition-transform duration-200 group-hover:translate-x-1" />
        </button>
      </div>

      {/* 3 general cards */}
      <HomeFeatures />

      {/* About project */}
      <HomeAboutSection />

      {/* Technologies */}
      <HomeTechnologies />

      {/* Technologies description */}
      <HomeTechDescriptionSection />

      {/* Footer */}
      <footer className="relative z-20 mt-12 w-full border-t border-white/20 bg-white/10 py-8 text-center text-sm text-gray-300 backdrop-blur-lg">
        <div className="mx-auto flex max-w-6xl flex-col items-center justify-center gap-2 md:flex-row md:justify-between md:px-8">
          <span>
            © {new Date().getFullYear()} SmartBusiness. Wszelkie prawa
            zastrzeżone.
          </span>
          <span>
            Autor:{" "}
            <a
              href="https://github.com/Er1v3s"
              target="_blank"
              rel="noopener noreferrer"
              className="text-cyan-400 hover:underline"
            >
              Filip Statkiewicz
            </a>
          </span>
        </div>
      </footer>
    </div>
  );
};
