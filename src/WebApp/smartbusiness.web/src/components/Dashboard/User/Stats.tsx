import { useAuth } from "../../../context/auth/AuthContext";

export const StatsComponent: React.FC = () => {
  const { user } = useAuth();

  return (
    <div className="space-y-4">
      <h2 className="text-2xl font-bold text-gray-800 dark:text-gray-100">
        Statystyki konta
      </h2>
      <div className="ounded-lg border-2 border-gray-100 bg-white p-6 shadow-xl dark:border-gray-700 dark:bg-gray-800">
        <div className="mb-2 text-gray-800 dark:text-gray-100">
          Data założenia konta:{" "}
          <span className="font-semibold text-indigo-600 dark:text-indigo-300">
            {user?.createdAt
              ? Math.floor(
                  (Date.now() - new Date(user.createdAt).getTime()) /
                    (1000 * 60 * 60 * 24),
                )
              : "-"}
          </span>
        </div>
        <div className="mb-2 text-gray-800 dark:text-gray-100">
          Coś innego, nie wiem jeszcze co:{" "}
          <span className="font-semibold text-indigo-600 dark:text-indigo-300">
            {/* {user?.lastActive ? new Date(user.lastActive).toLocaleString() : "-"} */}
          </span>
        </div>
      </div>
    </div>
  );
};
