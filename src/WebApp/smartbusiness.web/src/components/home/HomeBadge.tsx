import React from "react";

interface HomeBadgeProps {
  label: string;
}

export const HomeBadge: React.FC<HomeBadgeProps> = ({ label }) => (
  <span className="inline-block rounded bg-white/10 px-3 py-1 text-sm font-medium text-white/80">
    {label}
  </span>
);
