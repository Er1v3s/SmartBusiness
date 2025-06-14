import type { SalesPredictionResponse } from "../models/prediction";
import { axiosPrediction } from "./axiosInstance";
import type { AxiosResponse } from "axios";

const apiMLConnector = {
  
  // PREDICTIONS
    getSalesPrediction : async (monthsAhead: number = 1): Promise<SalesPredictionResponse> => {
        const response: AxiosResponse<SalesPredictionResponse> = await axiosPrediction.get(
            "/predict/sales",
            { params: { months_ahead: monthsAhead } }
        );

        return response.data;
    },

    getTaxPrediction : async (monthsAhead: number = 1): Promise<SalesPredictionResponse> => {
        const response: AxiosResponse<SalesPredictionResponse> = await axiosPrediction.get(
            "/predict/tax",
            { params: { months_ahead: monthsAhead } }
        );

        return response.data;
    },

    getNetSalesPrediction : async (monthsAhead: number = 1): Promise<SalesPredictionResponse> => {
        const response: AxiosResponse<SalesPredictionResponse> = await axiosPrediction.get(
            "/predict/net-sales",
            { params: { months_ahead: monthsAhead } }
        );

        return response.data;
    },

    getServiceSalesPrediction : async (itemId: string, monthsAhead: number = 1): Promise<SalesPredictionResponse> => {
        const response: AxiosResponse<SalesPredictionResponse> = await axiosPrediction.get(
            "/predict/product-sales",
            { params: { item_id: itemId, months_ahead: monthsAhead } }
        );

        return response.data;
    },
};

export default apiMLConnector;
