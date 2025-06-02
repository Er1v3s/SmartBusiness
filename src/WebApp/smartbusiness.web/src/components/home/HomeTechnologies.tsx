import React, { useRef, useEffect, useState } from "react";
import dot_net_core_logo from "../../assets/dot_net_core_logo.svg";
import react_logo from "../../assets/react_logo.svg";
import tailwindcss_logo from "../../assets/tailwindcss.svg";
import microsoft_azure_logo from "../../assets/microsoft_azure_logo.svg";
import ml_net_logo from "../../assets/ml_net_logo.svg";
import { motion, useAnimation } from "framer-motion";

const techs = [
  {
    logo: dot_net_core_logo,
    name: ".NET 9",
    desc: "C# Backend",
    color: "text-cyan-100",
    link: "https://learn.microsoft.com/pl-pl/dotnet/fundamentals/",
  },
  {
    logo: react_logo,
    name: "React",
    desc: "TypeScript Frontend",
    color: "text-blue-100",
    link: "https://react.dev/",
  },
  {
    logo: tailwindcss_logo,
    name: "Tailwind",
    desc: "UI/UX",
    color: "text-cyan-100",
    link: "https://tailwindcss.com",
  },
  {
    logo: microsoft_azure_logo,
    name: "Azure",
    desc: "Cloud",
    color: "text-blue-100",
    link: "https://learn.microsoft.com/azure/",
  },
  {
    logo: ml_net_logo,
    name: "ML.NET",
    desc: "AI/ML",
    color: "text-pink-100",
    link: "https://learn.microsoft.com/dotnet/machine-learning/",
  },
];

// To create a marquee effect, we duplicate the techs array
const marqueeTechs = [...techs, ...techs];

export const HomeTechnologies: React.FC = () => {
  const marqueeRef = useRef<HTMLDivElement>(null);
  const [width, setWidth] = useState(0);
  const controls = useAnimation();

  useEffect(() => {
    if (marqueeRef.current) {
      setWidth(marqueeRef.current.scrollWidth);
    }
  }, []);

  useEffect(() => {
    if (width > 0) {
      controls.start({
        x: [0, -width / 2],
        transition: {
          x: {
            repeat: Infinity,
            repeatType: "loop",
            duration: 30,
            ease: "linear",
          },
        },
      });
    }
  }, [width, controls]);

  return (
    <section className="relative z-10 mx-auto mt-4 max-w-5xl overflow-hidden px-6 pb-15">
      <h2 className="mb-8 text-center text-2xl font-bold tracking-tight text-white/90">
        Technologie
      </h2>
      <div className="relative w-full overflow-x-hidden p-1">
        <motion.div
          className="flex min-w-max gap-16"
          ref={marqueeRef}
          animate={controls}
          style={{ x: 0 }}
        >
          {marqueeTechs.map((t, i) => (
            <div key={i} className="flex min-w-[120px] flex-col items-center">
              <a
                href={t.link}
                target="_blank"
                rel="noopener noreferrer"
                className="mb-2 flex h-14 w-14 items-center justify-center rounded-full bg-white/10 transition hover:scale-110 hover:bg-white/20"
              >
                <img src={t.logo} alt={t.name} className="h-8 w-8" />
              </a>
              <span className={`text-base font-semibold ${t.color}`}>
                {t.name}
              </span>
              <span className="text-center text-xs text-white/60">
                {t.desc}
              </span>
            </div>
          ))}
        </motion.div>
      </div>
    </section>
  );
};
