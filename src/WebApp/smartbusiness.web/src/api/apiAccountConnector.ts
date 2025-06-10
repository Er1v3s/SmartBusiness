import axiosInstance from "./axiosInstance";
import type { Company, User } from "../models/account";
import type { AxiosResponse } from "axios";
import { removeAccessTokens, setAccessTokens } from "../context/auth/TokenManager";

const apiAccountConnector = {
  
  // AUTHENTICATION
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

    // ACCOUNT
    updateProfile : async (username: string, email: string): Promise<void> => {
        try {
            await axiosInstance.put("/account/update", 
                { username, email }
            );
        } catch (error) {
            return Promise.reject(error);
        }
    },

    changePassword : async (currentPassword: string, newPassword: string): Promise<void> => {
        try {
            await axiosInstance.put("/account/change-password", 
                { currentPassword, newPassword }
            );
        } catch (error) {
            return Promise.reject(error);
        }
    },

    deleteAccount : async (password: string): Promise<void> => {
        try {
            await axiosInstance.request({
                url: "/account/delete",
                method: "DELETE",
                data: { password }
            })
        } catch (error) {
            return Promise.reject(error);
        }
    },

    // COMPANY 
    createCompany : async (name: string): Promise<void> => {
        try {
            await axiosInstance.post("/company", 
                { name }
            );
        } catch (error) {
            return Promise.reject(error);
        }
    },

    getCompany : async (companyId: string): Promise<Company> => {
        try {
            const response: AxiosResponse = await axiosInstance.get(
                `/company/${companyId}`,
            );
            return response.data;
        } catch (error) {
            return Promise.reject(error);
        }
    },

    getCompanies : async (): Promise<Company[]> => {
        try {
            const response: AxiosResponse = await axiosInstance.get(
                `/company`,
            );
            return response.data;
        } catch (error) {
            return Promise.reject(error);
        }
    },

    updateCompany : async (companyId: string, name: string): Promise<void> => {
        try {
            await axiosInstance.put(`/company/${companyId}`, 
                { name }
            );
        } catch (error) {
            return Promise.reject(error);
        }
    },

    deleteCompany : async (companyId: string): Promise<void> => {
        try {
            await axiosInstance.delete(`/company/${companyId}`);
        } catch (error) {
            return Promise.reject(error);
        }
    }
};

export default apiAccountConnector;
