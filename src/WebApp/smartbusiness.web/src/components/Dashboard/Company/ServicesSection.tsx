import { ArrowLeft, ArrowRight, Search, X } from "lucide-react";
import { useState } from "react";
import {
  ButtonError,
  ButtonNeutral,
  ButtonSuccess,
} from "../../General/Buttons";

const VAT_OPTIONS = ["23%", "8%", "5%", "0%", "zw."];
const CATEGORIES = ["Wszystkie", "Kategoria 1", "Kategoria 2", "Kategoria 3"];

const mockServices = [
  ...Array.from({ length: 30 }, (_, i) => ({
    id: 1 + i,
    name: `Usługa ${String.fromCharCode(68 + (i % 26))}`,
    description: `Opis usługi ${String.fromCharCode(68 + (i % 26))}`,
    price: 30 + ((i * 15) % 300),
    vat: VAT_OPTIONS[i % VAT_OPTIONS.length],
    category: CATEGORIES[1 + (i % (CATEGORIES.length - 1))],
    duration: 30 + (i % 120),
  })),
];

export const ServicesSection = () => {
  const [search, setSearch] = useState("");
  const [category, setCategory] = useState("Wszystkie");
  const [showFilters, setShowFilters] = useState(false);
  const [services, setServices] = useState(mockServices);
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(6);
  type EditServiceForm = {
    name: string;
    description: string;
    price: string;
    vat: string;
    category: string;
    duration: string;
  };
  const [editingId, setEditingId] = useState<number | null>(null);
  const [editForm, setEditForm] = useState<EditServiceForm | null>(null);
  const [modalOpen, setModalOpen] = useState(false);

  // Filtrowanie i wyszukiwanie
  const filtered = services.filter(
    (s) =>
      (category === "Wszystkie" || s.category === category) &&
      (s.name.toLowerCase().includes(search.toLowerCase()) ||
        s.description.toLowerCase().includes(search.toLowerCase())),
  );
  const paginated = filtered.slice((page - 1) * pageSize, page * pageSize);
  const pageCount = Math.ceil(filtered.length / pageSize);

  // Dodawanie usługi (mock)
  const handleAdd = (e: React.FormEvent) => {
    e.preventDefault();
    const form = e.target as HTMLFormElement;
    const name = (form.elements.namedItem("name") as HTMLInputElement).value;
    const description = (
      form.elements.namedItem("description") as HTMLInputElement
    ).value;
    const price = parseFloat(
      (form.elements.namedItem("price") as HTMLInputElement).value,
    );
    const vat = (form.elements.namedItem("vat") as HTMLSelectElement).value;
    const category = (form.elements.namedItem("category") as HTMLSelectElement)
      .value;
    const duration = parseInt(
      (form.elements.namedItem("duration") as HTMLInputElement).value,
      10,
    );
    setServices([
      ...services,
      { id: Date.now(), name, description, price, vat, category, duration },
    ]);
    form.reset();
  };

  // Modal komponent do edycji usługi
  function EditServiceModal({
    open,
    onClose,
    form,
    setForm,
    onSave,
    onDelete,
  }: {
    open: boolean;
    onClose: () => void;
    form: EditServiceForm | null;
    setForm: (f: EditServiceForm) => void;
    onSave: () => void;
    onDelete: () => void;
  }) {
    if (!open || !form) return null;
    return (
      <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/60 backdrop-blur-sm">
        <div className="relative w-full max-w-md rounded-lg bg-white p-6 shadow-lg dark:bg-gray-800">
          <button
            className="absolute top-3 right-3 text-gray-400 hover:text-gray-700 dark:hover:text-white"
            onClick={onClose}
            aria-label="Zamknij"
          >
            <span className="text-2xl">
              <X />
            </span>
          </button>
          <h2 className="mb-4 text-lg font-semibold text-gray-800 dark:text-gray-100">
            Edytuj usługę
          </h2>
          <form
            className="flex flex-col gap-2"
            onSubmit={(e) => {
              e.preventDefault();
              onSave();
            }}
          >
            <label htmlFor="edit-name">
              <span className="flex flex-col text-sm font-medium text-gray-700 dark:text-gray-200">
                Nazwa usługi
              </span>
              <input
                id="edit-name"
                className="w-full rounded border-2 border-gray-300 px-4 py-2 shadow-sm sm:text-sm dark:border-gray-600 dark:bg-gray-900 dark:text-white"
                value={form.name}
                onChange={(e) => setForm({ ...form, name: e.target.value })}
                placeholder="Nazwa usługi"
                autoFocus
              />
            </label>

            <label htmlFor="edit-description">
              <span className="mb-0.5 flex flex-col text-sm font-medium text-gray-700 dark:text-gray-200">
                Opis
              </span>
              <input
                id="edit-description"
                className="w-full rounded border-2 border-gray-300 px-4 py-2 shadow-sm sm:text-sm dark:border-gray-600 dark:bg-gray-900 dark:text-white"
                value={form.description}
                onChange={(e) =>
                  setForm({ ...form, description: e.target.value })
                }
                placeholder="Opis"
              />
            </label>

            <label htmlFor="edit-category">
              <span className="mb-0.5 text-sm font-medium text-gray-700 dark:text-gray-200">
                Kategoria
              </span>
              <select
                id="edit-category"
                className="w-full rounded border-2 border-gray-300 px-4 py-2 shadow-sm sm:text-sm dark:border-gray-600 dark:bg-gray-900 dark:text-white"
                value={form.category}
                onChange={(e) => setForm({ ...form, category: e.target.value })}
              >
                {CATEGORIES.slice(1).map((c) => (
                  <option key={c}>{c}</option>
                ))}
              </select>
            </label>

            <label htmlFor="edit-duration">
              <span className="mb-0.5 text-sm font-medium text-gray-700 dark:text-gray-200">
                Czas trwania (min)
              </span>
              <input
                id="edit-duration"
                className="w-full rounded border-2 border-gray-300 px-4 py-2 shadow-sm sm:text-sm dark:border-gray-600 dark:bg-gray-900 dark:text-white"
                type="number"
                min="1"
                step="1"
                value={form.duration}
                onChange={(e) => setForm({ ...form, duration: e.target.value })}
                placeholder="Czas trwania (min)"
              />
            </label>

            <label htmlFor="edit-price">
              <span className="mb-0.5 text-sm font-medium text-gray-700 dark:text-gray-200">
                Cena
              </span>
              <input
                id="edit-price"
                className="w-full rounded border-2 border-gray-300 px-4 py-2 shadow-sm sm:text-sm dark:border-gray-600 dark:bg-gray-900 dark:text-white"
                type="number"
                min="0"
                step="0.01"
                value={form.price}
                onChange={(e) => setForm({ ...form, price: e.target.value })}
                placeholder="Cena"
              />
            </label>

            <label htmlFor="edit-vat">
              <span className="mb-0.5 text-sm font-medium text-gray-700 dark:text-gray-200">
                VAT
              </span>
              <select
                id="edit-vat"
                className="w-full rounded border-2 border-gray-300 px-4 py-2 shadow-sm sm:text-sm dark:border-gray-600 dark:bg-gray-900 dark:text-white"
                value={form.vat}
                onChange={(e) => setForm({ ...form, vat: e.target.value })}
              >
                {VAT_OPTIONS.map((v) => (
                  <option key={v}>{v}</option>
                ))}
              </select>
            </label>

            <div className="mt-6 flex justify-between gap-2">
              <ButtonError text="Usuń" type="button" onClick={onDelete} />
              <div className="flex gap-2">
                <ButtonNeutral text="Anuluj" type="button" onClick={onClose} />
                <ButtonSuccess text="Zapisz" type="submit" onClick={onSave} />
              </div>
            </div>
          </form>
        </div>
      </div>
    );
  }

  return (
    <div className="h-full w-full p-4">
      {/* Formularz dodawania usługi */}
      <form
        onSubmit={handleAdd}
        className="mb-6 flex flex-wrap justify-around rounded-lg border-2 border-gray-100 bg-white p-4 shadow dark:border-gray-700 dark:bg-gray-800"
      >
        <input
          name="name"
          required
          placeholder="Nazwa usługi"
          className="input w-1/5 rounded-lg border-2 border-gray-200 px-3 text-gray-800 dark:border-gray-700 dark:text-gray-100"
        />
        <input
          name="description"
          required
          placeholder="Opis"
          className="input w-1/5 rounded-lg border-2 border-gray-200 px-3 text-gray-800 dark:border-gray-700 dark:text-gray-100"
        />
        <select
          name="category"
          required
          className="input rounded-lg border-2 border-gray-200 bg-white px-1 text-gray-800 shadow dark:border-gray-700 dark:bg-gray-800 dark:text-gray-100"
        >
          {CATEGORIES.slice(1).map((c) => (
            <option key={c}>{c}</option>
          ))}
        </select>
        <input
          name="duration"
          required
          type="number"
          min="1"
          step="1"
          placeholder="Czas"
          className="input w-1/12 rounded-lg border-2 border-gray-200 px-3 text-gray-800 dark:border-gray-700 dark:text-gray-100"
        />
        <input
          name="price"
          required
          type="number"
          min="0"
          step="0.01"
          placeholder="Cena"
          className="input w-1/12 rounded-lg border-2 border-gray-200 px-3 text-gray-800 dark:border-gray-700 dark:text-gray-100"
        />
        <select
          name="vat"
          required
          className="input rounded-lg border-2 border-gray-200 bg-white px-1 text-gray-800 shadow dark:border-gray-700 dark:bg-gray-800 dark:text-gray-100"
        >
          {VAT_OPTIONS.map((v) => (
            <option key={v}>{v}</option>
          ))}
        </select>
        <div className="">
          <ButtonSuccess text="Dodaj usługę" type="submit" />
        </div>
      </form>

      {/* Pasek wyszukiwania i filtr */}
      <div className="mb-4 flex flex-wrap items-center gap-2">
        <div className="flex flex-1 items-center gap-2">
          <label htmlFor="SearchService">
            <div className="relative">
              <input
                id="SearchService"
                type="search"
                placeholder="Szukaj usługi..."
                value={search}
                onChange={(e) => setSearch(e.target.value)}
                className="rounded-lg border-2 border-gray-300 bg-white p-3 text-gray-800 shadow-sm md:min-w-md dark:border-gray-700 dark:bg-gray-800 dark:text-gray-100"
              />
              <span className="absolute inset-y-0 right-2 grid w-8 place-content-center">
                <button
                  type="button"
                  aria-label="Submit"
                  className="rounded-full p-1.5 text-gray-700 transition-colors hover:bg-gray-100 dark:text-gray-200 dark:hover:bg-gray-800"
                >
                  <Search />
                </button>
              </span>
            </div>
          </label>
        </div>
        <ButtonNeutral
          type="button"
          text="Filtruj"
          onClick={() => setShowFilters((v) => !v)}
        />
        {showFilters && (
          <select
            value={category}
            onChange={(e) => setCategory(e.target.value)}
            className="input rounded-lg border-2 border-gray-300 bg-gray-50 px-3 py-2 text-gray-800 dark:border-gray-700 dark:bg-gray-800 dark:text-gray-200"
          >
            {CATEGORIES.map((c) => (
              <option key={c}>{c}</option>
            ))}
          </select>
        )}
      </div>

      {/* Tabela usług */}
      <div className="overflow-x-auto rounded-lg bg-white shadow dark:bg-gray-800">
        <table className="min-w-full text-sm">
          <thead>
            <tr className="bg-gray-100 dark:bg-gray-700">
              <th className="px-4 py-2 text-left">Nazwa</th>
              <th className="px-4 py-2 text-left">Opis</th>
              <th className="px-4 py-2 text-left">Kategoria</th>
              <th className="px-4 py-2 text-right">Czas (min)</th>
              <th className="px-4 py-2 text-right">Cena</th>
              <th className="px-4 py-2 text-right">VAT</th>
            </tr>
          </thead>
          <tbody>
            {paginated.length === 0 ? (
              <tr>
                <td colSpan={6} className="py-6 text-center text-gray-400">
                  Brak wyników
                </td>
              </tr>
            ) : (
              paginated.map((s) => (
                <tr
                  key={s.id}
                  className="cursor-pointer border-b border-gray-100 transition hover:bg-indigo-50/40 dark:border-gray-700 dark:hover:bg-gray-700/40"
                  onClick={() => {
                    setEditingId(s.id);
                    setEditForm({
                      ...s,
                      price: s.price.toString(),
                      duration: s.duration.toString(),
                    });
                    setModalOpen(true);
                  }}
                >
                  <td className="px-4 py-2 font-medium">{s.name}</td>
                  <td className="px-4 py-2">{s.description}</td>
                  <td className="px-4 py-2">{s.category}</td>
                  <td className="px-4 py-2 text-right">{s.duration}</td>
                  <td className="px-4 py-2 text-right">
                    {s.price.toFixed(2)} zł
                  </td>
                  <td className="px-4 py-2 text-right">{s.vat}</td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      {/* Paginacja */}
      <div className="relative mt-4 flex min-w-[180px] flex-wrap items-center justify-center gap-4">
        <div className="mx-auto flex items-center gap-4">
          <button
            className="rounded bg-gray-100 px-2 py-1 text-gray-700 shadow disabled:opacity-30 dark:bg-gray-800 dark:text-gray-100"
            onClick={() => setPage((p) => Math.max(1, p - 1))}
            disabled={page === 1}
            aria-label="Poprzednia strona"
          >
            <ArrowLeft />
          </button>
          <span className="text-sm font-medium text-gray-800 select-none dark:text-gray-100">
            {page}/{pageCount}
          </span>
          <button
            className="rounded bg-gray-100 px-2 py-1 text-gray-700 shadow disabled:opacity-30 dark:bg-gray-800 dark:text-gray-100"
            onClick={() => setPage((p) => Math.min(pageCount, p + 1))}
            disabled={page === pageCount}
            aria-label="Następna strona"
          >
            <ArrowRight />
          </button>
        </div>
        <div className="absolute right-0 flex items-center gap-1 pl-6 text-xs text-gray-800 dark:text-gray-100">
          <span>Ilość na stronę:</span>
          <select
            value={pageSize}
            onChange={(e) => {
              setPageSize(Number(e.target.value));
              setPage(1);
            }}
            className="input w-16 bg-gray-100 px-1 py-0.5 text-xs text-gray-800 shadow dark:bg-gray-800 dark:text-gray-100"
          >
            {[5, 10, 15].map((val) => (
              <option key={val} value={val}>
                {val}
              </option>
            ))}
          </select>
        </div>
      </div>
      {/* Modal edycji usługi */}
      <EditServiceModal
        open={modalOpen}
        onClose={() => {
          setModalOpen(false);
          setEditingId(null);
          setEditForm(null);
        }}
        form={editForm}
        setForm={(f) => setEditForm(f)}
        onSave={() => {
          if (!editForm || editingId == null) return;
          setServices(
            services.map((sv) =>
              sv.id === editingId
                ? {
                    ...sv,
                    ...editForm,
                    price: parseFloat(editForm.price),
                    duration: parseInt(editForm.duration, 10),
                  }
                : sv,
            ),
          );
          setModalOpen(false);
          setEditingId(null);
          setEditForm(null);
        }}
        onDelete={() => {
          if (editingId == null) return;
          setServices(services.filter((sv) => sv.id !== editingId));
          setModalOpen(false);
          setEditingId(null);
          setEditForm(null);
        }}
      />
    </div>
  );
};
