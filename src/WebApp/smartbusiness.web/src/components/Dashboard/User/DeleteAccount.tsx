export const DeleteAccountComponent: React.FC = () => (
  <div className="space-y-4">
    <h2 className="text-2xl font-bold text-red-700">Usuń konto</h2>
    <div className="rounded-xl bg-white/70 p-6 shadow">
      <p className="mb-4 text-gray-700">
        Usunięcie konta jest nieodwracalne. Wszystkie Twoje dane zostaną trwale
        usunięte.
      </p>
      <button className="rounded bg-red-600 px-4 py-2 font-semibold text-white transition hover:bg-red-700">
        Usuń konto
      </button>
    </div>
  </div>
);
