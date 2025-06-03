import type { User } from "../../../models";

export const StatsComponent: React.FC<{ user: User }> = ({ user }) => (
  <div className="space-y-4">
    <h2 className="text-2xl font-bold text-gray-800 dark:text-gray-100">
      Statystyki konta
    </h2>
    <div className="rounded-xl border border-gray-200 bg-gray-100 p-6 shadow-xl dark:border-gray-900 dark:bg-gray-800">
      <div className="mb-2 text-gray-800 dark:text-gray-100">
        Dni od założenia konta:{" "}
        <span className="font-semibold">
          {user?.createdAt
            ? Math.floor(
                (Date.now() - new Date(user.createdAt).getTime()) /
                  (1000 * 60 * 60 * 24),
              )
            : "-"}
        </span>
      </div>
      <div className="mb-2 text-gray-800 dark:text-gray-100">
        Ostatnia aktywność:{" "}
        <span className="font-semibold">
          {/* {user?.lastActive ? new Date(user.lastActive).toLocaleString() : "-"} */}
        </span>
      </div>
    </div>
  </div>
);
