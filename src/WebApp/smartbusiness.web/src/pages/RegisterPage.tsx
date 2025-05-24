import React, { useState } from "react";
// import { Link } from "react-router-dom";
import { useNavigate } from "react-router-dom";

export const RegisterPage: React.FC = () => {
  const [username, setUsername] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const handleRegister = (e: React.FormEvent) => {
    e.preventDefault();
    // Handle registration logic here

    console.log("Username:", username);
    console.log("Email:", email);
    console.log("Password:", password);
  };

  const handleUsernameChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setUsername(e.target.value);
  };

  const handleEmailChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setEmail(e.target.value);
  };

  const handlePasswordChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setPassword(e.target.value);
  };

  const navigate = useNavigate();

  const handleLoginRedirect = () => {
    navigate("/login");
  };

  return (
    <div className="flex w-full items-center justify-center bg-gray-100 sm:w-screen sm:max-w-lg dark:bg-gray-900">
      <div className="w-full rounded-lg bg-white p-12 shadow-md dark:bg-gray-800">
        <h2 className="mb-4 text-center text-3xl font-bold text-gray-700 dark:text-gray-200">
          Register
        </h2>

        <form onSubmit={handleRegister}>
          <div className="mb-4">
            <input
              type="text"
              id="username"
              placeholder="Username"
              className="w-full rounded-lg border border-gray-300 p-2 focus:border-blue-500 focus:outline-none dark:border-gray-600 dark:bg-gray-700 dark:text-gray-200"
              required
              value={username}
              onChange={handleUsernameChange}
            />
          </div>

          <div className="mb-4">
            <input
              type="email"
              id="email"
              placeholder="Email"
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
              placeholder="Password"
              required
              value={password}
              onChange={handlePasswordChange}
            />
          </div>

          <button
            type="submit"
            className="w-full rounded-lg bg-blue-500 py-2 text-white hover:bg-blue-600 focus:ring focus:ring-blue-300 focus:outline-none dark:bg-blue-600 dark:hover:bg-blue-700"
          >
            Register
          </button>
        </form>

        <span className="mt-4 flex items-center">
          <span className="h-px flex-1 bg-gradient-to-r from-transparent to-gray-300 dark:to-gray-600"></span>
          <span className="h-px flex-1 bg-gradient-to-l from-transparent to-gray-300 dark:to-gray-600"></span>
        </span>

        <div className="mt-4 text-center">
          <label className="text-sm text-gray-600 dark:text-gray-400">
            Already have an account?{" "}
            <a
              onClick={handleLoginRedirect}
              className="cursor-pointer text-blue-500 hover:underline dark:text-blue-400"
            >
              Log in
            </a>
          </label>
        </div>
      </div>
    </div>
  );
};
