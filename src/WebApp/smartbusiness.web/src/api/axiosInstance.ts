import axios from "axios";
import { API_BASE_URL } from "../../config.ts";
import { removeAccessTokens, setAccessTokens } from "../context/TokenManager.ts";

const axiosInstance = axios.create({
  baseURL: API_BASE_URL,
});

// Interceptor to handle token refresh logic
let isInterceptorSetup = false;
let isRefreshing = false;
let refreshSubscribers: Array<(token: string) => void> = [];

const setupResponseInterceptor = () => {
  if (isInterceptorSetup) return;

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



setupResponseInterceptor();

export default axiosInstance;