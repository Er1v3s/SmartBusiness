import type { AxiosResponse } from "axios";
import axiosInstance from "./axiosInstance.ts";
import type { User } from "../models/index.ts";
import { removeAccessTokens, setAccessTokens } from "../context/TokenManager.ts";

const apiConnector = {

    login : async (email: string, password: string): Promise<void> => {
        try {
            const response: AxiosResponse = await axiosInstance.post(
                "/auth/login",
                { email, password },
                { withCredentials: true },
            );

            const { jwtToken, expirationDateInUtc } = response.data;
            setAccessTokens(jwtToken, expirationDateInUtc);
        } catch {
            return Promise.reject();
        }
    },

    register : async (username: string, email: string, password: string): Promise<void> => {
        try {
            await axiosInstance.post(
                "/auth/register",
                { username, email, password },
                { withCredentials: true }
            );
        } catch {
            return Promise.reject();
        }
    },

    loginUsingRefreshToken : async (): Promise<void> => {
        try {
            const response: AxiosResponse = await axiosInstance.get(
                "/auth/refresh",
                { withCredentials: true },
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
                {},
                { withCredentials: true },
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
    }
}

export default apiConnector;