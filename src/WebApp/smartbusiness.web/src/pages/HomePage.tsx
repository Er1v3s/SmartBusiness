import { Shield } from "lucide-react";
import { ArrowRight, Zap, Users } from "lucide-react";
import React from "react";
import { useAuth } from "../context/auth/AuthContext";
import { useNavigate } from "react-router-dom";

// Home Page Component
export const HomePage: React.FC = () => {
  const { isAuthenticated } = useAuth();
  const navigate = useNavigate();

  return (
    <div className="min-h-screen bg-gradient-to-br from-indigo-900 via-purple-900 to-pink-800">
      {/* Hero Section */}
      <div className="relative z-10 mx-auto max-w-4xl px-6 pt-30 pb-32 text-center">
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
      {/* Features */}
      <div className="relative z-10 mx-auto max-w-6xl px-6 pb-20">
        <div className="grid gap-8 md:grid-cols-3">
          <div className="rounded-2xl border border-white/20 bg-white/10 p-6 backdrop-blur-lg">
            <Shield className="mb-4 h-12 w-12 text-cyan-400" />
            <h3 className="mb-2 text-xl font-semibold text-white">
              Bezpieczeństwo
            </h3>
            <p className="text-gray-300">
              Zaawansowane szyfrowanie i uwierzytelnianie JWT
            </p>
          </div>

          <div className="rounded-2xl border border-white/20 bg-white/10 p-6 backdrop-blur-lg">
            <Zap className="mb-4 h-12 w-12 text-purple-400" />
            <h3 className="mb-2 text-xl font-semibold text-white">Wydajność</h3>
            <p className="text-gray-300">
              Błyskawiczna responsywność i optymalizacja
            </p>
          </div>

          <div className="rounded-2xl border border-white/20 bg-white/10 p-6 backdrop-blur-lg">
            <Users className="mb-4 h-12 w-12 text-pink-400" />
            <h3 className="mb-2 text-xl font-semibold text-white">
              Współpraca
            </h3>
            <p className="text-gray-300">
              Zespołowe narzędzia do efektywnej pracy
            </p>
          </div>
        </div>
      </div>
      {/* Features */}
      <div className="relative z-10 mx-auto max-w-6xl px-6 pb-20">
        <div className="grid gap-8 md:grid-cols-3">
          <div className="rounded-2xl border border-white/20 bg-white/10 p-6 backdrop-blur-lg">
            <Shield className="mb-4 h-12 w-12 text-cyan-400" />
            <h3 className="mb-2 text-xl font-semibold text-white">
              Bezpieczeństwo
            </h3>
            <p className="text-gray-300">
              Zaawansowane szyfrowanie i uwierzytelnianie JWT
            </p>
          </div>

          <div className="rounded-2xl border border-white/20 bg-white/10 p-6 backdrop-blur-lg">
            <Zap className="mb-4 h-12 w-12 text-purple-400" />
            <h3 className="mb-2 text-xl font-semibold text-white">Wydajność</h3>
            <p className="text-gray-300">
              Błyskawiczna responsywność i optymalizacja
            </p>
          </div>

          <div className="rounded-2xl border border-white/20 bg-white/10 p-6 backdrop-blur-lg">
            <Users className="mb-4 h-12 w-12 text-pink-400" />
            <h3 className="mb-2 text-xl font-semibold text-white">
              Współpraca
            </h3>
            <p className="text-gray-300">
              Zespołowe narzędzia do efektywnej pracy
            </p>
          </div>
        </div>
      </div>
      {/* Features */}
      <div className="relative z-10 mx-auto max-w-6xl px-6 pb-20">
        <div className="grid gap-8 md:grid-cols-3">
          <div className="rounded-2xl border border-white/20 bg-white/10 p-6 backdrop-blur-lg">
            <Shield className="mb-4 h-12 w-12 text-cyan-400" />
            <h3 className="mb-2 text-xl font-semibold text-white">
              Bezpieczeństwo
            </h3>
            <p className="text-gray-300">
              Zaawansowane szyfrowanie i uwierzytelnianie JWT
            </p>
          </div>

          <div className="rounded-2xl border border-white/20 bg-white/10 p-6 backdrop-blur-lg">
            <Zap className="mb-4 h-12 w-12 text-purple-400" />
            <h3 className="mb-2 text-xl font-semibold text-white">Wydajność</h3>
            <p className="text-gray-300">
              Błyskawiczna responsywność i optymalizacja
            </p>
          </div>

          <div className="rounded-2xl border border-white/20 bg-white/10 p-6 backdrop-blur-lg">
            <Users className="mb-4 h-12 w-12 text-pink-400" />
            <h3 className="mb-2 text-xl font-semibold text-white">
              Współpraca
            </h3>
            <p className="text-gray-300">
              Zespołowe narzędzia do efektywnej pracy
            </p>
          </div>
        </div>
      </div>
      {/* Features */}
      <div className="relative z-10 mx-auto max-w-6xl px-6 pb-20">
        <div className="grid gap-8 md:grid-cols-3">
          <div className="rounded-2xl border border-white/20 bg-white/10 p-6 backdrop-blur-lg">
            <Shield className="mb-4 h-12 w-12 text-cyan-400" />
            <h3 className="mb-2 text-xl font-semibold text-white">
              Bezpieczeństwo
            </h3>
            <p className="text-gray-300">
              Zaawansowane szyfrowanie i uwierzytelnianie JWT
            </p>
          </div>

          <div className="rounded-2xl border border-white/20 bg-white/10 p-6 backdrop-blur-lg">
            <Zap className="mb-4 h-12 w-12 text-purple-400" />
            <h3 className="mb-2 text-xl font-semibold text-white">Wydajność</h3>
            <p className="text-gray-300">
              Błyskawiczna responsywność i optymalizacja
            </p>
          </div>

          <div className="rounded-2xl border border-white/20 bg-white/10 p-6 backdrop-blur-lg">
            <Users className="mb-4 h-12 w-12 text-pink-400" />
            <h3 className="mb-2 text-xl font-semibold text-white">
              Współpraca
            </h3>
            <p className="text-gray-300">
              Zespołowe narzędzia do efektywnej pracy
            </p>
          </div>
        </div>
      </div>

      {/* Background Effects */}
      <div className="realtive inset-0 overflow-hidden">
        {/* <div className="absolute -top-0 -right-40 h-80 w-80 animate-pulse rounded-full bg-purple-500 opacity-70 mix-blend-multiply blur-xl filter"></div> */}
        {/* <div className="absolute -bottom-0 -left-40 h-80 w-80 animate-pulse rounded-full bg-cyan-500 opacity-20 mix-blend-multiply blur-xl filter"></div> */}
      </div>
    </div>
  );
};
