import axios, { AxiosError } from "axios";
import { API_BASE_URL } from "../../config.ts";
import { removeAccessTokens, setAccessTokens } from "../context/auth/TokenManager.ts";
import type { ApiResponseError, ApiResponseValidationError } from "../models/authErrors.ts";

// AccountService, SalesService, WriteService, ReadService
export const axiosAccount = axios.create({ baseURL: API_BASE_URL, withCredentials: true });
export const axiosSales = axios.create({ baseURL: API_BASE_URL, withCredentials: true });
export const axiosWrite = axios.create({ baseURL: API_BASE_URL, withCredentials: true });
export const axiosRead = axios.create({ baseURL: API_BASE_URL, withCredentials: true });

let isRefreshing = false;
let refreshSubscribers: Array<(token: string) => void> = [];

const getCompanyId = () => localStorage.getItem("COMPANY_ID");

// eslint-disable-next-line @typescript-eslint/no-explicit-any
const addCompanyIdHeader = (config: any) => {
  const companyId = getCompanyId();
  if (companyId) config.headers["X-Company-Id"] = companyId;
  return config;
};

const setupInterceptors = (instance: typeof axiosAccount, withCompanyId: boolean) => {
  instance.interceptors.request.use(
    async (config) => {
      const token = localStorage.getItem("ACCESS_TOKEN");
      const expirationDate = localStorage.getItem("ACCESS_TOKEN_EXPIRATION");
      const currentTime = new Date().getTime();

      if (
        token &&
        expirationDate &&
        new Date(expirationDate).getTime() <= currentTime
      ) 
      {
        if (isRefreshing) {
          return new Promise((resolve) => {
            refreshSubscribers.push((newToken) => {
              config.headers.Authorization = `Bearer ${newToken}`;
              resolve(config);
            });
          });
        }

        isRefreshing = true;

        try {
          const response = await axios.post(`${API_BASE_URL}/auth/refresh`, {
            withCredentials: true,
          });

          const { newToken, newExpirationDate } = response.data;
          setAccessTokens(newToken, newExpirationDate);
          config.headers.Authorization = `Bearer ${newToken}`;
          refreshSubscribers.forEach((callback) => callback(newToken));
          refreshSubscribers = [];

          return config;
        } catch (error) {
          removeAccessTokens();
          refreshSubscribers = [];

          // Force redirect to home page on refresh failure
          window.location.href = "/home";
          return Promise.reject(error);
        } finally {
          isRefreshing = false;
        }
      } else if (token) {
        config.headers.Authorization = `Bearer ${token}`;
      } else {
        delete config.headers.Authorization;
      }

      if (withCompanyId){
        addCompanyIdHeader(config);
      } 

      return config;
    },
    (error) => Promise.reject(error),
  );

  instance.interceptors.response.use(
    (response) => response,
    async (err) => {
      const error = err as AxiosError<ApiResponseError>;
      // TO DELETE AFTER DEVELOPMENT
      console.log("API Error:", error);
      // TO DELETE AFTER DEVELOPMENT
      const fallbackError: ApiResponseError = {
        title: error.response?.data?.title || "Błąd serwera",
        status: error.response?.data?.status || "500",
        detail: error.response?.data?.detail || "Wystąpił błąd podczas przetwarzania żądania. Spróbuj ponownie później.",
        errors: Array.isArray(error.response?.data.errors)
          ? error.response?.data.errors.map((e: ApiResponseValidationError) => ({
              property: e.property ?? "unknown",
              errorMessage: e.errorMessage ?? "Validation error",
            })) : null,
      };

      return Promise.reject(fallbackError);
    },
  );
};

// AccountService does not require X-Company-Id
setupInterceptors(axiosAccount, false);

// SalesService, WriteService, ReadService need X-Company-Id to be set, becase each of these operations is on company context
setupInterceptors(axiosSales, true);
setupInterceptors(axiosWrite, true);
setupInterceptors(axiosRead, true);

export default axiosAccount;