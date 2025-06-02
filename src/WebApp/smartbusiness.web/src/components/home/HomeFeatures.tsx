import React from "react";
import { Shield, Zap, Users } from "lucide-react";
import { motion } from "framer-motion";

const features = [
  {
    icon: <Shield className="mx-auto mb-4 h-12 w-12 text-cyan-400" />,
    title: "Bezpieczeństwo",
    description: "Zaawansowane szyfrowanie i uwierzytelnianie JWT",
  },
  {
    icon: <Zap className="mx-auto mb-4 h-12 w-12 text-purple-400" />,
    title: "Wydajność",
    description: "Błyskawiczna responsywność i optymalizacja",
  },
  {
    icon: <Users className="mx-auto mb-4 h-12 w-12 text-pink-400" />,
    title: "Współpraca",
    description: "Zespołowe narzędzia do efektywnej pracy",
  },
];

export const HomeFeatures: React.FC = () => (
  <section className="relative z-10 mx-auto max-w-6xl px-6 pb-20">
    <div className="grid gap-8 md:grid-cols-3">
      {features.map((f, i) => (
        <motion.div
          key={i}
          className="rounded-2xl border border-white/20 bg-white/10 p-6 text-center backdrop-blur-lg"
          initial={{ opacity: 0, y: 40 }}
          whileInView={{ opacity: 1, y: 0 }}
          viewport={{ once: true, amount: 0.5 }}
          transition={{ duration: 0.7, delay: i * 0.18, ease: "easeOut" }}
        >
          {f.icon}
          <h3 className="mb-2 text-xl font-semibold text-white">{f.title}</h3>
          <p className="text-gray-300">{f.description}</p>
        </motion.div>
      ))}
    </div>
  </section>
);
