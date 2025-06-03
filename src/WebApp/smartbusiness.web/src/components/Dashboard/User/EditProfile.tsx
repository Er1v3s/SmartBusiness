import type { User } from "../../../models";

export const EditProfileComponent: React.FC<{ user: User }> = ({ user }) => (
  <div className="space-y-4">
    <h2 className="text-2xl font-bold text-gray-800 dark:text-gray-100">
      Edycja profilu
    </h2>
    <form className="space-y-4 rounded-xl border border-gray-200 bg-gray-50 p-6 shadow-xl dark:border-gray-800 dark:bg-gray-900">
      <div>
        <label htmlFor="username">
          <span className="text-sm font-medium text-gray-800 dark:text-gray-100">
            Nazwa użytkownika
          </span>
          <input
            type="username"
            id="username"
            defaultValue={user?.username}
            className="w-full rounded-md border border-gray-400 bg-white/5 p-2 text-gray-800 placeholder-gray-400 shadow-md focus:border-transparent focus:ring-2 focus:ring-indigo-500 focus:outline-none dark:border-white/20 dark:text-gray-100"
            placeholder="Wprowadź nazwę użytkownika"
          />
        </label>
      </div>
      <div>
        <label htmlFor="email">
          <span className="text-sm font-medium text-gray-800 dark:text-gray-100">
            Email
          </span>
          <input
            type="email"
            id="email"
            defaultValue={user?.email}
            className="w-full rounded-md border border-gray-400 bg-white/5 p-2 text-gray-800 placeholder-gray-400 shadow-md focus:border-transparent focus:ring-2 focus:ring-indigo-500 focus:outline-none dark:border-white/20 dark:text-gray-100"
            placeholder="Wprowadź email"
          />
        </label>
      </div>
      <button
        type="submit"
        className="rounded bg-gradient-to-r from-indigo-600 to-indigo-500 px-4 py-2 font-semibold text-white shadow transition hover:from-indigo-700 hover:to-indigo-600 focus:ring-2 focus:ring-indigo-400 focus:outline-none"
      >
        Zapisz zmiany
      </button>
    </form>
  </div>
);
