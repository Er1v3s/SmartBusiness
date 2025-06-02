import React from "react";
import logo_white from "../../assets/logo_white.svg";
import { HomeBadge } from "../Home/HomeBadge";
import { HomeBenefitsGrid } from "../Home/HomeBenefitsGrid";
import { motion } from "framer-motion";

export const HomeAboutSection: React.FC = () => (
  <section className="relative z-10 mx-auto max-w-6xl px-4 pb-15">
    <motion.div
      className="flex flex-col overflow-hidden rounded-3xl border border-white/15 bg-white/5 shadow-2xl backdrop-blur-lg md:flex-row"
      initial={{ opacity: 0, scale: 0.97 }}
      whileInView={{ opacity: 1, scale: 1 }}
      viewport={{ once: true, amount: 0.3 }}
      transition={{ duration: 0.7, ease: "easeOut" }}
    >
      {/* LEFT (Logo) */}
      <div className="flex w-full items-center justify-center bg-white/10 py-10 md:min-h-[500px] md:w-1/3 md:py-0">
        <img
          src={logo_white}
          alt="SmartBusiness logo"
          className="h-24 w-24 object-contain drop-shadow-2xl sm:h-36 sm:w-36 md:h-100 md:w-100"
        />
      </div>
      {/* RIGHT (content) */}
      <div className="flex flex-1 flex-col justify-center px-6 py-10 md:px-12 md:py-16">
        <motion.h2
          className="mb-4 text-center text-4xl font-extrabold tracking-tight text-white md:text-left"
          initial={{ opacity: 0, y: -30 }}
          whileInView={{ opacity: 1, y: 0 }}
          viewport={{ once: true, amount: 0.5 }}
          transition={{ duration: 0.5, delay: 0.2 }}
        >
          O projekcie
        </motion.h2>
        <motion.p
          className="mx-auto mb-4 max-w-xl text-center text-2xl font-light text-white/90 md:mx-0 md:text-left"
          initial={{ opacity: 0, y: -20 }}
          whileInView={{ opacity: 1, y: 0 }}
          viewport={{ once: true, amount: 0.5 }}
          transition={{ duration: 0.5, delay: 0.35 }}
        >
          SmartBusiness to platforma, która daje Twojej firmie przewagę:
          bezpieczeństwo, inteligencję i wygodę w jednym miejscu. Zyskaj spokój,
          oszczędność czasu i realny wzrost zysków!
        </motion.p>
        <motion.div
          className="mb-8 flex flex-wrap justify-center gap-3 md:justify-start"
          initial={{ opacity: 0, y: -10 }}
          whileInView={{ opacity: 1, y: 0 }}
          viewport={{ once: true, amount: 0.5 }}
          transition={{ duration: 0.5, delay: 0.5 }}
        >
          <HomeBadge label="Bezpieczeństwo" />
          <HomeBadge label="Automatyzacja" />
          <HomeBadge label="Wygoda" />
          <HomeBadge label="Inteligencja" />
          <HomeBadge label="Dostępność 24/7" />
        </motion.div>
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          whileInView={{ opacity: 1, y: 0 }}
          viewport={{ once: true, amount: 0.5 }}
          transition={{ duration: 0.6, delay: 0.7 }}
        >
          <HomeBenefitsGrid />
        </motion.div>
      </div>
    </motion.div>
  </section>
);
