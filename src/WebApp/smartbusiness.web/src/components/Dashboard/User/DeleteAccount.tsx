export const DeleteAccountComponent: React.FC = () => (
  <div className="space-y-4">
    <h2 className="text-2xl font-bold text-gray-800 dark:text-gray-100">
      Usuń konto
    </h2>
    <div className="rounded-xl border border-gray-200 bg-gray-100 p-6 shadow-xl dark:border-gray-900 dark:bg-gray-800">
      <p className="mb-4 text-gray-800 dark:text-gray-100">
        Usunięcie konta jest nieodwracalne. Wszystkie Twoje dane zostaną trwale
        usunięte.
      </p>
      <button className="rounded bg-red-700 px-4 py-2 font-semibold text-white transition hover:bg-red-800">
        Usuń konto
      </button>
    </div>
  </div>
);
