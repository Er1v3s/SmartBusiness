import { ArrowLeft, ArrowRight } from "lucide-react";
import { useState } from "react";

const VAT_OPTIONS = ["23%", "8%", "5%", "0%", "zw."];
const CATEGORIES = ["Wszystkie", "Kategoria 1", "Kategoria 2", "Kategoria 3"];

const mockServices = [
  ...Array.from({ length: 30 }, (_, i) => ({
    id: 1 + i,
    name: `Produkt ${String.fromCharCode(68 + (i % 26))}`,
    description: `Opis produktu ${String.fromCharCode(68 + (i % 26))}`,
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
  const pageSize = 6;

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

  return (
    <div className="h-full w-full p-4">
      {/* Formularz dodawania usługi */}
      <form
        onSubmit={handleAdd}
        className="mb-6 flex flex-wrap items-end gap-2 rounded-lg bg-white p-4 shadow dark:bg-gray-800"
      >
        <input
          name="name"
          required
          placeholder="Nazwa usługi"
          className="input"
        />
        <input
          name="description"
          required
          placeholder="Opis"
          className="input"
        />
        <input
          name="price"
          required
          type="number"
          min="0"
          step="0.01"
          placeholder="Cena"
          className="input w-28"
        />
        <select name="vat" required className="input w-20">
          {VAT_OPTIONS.map((v) => (
            <option key={v}>{v}</option>
          ))}
        </select>
        <select name="category" required className="input w-32">
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
          placeholder="Czas (min)"
          className="input w-24"
        />
        <button type="submit" className="btn-primary">
          Dodaj usługę
        </button>
      </form>

      {/* Pasek wyszukiwania i filtr */}
      <div className="mb-4 flex flex-wrap items-center gap-2">
        <input
          type="search"
          placeholder="Szukaj usługi..."
          value={search}
          onChange={(e) => setSearch(e.target.value)}
          className="input min-w-[200px] flex-1"
        />
        <button
          type="button"
          className="btn-secondary"
          onClick={() => setShowFilters((v) => !v)}
        >
          Filtruj
        </button>
        {showFilters && (
          <select
            value={category}
            onChange={(e) => setCategory(e.target.value)}
            className="input w-40"
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
              <th className="px-4 py-2 text-right">Cena</th>
              <th className="px-4 py-2 text-right">VAT</th>
              <th className="px-4 py-2 text-left">Kategoria</th>
              <th className="px-4 py-2 text-right">Czas (min)</th>
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
              paginated.map((s) =>
                editingId === s.id ? (
                  editForm && (
                    <tr
                      key={s.id}
                      className="bg-indigo-50/40 dark:bg-gray-700/40"
                    >
                      <td className="px-4 py-2 font-medium">
                        <input
                          className="input w-full"
                          value={editForm.name}
                          onChange={(e) =>
                            setEditForm({ ...editForm, name: e.target.value })
                          }
                        />
                      </td>
                      <td className="px-4 py-2">
                        <input
                          className="input w-full"
                          value={editForm.description}
                          onChange={(e) =>
                            setEditForm({
                              ...editForm,
                              description: e.target.value,
                            })
                          }
                        />
                      </td>
                      <td className="px-4 py-2 text-right">
                        <input
                          className="input w-20 text-right"
                          type="number"
                          min="0"
                          step="0.01"
                          value={editForm.price}
                          onChange={(e) =>
                            setEditForm({ ...editForm, price: e.target.value })
                          }
                        />{" "}
                        zł
                      </td>
                      <td className="px-4 py-2 text-right">
                        <select
                          className="input w-16"
                          value={editForm.vat}
                          onChange={(e) =>
                            setEditForm({ ...editForm, vat: e.target.value })
                          }
                        >
                          {VAT_OPTIONS.map((v) => (
                            <option key={v}>{v}</option>
                          ))}
                        </select>
                      </td>
                      <td className="px-4 py-2">
                        <select
                          className="input w-28"
                          value={editForm.category}
                          onChange={(e) =>
                            setEditForm({
                              ...editForm,
                              category: e.target.value,
                            })
                          }
                        >
                          {CATEGORIES.slice(1).map((c) => (
                            <option key={c}>{c}</option>
                          ))}
                        </select>
                      </td>
                      <td className="px-4 py-2 text-right">
                        <input
                          className="input w-16 text-right"
                          type="number"
                          min="1"
                          step="1"
                          value={editForm.duration}
                          onChange={(e) =>
                            setEditForm({
                              ...editForm,
                              duration: e.target.value,
                            })
                          }
                        />
                      </td>
                      <td className="flex gap-2 px-4 py-2">
                        <button
                          className="btn-primary px-2 py-1 text-xs"
                          onClick={() => {
                            if (!editForm) return;
                            setServices(
                              services.map((sv) =>
                                sv.id === s.id
                                  ? {
                                      ...sv,
                                      ...editForm,
                                      price: parseFloat(editForm.price),
                                      duration: parseInt(editForm.duration, 10),
                                    }
                                  : sv,
                              ),
                            );
                            setEditingId(null);
                            setEditForm(null);
                          }}
                        >
                          Zapisz
                        </button>
                        <button
                          className="btn-secondary px-2 py-1 text-xs"
                          onClick={() => {
                            setEditingId(null);
                            setEditForm(null);
                          }}
                        >
                          Anuluj
                        </button>
                        <button
                          className="btn-secondary border-red-400 px-2 py-1 text-xs text-red-600"
                          onClick={() => {
                            setServices(
                              services.filter((sv) => sv.id !== s.id),
                            );
                            setEditingId(null);
                            setEditForm(null);
                          }}
                        >
                          Usuń
                        </button>
                      </td>
                    </tr>
                  )
                ) : (
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
                    }}
                  >
                    <td className="px-4 py-2 font-medium">{s.name}</td>
                    <td className="px-4 py-2">{s.description}</td>
                    <td className="px-4 py-2 text-right">
                      {s.price.toFixed(2)} zł
                    </td>
                    <td className="px-4 py-2 text-right">{s.vat}</td>
                    <td className="px-4 py-2">{s.category}</td>
                    <td className="px-4 py-2 text-right">{s.duration}</td>
                  </tr>
                ),
              )
            )}
          </tbody>
        </table>
      </div>

      {/* Paginacja */}
      <div className="mt-4 flex justify-center gap-2">
        <button
          className="rounded bg-gray-200 px-2 py-1 text-gray-700 disabled:opacity-50 dark:bg-gray-700 dark:text-gray-200"
          onClick={() => setPage((p) => Math.max(1, p - 1))}
          disabled={page === 1}
          aria-label="Poprzednia strona"
        >
          <ArrowLeft />
        </button>

        {Array.from({ length: pageCount }, (_, i) => (
          <button
            key={i}
            className={`rounded px-3 py-1 ${page === i + 1 ? "bg-indigo-600 text-white" : "bg-gray-200 text-gray-700 dark:bg-gray-700 dark:text-gray-200"}`}
            onClick={() => setPage(i + 1)}
          >
            {i + 1}
          </button>
        ))}

        <button
          className="rounded bg-gray-200 px-2 py-1 text-gray-700 disabled:opacity-50 dark:bg-gray-700 dark:text-gray-200"
          onClick={() => setPage((p) => Math.min(pageCount, p + 1))}
          disabled={page === pageCount}
          aria-label="Następna strona"
        >
          <ArrowRight />
        </button>
      </div>
    </div>
  );
};
