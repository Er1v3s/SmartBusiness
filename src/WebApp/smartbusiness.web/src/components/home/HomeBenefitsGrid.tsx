import React from "react";
import { Shield, Zap, Users, ArrowRight, Timer } from "lucide-react";
import { HomeBenefitCard } from "./HomeBenefitCard";

export const HomeBenefitsGrid: React.FC = () => (
  <div className="mt-4 grid grid-cols-1 gap-4 md:grid-cols-2">
    <HomeBenefitCard
      icon={<Shield className="mt-1 h-15 w-15 text-white/70" />}
      title="Bezpieczeństwo na poziomie bankowym"
      description="Twoje dane są szyfrowane i chronione jak w najlepszych instytucjach finansowych."
    />
    <HomeBenefitCard
      icon={<Zap className="mt-1 h-15 w-15 text-white/70" />}
      title="Automatyczne analizy i prognozy"
      description="Otrzymujesz gotowe raporty i przewidywania, które pomagają podejmować trafne decyzje."
    />
    <HomeBenefitCard
      icon={<Users className="mt-1 h-15 w-15 text-white/70" />}
      title="Współpraca bez barier"
      description="Zespół pracuje razem – w biurze i zdalnie, z każdego miejsca na świecie."
    />
    <HomeBenefitCard
      icon={<ArrowRight className="mt-1 h-15 w-15 text-white/70" />}
      title="Oszczędność czasu i pieniędzy"
      description="Automatyzacja procesów pozwala skupić się na rozwoju firmy, a nie na papierkowej robocie."
    />
    <HomeBenefitCard
      icon={<Timer className="mt-1 h-15 w-15 text-white/70" />}
      title="Dostępność 24/7"
      description="Masz dostęp do wszystkich danych i narzędzi zawsze, gdy ich potrzebujesz – bez przerw i ograniczeń."
    />
    <HomeBenefitCard
      icon={<Zap className="mt-1 h-15 w-15 text-white/70" />}
      title="Intuicyjna obsługa"
      description="Nie musisz być ekspertem IT – wszystko jest proste, szybkie i przyjazne dla każdego użytkownika."
    />
  </div>
);
