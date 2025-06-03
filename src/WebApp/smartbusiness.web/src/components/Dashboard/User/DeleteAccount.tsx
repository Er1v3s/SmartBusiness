export const DeleteAccountComponent: React.FC = () => (
  <div className="space-y-4">
    <h2 className="text-2xl font-bold text-gray-800 dark:text-gray-100">
      Usuń konto
    </h2>
    <div className="rounded-xl border border-gray-200 bg-gray-50 p-6 shadow-xl dark:border-gray-800 dark:bg-gray-900">
      <p className="mb-4 text-gray-800 dark:text-gray-100">
        Usunięcie konta jest nieodwracalne. Wszystkie Twoje dane zostaną trwale
        usunięte.
      </p>
      <button className="rounded bg-gradient-to-r from-red-700 to-red-600 px-4 py-2 font-semibold text-white shadow transition hover:from-red-800 hover:to-red-700 focus:ring-2 focus:ring-red-400 focus:outline-none">
        Usuń konto
      </button>
    </div>
  </div>
);
