export const ChangePasswordComponent: React.FC = () => (
  <div className="space-y-4">
    <h2 className="text-2xl font-bold text-gray-800 dark:text-gray-100">
      Zmień hasło
    </h2>
    {/* <form className="max-w-md space-y-6 rounded-xl border border-gray-200 bg-gray-100 p-6 shadow-xl dark:border-gray-900 dark:bg-gray-800"> */}
    <form className="space-y-6 rounded-xl border border-gray-200 bg-gray-50 p-6 shadow-xl dark:border-gray-800 dark:bg-gray-900">
      <div>
        <label htmlFor="Password">
          <span className="text-sm font-medium text-gray-800 dark:text-gray-100">
            Stare hasło
          </span>

          <input
            type="password"
            id="oldPassword"
            placeholder="Wprowadź stare hasło"
            className="w-full rounded-md border border-gray-400 bg-white/5 p-2 text-gray-800 placeholder-gray-400 shadow-md focus:border-transparent focus:ring-2 focus:ring-indigo-500 focus:outline-none dark:border-white/20 dark:text-gray-100"
          />
        </label>
      </div>
      <div>
        <label htmlFor="Password">
          <span className="text-sm font-medium text-gray-800 dark:text-gray-100">
            Nowe hasło
          </span>

          <input
            type="password"
            id="newPassword"
            placeholder="Wprowadź nowe hasło"
            className="w-full rounded-lg border border-gray-400 bg-white/5 p-2 text-gray-800 placeholder-gray-400 shadow-md focus:border-transparent focus:ring-2 focus:ring-indigo-500 focus:outline-none dark:border-white/20 dark:text-gray-100"
          />
        </label>
      </div>
      <button
        type="submit"
        className="rounded bg-indigo-600 px-4 py-2 font-semibold text-gray-100 transition hover:bg-indigo-700"
      >
        Zmień hasło
      </button>
    </form>
  </div>
);
