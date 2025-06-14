import { PredictionContext } from "./PredictionContext.tsx";
import type {
  PredictionContextType,
  SalesPredictionResponse,
} from "../../models/prediction.ts";
import apiMLConnector from "../../api/apiMLConnector.ts";

export const PredictionProvider: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {
  const fetchSalesPrediction = async (
    monthsAhead?: number,
  ): Promise<SalesPredictionResponse> => {
    return await apiMLConnector.getSalesPrediction(monthsAhead);
  };

  const fetchTaxPrediction = async (
    monthsAhead?: number,
  ): Promise<SalesPredictionResponse> => {
    return await apiMLConnector.getTaxPrediction(monthsAhead);
  };

  const fetchNetSalesPrediction = async (
    monthsAhead?: number,
  ): Promise<SalesPredictionResponse> => {
    return await apiMLConnector.getNetSalesPrediction(monthsAhead);
  };

  const fetchServiceSalesPrediction = async (
    serviceId: string,
    monthsAhead: number,
  ): Promise<SalesPredictionResponse> => {
    return await apiMLConnector.getServiceSalesPrediction(
      serviceId,
      monthsAhead,
    );
  };

  // Context value
  const value: PredictionContextType = {
    fetchSalesPrediction,
    fetchTaxPrediction,
    fetchNetSalesPrediction,
    fetchServiceSalesPrediction,
  };

  return (
    <PredictionContext.Provider value={value}>
      {children}
    </PredictionContext.Provider>
  );
};
