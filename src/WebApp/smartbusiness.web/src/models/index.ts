// Types
export interface User {
  id: string;
  username: string;
  email: string;
  createdAt?: string; // Optional, can be used for user creation date
}

export interface AuthContextType {
  user: User | null;
  login: (email: string, password: string, rememberMe: boolean) => Promise<void>;
  register: (username: string, email: string, password: string) => Promise<void>;
  fetchUserData: () => Promise<void>;
  logout: () => void;
  sendResetLink: (email: string) => Promise<void>;
  resetPassword: (token: string, password: string) => Promise<void>;
  isAuthenticated: boolean;
  // token: string | null;
}

export interface AccountContextType {
  updateAccount: (username: string, email: string) => Promise<void>;
  changePassword: (currentPassword: string, newPassword: string) => Promise<void>;
  deleteAccount: (password: string) => Promise<void>;
}

export interface LoginForm {
  email: string;
  password: string;
  rememberMe: boolean;
}

export interface RegisterForm {
  username: string;
  email: string;
  password: string;
}