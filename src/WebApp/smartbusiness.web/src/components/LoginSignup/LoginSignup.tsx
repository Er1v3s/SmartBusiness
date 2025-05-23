import "./LoginSignup.css";

export const LoginSignup = () => {
  return (
    <div className="flex w-full items-center justify-center bg-gray-100 md:w-screen md:max-w-lg dark:bg-gray-900">
      <div className="w-full rounded-lg bg-white p-12 shadow-md dark:bg-gray-800">
        <h2 className="mb-4 text-center text-3xl font-bold text-gray-700 dark:text-gray-200">
          Login
        </h2>

        <form>
          <div className="mb-4">
            <input
              type="email"
              id="email"
              className="w-full rounded-lg border border-gray-300 p-2 focus:border-blue-500 focus:outline-none dark:border-gray-600 dark:bg-gray-700 dark:text-gray-200"
              placeholder="Enter your email"
              required
            />
          </div>

          <div className="mb-4">
            <input
              type="password"
              id="password"
              className="w-full rounded-lg border border-gray-300 p-2 focus:border-blue-500 focus:outline-none dark:border-gray-600 dark:bg-gray-700 dark:text-gray-200"
              placeholder="Enter your password"
              required
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
          <div className="w-full-1 mt-4 flex-1 items-center justify-between">
            <div className="flex flex-1 items-center justify-center">
              <input
                type="checkbox"
                id="remember"
                className="mr-2 h-4 w-4 rounded border-gray-300 text-blue-600 focus:ring-blue-500 dark:border-gray-600 dark:bg-gray-700 dark:ring-offset-gray-800"
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
          <p className="text-sm text-gray-600 dark:text-gray-400">
            Don't have an account?{" "}
            <a
              href="#"
              className="text-blue-500 hover:underline dark:text-blue-400"
            >
              Sign up
            </a>
          </p>
        </div>
      </div>
    </div>
  );
};
