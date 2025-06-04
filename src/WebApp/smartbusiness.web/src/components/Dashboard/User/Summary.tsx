import { useAuth } from "../../../context/auth/AuthContext";

export const SummaryComponent: React.FC = () => {
  const { user } = useAuth();

  return (
    <div className="min-w- space-y-4">
      <h2 className="text-2xl font-bold text-gray-800 dark:text-gray-100">
        Witaj, {user?.username || user?.email}!
      </h2>
      <div className="rounded-xl border border-gray-200 bg-gray-50 p-6 shadow-xl dark:border-gray-800 dark:bg-gray-900">
        <div className="mb-2 text-gray-800 dark:text-gray-100">
          Nazwa użytkownika:{" "}
          <span className="font-semibold text-indigo-600 dark:text-indigo-300">
            {user?.username}
          </span>
        </div>
        <div className="mb-2 text-gray-800 dark:text-gray-100">
          Email:{" "}
          <span className="font-semibold text-indigo-600 dark:text-indigo-300">
            {user?.email}
          </span>
        </div>
        <div className="mb-2 text-gray-800 dark:text-gray-100">
          Konto utworzone:{" "}
          <span className="font-semibold text-indigo-600 dark:text-indigo-300">
            {user?.createdAt
              ? new Date(user.createdAt).toLocaleDateString()
              : "-"}
          </span>
        </div>
        <div className="mb-2 text-gray-800 dark:text-gray-100">
          Status:{" "}
          <span className="font-semibold text-green-600 dark:text-green-400">
            Aktywne
          </span>
        </div>
      </div>
    </div>
  );
};
