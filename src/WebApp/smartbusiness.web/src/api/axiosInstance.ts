import axios, { AxiosError } from "axios";
import { API_BASE_URL } from "../../config.ts";
import { removeAccessTokens, setAccessTokens } from "../context/auth/TokenManager.ts";
import type { ApiResponseError, ApiResponseValidationError } from "../models/authErrors.ts";

const axiosInstance = axios.create({
  baseURL: API_BASE_URL,
  withCredentials: true,
});

let isInterceptorSetup = false;
let isRefreshing = false;
let refreshSubscribers: Array<(token: string) => void> = [];

const setupResponseInterceptor = () => {
  if (isInterceptorSetup) return;

  // Interceptor to handle token refresh logic
  // Interceptor to handle token refresh logic
  axiosInstance.interceptors.request.use(
    async (config) => {
      const token = localStorage.getItem("ACCESS_TOKEN");
      const expirationDate = localStorage.getItem("ACCESS_TOKEN_EXPIRATION");
      const currentTime = new Date(Date.now()).getTime();

      // Check if the token exists and if it has expired
      if (
        token &&
        expirationDate &&
        new Date(expirationDate).getTime() < currentTime 
      ) {
        // If refreshing is in progress, add the request to the queue
        if (isRefreshing) {
          return new Promise((resolve) => {
            refreshSubscribers.push((newToken) => {
              config.headers.Authorization = `Bearer ${newToken}`;
              resolve(config);
            });
          });
        }

        // Request a new token using the refresh token
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
          return Promise.reject(error);
          
        } finally {
          isRefreshing = false;
        }
      } else if (token) {
        // Token is valid, set the header
        config.headers.Authorization = `Bearer ${token}`;
      }
      else {
        // No token available, do not set Authorization header
        delete config.headers.Authorization;
      }

      return config;
    },
    (error) => Promise.reject(error),
  );

  isInterceptorSetup = true;
};

// Interceptor to handle API response errors
axiosInstance.interceptors.response.use(
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

setupResponseInterceptor();

export default axiosInstance;