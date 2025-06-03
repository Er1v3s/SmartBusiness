import React from "react";
import { motion } from "framer-motion";
import dot_net_core_logo from "../../assets/dot_net_core_logo.svg";
import react_logo from "../../assets/react_logo.svg";
import tailwindcss_logo from "../../assets/tailwindcss.svg";
import microsoft_azure_logo from "../../assets/microsoft_azure_logo.svg";
import ml_net_logo from "../../assets/ml_net_logo.svg";

const techDescriptions = [
  {
    logo: dot_net_core_logo,
    title: "Stabilność i bezpieczeństwo Twoich danych",
    color: "text-blue-300",
    desc1:
      "Dzięki nowoczesnej technologii .NET 9 Twoje dane są przechowywane i przetwarzane w sposób bezpieczny i niezawodny. System działa płynnie nawet przy dużej liczbie użytkowników, a każda funkcja jest zaprojektowana z myślą o Twoim komforcie i bezpieczeństwie.",
    desc2:
      "Możesz mieć pewność, że Twoje informacje są chronione i zawsze dostępne, gdy ich potrzebujesz.",
    reverse: false,
    link: "https://learn.microsoft.com/dotnet/core/",
  },
  {
    logo: react_logo,
    title: "Intuicyjna i szybka obsługa",
    color: "text-green-300",
    desc1:
      "Interfejs SmartBusiness jest przejrzysty i łatwy w obsłudze – wszystko znajdziesz tam, gdzie się tego spodziewasz. Dzięki nowoczesnym rozwiązaniom możesz korzystać z platformy wygodnie na komputerze, tablecie i telefonie.",
    desc2:
      "Szybko wykonasz najważniejsze zadania, nawet jeśli nie masz doświadczenia z podobnymi systemami.",
    reverse: true,
    link: "https://react.dev/",
  },
  {
    logo: tailwindcss_logo,
    title: "Nowoczesny wygląd i wygoda",
    color: "text-blue-300",
    desc1:
      "Platforma jest nie tylko funkcjonalna, ale też atrakcyjna wizualnie. Przejrzysty układ i czytelne elementy sprawiają, że praca z systemem jest przyjemna i nie męczy wzroku nawet podczas dłuższego korzystania.",
    desc2:
      "Wszystko jest zaprojektowane tak, byś mógł skupić się na tym, co najważniejsze dla Twojego biznesu.",
    reverse: false,
    link: "https://tailwindcss.com/docs",
  },
  {
    logo: ml_net_logo,
    title: "Inteligentne prognozy i analizy",
    color: "text-pink-300",
    desc1:
      "Dzięki sztucznej inteligencji SmartBusiness analizuje dane i podpowiada najlepsze decyzje. Otrzymujesz prognozy sprzedaży, wykrywanie trendów i praktyczne wskazówki, które pomagają rozwijać firmę i unikać niepotrzebnych strat.",
    desc2:
      "Zyskujesz przewagę nad konkurencją, bo masz dostęp do narzędzi, z których korzystają największe firmy.",
    reverse: true,
    link: "https://learn.microsoft.com/dotnet/machine-learning/",
  },
  {
    logo: microsoft_azure_logo,
    title: "Dostępność zawsze i wszędzie",
    color: "text-blue-300",
    desc1:
      "Twoje dane są bezpiecznie przechowywane w chmurze Microsoft Azure. Masz do nich dostęp z każdego miejsca i urządzenia, a kopie zapasowe chronią Cię przed utratą ważnych informacji.",
    desc2:
      "Nie musisz martwić się o awarie czy utratę danych – wszystko jest automatycznie zabezpieczone.",
    reverse: false,
    link: "https://learn.microsoft.com/azure/",
  },
];

export const HomeTechDescriptionSection: React.FC = () => (
  <section className="relative z-10 mx-auto flex max-w-6xl flex-col gap-16 px-2 pb-15 sm:px-4 md:px-6">
    {techDescriptions.map((t, i) => (
      <motion.div
        key={i}
        className={`flex flex-col ${
          t.reverse ? "md:flex-row-reverse" : "md:flex-row"
        } w-full items-center gap-8 md:gap-10`}
        initial={{ opacity: 0, x: t.reverse ? 100 : -100 }}
        whileInView={{ opacity: 1, x: 0 }}
        viewport={{ once: true, amount: 0.3 }}
        transition={{ duration: 0.7, delay: i * 0.15, ease: "easeOut" }}
      >
        <div className="mb-4 flex w-full items-center justify-center md:mb-0 md:w-1/3">
          <a
            href={t.link}
            target="_blank"
            rel="noopener noreferrer"
            className="flex h-32 w-32 items-center justify-center transition-transform duration-200 hover:scale-110 sm:h-40 sm:w-40 md:h-48 md:w-48"
            tabIndex={0}
            aria-label={`Przejdź do dokumentacji ${t.title}`}
          >
            <img
              src={t.logo}
              alt={t.title}
              className="h-auto max-h-full w-auto max-w-full"
            />
          </a>
        </div>
        <div className="w-full md:w-2/3">
          <h3 className={`mb-3 text-2xl font-bold ${t.color}`}>{t.title}</h3>
          <p className="mb-2 text-gray-200">{t.desc1}</p>
          <p className="text-sm text-gray-400">{t.desc2}</p>
        </div>
      </motion.div>
    ))}
  </section>
);
