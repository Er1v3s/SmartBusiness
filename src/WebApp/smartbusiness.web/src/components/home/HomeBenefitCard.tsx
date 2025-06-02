import React from "react";

interface HomeBenefitCardProps {
  icon: React.ReactNode;
  title: string;
  description: string;
}

export const HomeBenefitCard: React.FC<HomeBenefitCardProps> = ({
  icon,
  title,
  description,
}) => (
  <div className="flex items-start gap-3 rounded-xl border border-white/10 bg-white/10 p-4 shadow-sm">
    {icon}
    <div>
      <div className="text-lg font-bold text-white/90">{title}</div>
      <div className="text-sm text-white/60">{description}</div>
    </div>
  </div>
);
