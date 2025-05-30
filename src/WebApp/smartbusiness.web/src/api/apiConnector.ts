import type { AxiosResponse } from "axios";
import axiosInstance from "./axiosInstance.ts";
import type { User } from "../models/index.ts";
import { removeAccessTokens, setAccessTokens } from "../context/TokenManager.ts";

const apiConnector = {

    login : async (email: string, password: string, rememberMe: boolean): Promise<void> => {
        const response: AxiosResponse = await axiosInstance.post(
            "/auth/login",
            { email, password, rememberMe },
        );

        const { jwtToken, expirationDateInUtc } = response.data;
        setAccessTokens(jwtToken, expirationDateInUtc);
    },

    register : async (username: string, email: string, password: string): Promise<void> => {
        await axiosInstance.post(
            "/auth/register",
            { username, email, password },
        );
    },

    loginUsingRefreshToken : async (): Promise<void> => {
        try {
            const response: AxiosResponse = await axiosInstance.get(
                "/auth/refresh",
            );

            const { jwtToken, expirationDateInUtc } = response.data;
            setAccessTokens(jwtToken, expirationDateInUtc);

        } catch {
            return Promise.reject();
        }
    },
    
    logout : async (): Promise<void> => {
        try {
            await axiosInstance.post("/auth/logout",
            );

            removeAccessTokens();
        } catch {
            return Promise.reject();
        }
    },

    me : async (): Promise<User | null> => {
        try {
            const response: AxiosResponse = await axiosInstance.get(
                "/account/me", 
                { withCredentials: true },
            );

            const { id, username, email } = response.data;

            const user: User = {
                id: id,
                username: username,
                email: email
            };

            return user;
        } catch {
            return null;
        }
    },

    sendResetLink : async (email: string): Promise<void> => {
        try {
            await axiosInstance.post("/auth/forgot-password", 
                { email });
        } catch (error) {
            return Promise.reject(error);
        }
    },

    resetPassword : async (token: string, newPassword: string): Promise<void> => {
        try {
            await axiosInstance.post("/auth/reset-password", 
                { token, newPassword }
            );
        } catch (error) {
            return Promise.reject(error);
        }
    },
}

export default apiConnector;