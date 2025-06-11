import { useEffect, useState } from "react";
import { AuthContext } from "./AuthContext";
import type { AuthContextType, User } from "../../models/account.ts";
import apiAccountConnector from "../../api/apiAccountConnector.ts";

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {
  const [user, setUser] = useState<User | null>(null);
  const [isAuthenticating, setIsAuthenticating] = useState(true);

  useEffect(() => {
    const initializeAuth = async () => {
      const accessToken = localStorage.getItem("ACCESS_TOKEN");
      const accessTokenExpiration = localStorage.getItem(
        "ACCESS_TOKEN_EXPIRATION",
      );

      const currentTime = new Date(Date.now()).getTime();

      const isAccessTokenValid =
        accessToken &&
        accessTokenExpiration &&
        new Date(accessTokenExpiration).getTime() > currentTime;

      if (isAccessTokenValid) {
        await fetchUserData();
        return;
      }

      // Access token missing or expired. Attempting refresh...
      try {
        await loginUsingRefreshToken();
        await fetchUserData();
        setIsAuthenticating(false);
      } catch {
        setUser(null);
      }
    };

    initializeAuth();

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [isAuthenticating]);

  useEffect(() => {
    const handleStorage = () => {
      const accessToken = localStorage.getItem("ACCESS_TOKEN");
      if (!accessToken) setUser(null);
    };
    window.addEventListener("storage", handleStorage);
    return () => window.removeEventListener("storage", handleStorage);
  }, []);

  const fetchUserData = async () => {
    const user = await apiAccountConnector.me();
    setUser(user);
  };

  const loginUsingRefreshToken = async () => {
    await apiAccountConnector.loginUsingRefreshToken();
    await fetchUserData();
  };

  const login = async (
    email: string,
    password: string,
    rememberMe: boolean,
  ): Promise<void> => {
    await apiAccountConnector.login(email, password, rememberMe);
    await fetchUserData();
  };

  const register = async (
    username: string,
    email: string,
    password: string,
  ): Promise<void> => {
    await apiAccountConnector.register(username, email, password);

    // After successful registration, automatically log in the user
    await login(email, password, true);
  };

  const logout = async () => {
    await apiAccountConnector.logout();
    setUser(null);
  };

  const sendResetLink = async (email: string): Promise<void> => {
    await apiAccountConnector.sendResetLink(email);
  };

  const resetPassword = async (
    token: string,
    password: string,
  ): Promise<void> => {
    await apiAccountConnector.resetPassword(token, password);
  };

  const value: AuthContextType = {
    user,
    login,
    register,
    logout,
    sendResetLink,
    resetPassword,
    fetchUserData,
    isAuthenticated: user !== null,
    loginUsingRefreshToken,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};
