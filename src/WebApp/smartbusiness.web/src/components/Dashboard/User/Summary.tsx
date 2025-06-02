import type { User } from "../../../models";

export const SummaryComponent: React.FC<{ user: User }> = ({ user }) => (
  <div className="space-y-4">
    <h2 className="text-2xl font-bold text-gray-800">
      Witaj, {user?.username || user?.email}!
    </h2>
    <div className="rounded-xl bg-white/70 p-6 shadow">
      <div className="mb-2 text-gray-600">
        Email: <span className="font-semibold">{user?.email}</span>
      </div>
      <div className="mb-2 text-gray-600">
        Konto utworzone:{" "}
        <span className="font-semibold">
          {user?.createdAt
            ? new Date(user.createdAt).toLocaleDateString()
            : "-"}
        </span>
      </div>
      <div className="mb-2 text-gray-600">
        Status: <span className="font-semibold text-green-600">Aktywne</span>
      </div>
    </div>

    <div className="bg-white p-8 text-black dark:bg-black dark:text-white">
      TEST DARK MODE
    </div>
  </div>
);
