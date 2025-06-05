import { useState } from "react";
import {
  ButtonError,
  ButtonNeutral,
  ButtonSuccess,
} from "../../General/Buttons";
import GenericModal from "./ProductServicesSection/GenericModal";
import GenericPagination from "./ProductServicesSection/GenericPagination";
import GenericSearchFilterBar from "./ProductServicesSection/GenericSearchFilterBar";

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
  const [pageSize, setPageSize] = useState(10);
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

  // Filter and paginate services
  const filtered = services.filter(
    (s) =>
      (category === "Wszystkie" || s.category === category) &&
      (s.name.toLowerCase().includes(search.toLowerCase()) ||
        s.description.toLowerCase().includes(search.toLowerCase())),
  );
  const paginated = filtered.slice((page - 1) * pageSize, page * pageSize);
  const pageCount = Math.ceil(filtered.length / pageSize);

  // Add new service
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

  // Modal to edit service
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
      <GenericModal
        open={open}
        title="Edytuj usługę"
        onClose={onClose}
        actions={
          <>
            <ButtonError text="Usuń" type="button" onClick={onDelete} />
            <div className="flex gap-2">
              <ButtonNeutral text="Anuluj" type="button" onClick={onClose} />
              <ButtonSuccess text="Zapisz" type="submit" onClick={onSave} />
            </div>
          </>
        }
      >
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
        </form>
      </GenericModal>
    );
  }

  return (
    <div className="h-full w-full p-4">
      {/* Form of adding new service */}
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

      {/* Search bar and filtering */}
      <GenericSearchFilterBar
        search={search}
        onSearchChange={setSearch}
        filterLabel="Filtruj"
        filterValue={category}
        filterOptions={CATEGORIES}
        onFilterChange={setCategory}
        showFilter={showFilters}
        onToggleFilter={() => setShowFilters((v) => !v)}
      />

      {/* Tabele of services */}
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

      {/* Pagination */}
      <GenericPagination
        page={page}
        pageCount={pageCount}
        pageSize={pageSize}
        onPageChange={setPage}
        onPageSizeChange={(val) => {
          setPageSize(val);
          setPage(1);
        }}
      />

      {/* Modal of editing services */}
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
