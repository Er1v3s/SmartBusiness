import React, { useState } from "react";
import { useAlert } from "../../../context/alert/useAlert";
import type { ApiResponseError } from "../../../models/authErrors";
import { Mail, UserIcon } from "lucide-react";
import { useAccount } from "../../../context/account/AccountContext";
import { useAuth } from "../../../context/auth/AuthContext";
import { ButtonSuccess } from "../../General/Buttons";

export const EditProfileComponent: React.FC = () => {
  const { user, fetchUserData } = useAuth();
  const { showAlert } = useAlert();
  const { updateAccount } = useAccount();

  const [username, setUsername] = useState(user?.username || "");
  const [email, setEmail] = useState(user?.email || "");
  const [loading, setLoading] = useState(false);

  const [usernameError, setUsernameError] = useState<string | null>(null);
  const [emailError, setEmailError] = useState<string | null>(null);

  // Validation
  const validateUsername = (value: string): string | null => {
    if (value && value.length < 3)
      return "Nazwa użytkownika musi mieć min. 3 znaki.";
    if (value && value.length > 50)
      return "Nazwa użytkownika nie może mieć więcej niż 50 znaków.";
    return null;
  };
  const validateEmail = (value: string): string | null => {
    if (value && value.length < 5) return "Email musi mieć min. 5 znaków.";
    if (value && value.length > 100)
      return "Email nie może mieć więcej niż 100 znaków.";
    const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    if (value && !emailRegex.test(value))
      return "Email musi być w poprawnym formacie (example@example.com).";
    return null;
  };

  const handleUsernameChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setUsername(e.target.value);
    setUsernameError(validateUsername(e.target.value));
  };
  const handleEmailChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setEmail(e.target.value);
    setEmailError(validateEmail(e.target.value));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const usernameErr = validateUsername(username);
    const emailErr = validateEmail(email);
    setUsernameError(usernameErr);
    setEmailError(emailErr);
    if (usernameErr || emailErr) return;
    setLoading(true);
    try {
      await updateAccount(username, email);
      await fetchUserData();
      showAlert({
        title: "Zaktualizowano profil!",
        message: "Zmiany zostały zapisane.",
        type: "success",
      });
    } catch (err) {
      const error = err as ApiResponseError;
      console.log(error);
      showAlert({
        title: error.title,
        message: error.detail,
        type: "error",
      });
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="space-y-4">
      <h2 className="text-2xl font-bold text-indigo-700 dark:text-indigo-200">
        Edycja profilu
      </h2>
      <form
        onSubmit={handleSubmit}
        className="space-y-6 rounded-lg border-2 border-gray-100 bg-white p-6 shadow-xl dark:border-gray-700 dark:bg-gray-800"
      >
        <div>
          <label>
            <span className="text-sm font-medium text-gray-800 dark:text-gray-100">
              Nazwa użytkownika
            </span>
            <div className="relative">
              <UserIcon className="absolute top-1/2 left-3 h-5 w-5 -translate-y-1/2 transform text-gray-400" />
              <input
                id="username"
                type="text"
                value={username}
                onChange={handleUsernameChange}
                className="w-full rounded-lg border border-gray-400 bg-white/5 p-3 pr-10 pl-10 text-gray-800 placeholder-gray-500 shadow-md focus:border-transparent focus:ring-2 focus:ring-indigo-500 focus:outline-none dark:border-white/20 dark:text-gray-100"
                placeholder="nazwa użytkownika"
                required
              />
            </div>
          </label>
          {usernameError && (
            <p className="mt-1 text-xs text-red-600 dark:text-red-400">
              {usernameError}
            </p>
          )}
        </div>

        <div>
          <label>
            <span className="text-sm font-medium text-gray-800 dark:text-gray-100">
              Email
            </span>
            <div className="relative">
              <Mail className="absolute top-1/2 left-3 h-5 w-5 -translate-y-1/2 transform text-gray-400" />
              <input
                id="email"
                type="email"
                value={email}
                onChange={handleEmailChange}
                className="w-full rounded-lg border border-gray-400 bg-white/5 p-3 pr-10 pl-10 text-gray-800 placeholder-gray-500 shadow-md focus:border-transparent focus:ring-2 focus:ring-indigo-500 focus:outline-none dark:border-white/20 dark:text-gray-100"
                placeholder="twoj@email.com"
                required
              />
            </div>
          </label>
          {emailError && (
            <p className="mt-1 text-xs text-red-600 dark:text-red-400">
              {emailError}
            </p>
          )}
        </div>

        <ButtonSuccess
          text={loading ? "Zapisywanie..." : "Zapisz zmiany"}
          type="submit"
          disabled={loading}
        />
      </form>
    </div>
  );
};
