import type { User } from "../../../models";

export const SummaryComponent: React.FC<{ user: User }> = ({ user }) => (
  <div className="space-y-4">
    <h2 className="text-2xl font-bold text-gray-800 dark:text-gray-100">
      Witaj, {user?.username || user?.email}!
    </h2>
    <div className="rounded-xl border border-gray-200 bg-gray-100 p-6 shadow-xl dark:border-gray-900 dark:bg-gray-800">
      <div className="mb-2 text-gray-800 dark:text-gray-100">
        Email: <span className="font-semibold">{user?.email}</span>
      </div>
      <div className="mb-2 text-gray-800 dark:text-gray-100">
        Konto utworzone:{" "}
        <span className="font-semibold">
          {user?.createdAt
            ? new Date(user.createdAt).toLocaleDateString()
            : "-"}
        </span>
      </div>
      <div className="mb-2 text-gray-800 dark:text-gray-100">
        Status: <span className="font-semibold text-green-600">Aktywne</span>
      </div>
    </div>
  </div>
);
