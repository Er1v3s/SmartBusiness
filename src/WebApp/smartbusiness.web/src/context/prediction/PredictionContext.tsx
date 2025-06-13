import { createContext, useContext } from "react";
import type { PredictionContextType } from "../../models/prediction.ts";

export const PredictionContext = createContext<
  PredictionContextType | undefined
>(undefined);

export const usePrediction = () => {
  const context = useContext(PredictionContext);
  if (context === undefined) {
    throw new Error("usePrediction must be used within a ServiceProvider");
  }
  return context;
};
