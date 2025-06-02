import type { User } from "../../../models";

export const StatsComponent: React.FC<{ user: User }> = ({ user }) => (
  <div className="space-y-4">
    <h2 className="text-2xl font-bold text-gray-800">Statystyki konta</h2>
    <div className="space-y-2 rounded-xl bg-white/70 p-6 shadow">
      <div className="text-gray-600">
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
      <div className="text-gray-600">
        Ostatnia aktywność:{" "}
        <span className="font-semibold">
          {/* {user?.lastActive ? new Date(user.lastActive).toLocaleString() : "-"} */}
        </span>
      </div>
      {/* Możesz dodać więcej statystyk */}
    </div>
  </div>
);
