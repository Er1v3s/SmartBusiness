export const ChangePasswordComponent: React.FC = () => (
  <div className="space-y-4">
    <h2 className="text-2xl font-bold text-gray-800 dark:text-gray-100">
      Zmień hasło
    </h2>
    {/* <div className=""> */}
    <form className="max-w-md space-y-4 rounded-xl border border-gray-200 bg-gray-100 p-6 shadow-xl dark:border-gray-900 dark:bg-gray-800">
      <div>
        <label className="mb-1 block text-gray-800 dark:text-gray-100">
          Stare hasło
        </label>
        <input type="password" className="w-full rounded border px-3 py-2" />
      </div>
      <div>
        <label className="mb-1 block text-gray-800 dark:text-gray-100">
          Nowe hasło
        </label>
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
