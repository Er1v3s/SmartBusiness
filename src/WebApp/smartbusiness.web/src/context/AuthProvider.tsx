import { useEffect, useState } from "react";
import { AuthContext } from "./AuthContext";
import type { AuthContextType, User } from "../models";
import apiConnector from "../api/apiConnector.ts";

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

  const fetchUserData = async () => {
    const user = await apiConnector.me();
    setUser(user);
  };

  const loginUsingRefreshToken = async () => {
    await apiConnector.loginUsingRefreshToken();
    await fetchUserData();
  };

  // Login - we send the data, backend saves the token in cookies
  const login = async (email: string, password: string): Promise<void> => {
    await apiConnector.login(email, password);
    await fetchUserData();
  };

  // Registration - we send the data, backend saves the token in cookies
  const register = async (
    username: string,
    email: string,
    password: string,
  ): Promise<void> => {
    await apiConnector.register(username, email, password);

    // After successful registration, automatically log in the user
    await login(email, password);
  };

  // Logout - we remove the session on the backend
  const logout = async () => {
    await apiConnector.logout();
    setUser(null);
  };

  // Context value
  const value: AuthContextType = {
    user,
    login,
    register,
    logout,
    isAuthenticated: user !== null,
    // token: null,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};
