export const ChangePasswordComponent: React.FC = () => (
  <div className="space-y-4">
    <h2 className="text-2xl font-bold text-gray-800">Zmień hasło</h2>
    <form className="max-w-md space-y-4 rounded-xl bg-white/70 p-6 shadow">
      <div>
        <label className="mb-1 block text-gray-700">Stare hasło</label>
        <input type="password" className="w-full rounded border px-3 py-2" />
      </div>
      <div>
        <label className="mb-1 block text-gray-700">Nowe hasło</label>
        <input type="password" className="w-full rounded border px-3 py-2" />
      </div>
      <button
        type="submit"
        className="rounded bg-cyan-600 px-4 py-2 font-semibold text-white transition hover:bg-cyan-700"
      >
        Zmień hasło
      </button>
    </form>
  </div>
);
