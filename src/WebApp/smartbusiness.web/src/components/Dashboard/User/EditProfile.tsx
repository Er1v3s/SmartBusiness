import type { User } from "../../../models";

export const EditProfileComponent: React.FC<{ user: User }> = ({ user }) => (
  <div className="space-y-4">
    <h2 className="text-2xl font-bold text-gray-800">Edycja profilu</h2>
    {/* Formularz zmiany emaila lub innych danych */}
    <form className="max-w-md space-y-4 rounded-xl bg-white/70 p-6 shadow">
      <div>
        <label className="mb-1 block text-gray-700">Email</label>
        <input
          type="email"
          defaultValue={user?.email}
          className="w-full rounded border px-3 py-2"
        />
      </div>
      <button
        type="submit"
        className="rounded bg-cyan-600 px-4 py-2 font-semibold text-white transition hover:bg-cyan-700"
      >
        Zapisz zmiany
      </button>
    </form>
  </div>
);
