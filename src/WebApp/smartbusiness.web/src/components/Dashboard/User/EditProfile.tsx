import type { User } from "../../../models";

export const EditProfileComponent: React.FC<{ user: User }> = ({ user }) => (
  <div className="space-y-4">
    <h2 className="text-2xl font-bold text-gray-800 dark:text-gray-100">
      Edycja profilu
    </h2>
    <form className="max-w-md space-y-4 rounded-xl border border-gray-200 bg-gray-100 p-6 shadow-xl dark:border-gray-900 dark:bg-gray-800">
      <div>
        <label htmlFor="username">
          <span className="text-sm font-medium text-gray-800 dark:text-gray-100">
            Nazwa użytkownika
          </span>
          <input
            type="username"
            defaultValue={user?.username}
            className="w-full rounded-md border border-gray-400 bg-white/5 p-2 text-gray-800 placeholder-gray-400 shadow-md focus:border-transparent focus:ring-2 focus:ring-indigo-500 focus:outline-none dark:border-white/20 dark:text-gray-100"
          />
        </label>
      </div>
      <div>
        <label htmlFor="username">
          <span className="text-sm font-medium text-gray-800 dark:text-gray-100">
            Email
          </span>
          <input
            type="email"
            defaultValue={user?.email}
            className="w-full rounded-md border border-gray-400 bg-white/5 p-2 text-gray-800 placeholder-gray-400 shadow-md focus:border-transparent focus:ring-2 focus:ring-indigo-500 focus:outline-none dark:border-white/20 dark:text-gray-100"
          />
        </label>
      </div>
      <button
        type="submit"
        className="rounded bg-indigo-600 px-4 py-2 font-semibold text-gray-100 transition hover:bg-indigo-700"
      >
        Zapisz zmiany
      </button>
    </form>
  </div>
);
