import React, { useState } from "react";
// import { Link } from "react-router-dom";
import { useNavigate } from "react-router-dom";

export const LoginPage: React.FC = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [rememberMe, setRememberMe] = useState(false);

  const handleLogin = (e: React.FormEvent) => {
    e.preventDefault();
    // Handle login logic here

    console.log("Email:", email);
    console.log("Password:", password);
    console.log("Remember Me:", rememberMe);
  };

  const handleRememberMeChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setRememberMe(e.target.checked);
  };

  const handleEmailChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setEmail(e.target.value);
  };

  const handlePasswordChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setPassword(e.target.value);
  };

  const navigate = useNavigate();

  const handleRegisterRedirect = () => {
    navigate("/register");
  };

  return (
    <div className="flex w-full items-center justify-center bg-gray-100 sm:w-screen sm:max-w-lg dark:bg-gray-900">
      <div className="w-full rounded-lg bg-white p-12 shadow-md dark:bg-gray-800">
        <h2 className="mb-4 text-center text-3xl font-bold text-gray-700 dark:text-gray-200">
          Login
        </h2>

        <form onSubmit={handleLogin}>
          <div className="mb-4">
            <input
              type="email"
              id="email"
              placeholder="email"
              className="w-full rounded-lg border border-gray-300 p-2 focus:border-blue-500 focus:outline-none dark:border-gray-600 dark:bg-gray-700 dark:text-gray-200"
              required
              value={email}
              onChange={handleEmailChange}
            />
          </div>

          <div className="mb-4">
            <input
              type="password"
              id="password"
              className="w-full rounded-lg border border-gray-300 p-2 focus:border-blue-500 focus:outline-none dark:border-gray-600 dark:bg-gray-700 dark:text-gray-200"
              placeholder="password"
              required
              value={password}
              onChange={handlePasswordChange}
            />
          </div>

          <button
            type="submit"
            className="w-full rounded-lg bg-blue-500 py-2 text-white hover:bg-blue-600 focus:ring focus:ring-blue-300 focus:outline-none dark:bg-blue-600 dark:hover:bg-blue-700"
          >
            Login
          </button>
        </form>

        <div className="flex-1 items-center">
          <div className="w-full-1 mt-4 flex-1 items-center justify-between sm:flex">
            <div className="flex items-center justify-center sm:flex-1 sm:justify-start">
              <input
                type="checkbox"
                id="remember"
                className="mr-2 h-4 w-4 rounded border-gray-300 text-blue-600 focus:ring-blue-500 dark:border-gray-600 dark:bg-gray-700 dark:ring-offset-gray-800"
                checked={rememberMe}
                onChange={handleRememberMeChange}
              />

              <label
                htmlFor="remember"
                className="text-sm text-gray-600 dark:text-gray-400"
              >
                Remember me
              </label>
            </div>

            <a
              href="#"
              className="text-sm text-blue-500 hover:underline dark:text-blue-400"
            >
              Forgot password?
            </a>
          </div>
        </div>

        <span className="mt-4 flex items-center">
          <span className="h-px flex-1 bg-gradient-to-r from-transparent to-gray-300 dark:to-gray-600"></span>
          <span className="h-px flex-1 bg-gradient-to-l from-transparent to-gray-300 dark:to-gray-600"></span>
        </span>

        <div className="mt-4 text-center">
          <label className="text-sm text-gray-600 dark:text-gray-400">
            Don't have an account?{" "}
            <a
              onClick={handleRegisterRedirect}
              className="cursor-pointer text-blue-500 dark:text-blue-400"
            >
              Sign up
            </a>
          </label>
        </div>
      </div>
    </div>
  );
};
