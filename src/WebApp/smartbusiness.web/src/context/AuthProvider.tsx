import { useState, useEffect } from "react";
import { AuthContext } from "./AuthContext";
import type { AuthContextType, User } from "../models";

// Auth Provider
export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {
  const [user, setUser] = useState<User | null>(null);
  const [token, setToken] = useState<string | null>(null);

  useEffect(() => {
    // Check for stored token on app load
    const mockToken = "stored-token";
    const mockUser = {
      id: "1",
      username: "demo_user",
      email: "user@example.com",
    };
    // Simulate stored auth state for demo
    if (Math.random() > 0.7) {
      setToken(mockToken);
      setUser(mockUser);
    }
  }, []);

  const login = async (
    email: string,
    password: string,
    rememberMe: boolean,
  ): Promise<void> => {
    // Simulate API call delay
    await new Promise((resolve) => setTimeout(resolve, 1000));

    // For demo purposes, simulate successful login
    const mockToken = "mock-jwt-token-" + Date.now();
    const mockUser = {
      id: "1",
      username: email.split("@")[0],
      email: email,
      password: password, // Not recommended to store passwords like this
      rememberMe: rememberMe,
    };

    setToken(mockToken);
    setUser(mockUser);
  };

  const register = async (
    username: string,
    email: string,
    password: string,
  ): Promise<void> => {
    // Simulate API call delay
    await new Promise((resolve) => setTimeout(resolve, 1000));

    // For demo purposes, simulate successful registration
    const mockToken = "mock-jwt-token-" + Date.now();
    const mockUser = {
      id: "2",
      username: username,
      email: email,
      password: password, // Not recommended to store passwords like this
      rememberMe: false,
    };

    setToken(mockToken);
    setUser(mockUser);
  };

  const logout = () => {
    setUser(null);
    setToken(null);
  };

  const value: AuthContextType = {
    user,
    token,
    login,
    register,
    logout,
    isAuthenticated: !!token && !!user,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};
