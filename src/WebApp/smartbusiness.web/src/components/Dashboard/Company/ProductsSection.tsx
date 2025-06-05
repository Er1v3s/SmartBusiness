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

const mockProducts = [
  ...Array.from({ length: 100 }, (_, i) => ({
    id: 1 + i,
    name: `Produkt ${String.fromCharCode(68 + (i % 26))}`,
    description: `Opis produktu ${String.fromCharCode(68 + (i % 26))}`,
    price: 30 + ((i * 15) % 300),
    vat: VAT_OPTIONS[i % VAT_OPTIONS.length],
    category: CATEGORIES[1 + (i % (CATEGORIES.length - 1))],
  })),
];

export const ProductsSection = () => {
  const [search, setSearch] = useState("");
  const [category, setCategory] = useState("Wszystkie");
  const [showFilters, setShowFilters] = useState(false);
  const [products, setProducts] = useState(mockProducts);
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  type EditProductForm = {
    name: string;
    description: string;
    price: string;
    vat: string;
    category: string;
  };
  const [editingId, setEditingId] = useState<number | null>(null);
  const [editForm, setEditForm] = useState<EditProductForm | null>(null);
  const [modalOpen, setModalOpen] = useState(false);

  // Filter and paginate products
  const filtered = products.filter(
    (p) =>
      (category === "Wszystkie" || p.category === category) &&
      (p.name.toLowerCase().includes(search.toLowerCase()) ||
        p.description.toLowerCase().includes(search.toLowerCase())),
  );
  const paginated = filtered.slice((page - 1) * pageSize, page * pageSize);
  const pageCount = Math.ceil(filtered.length / pageSize);

  // Add new product
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
    setProducts([
      ...products,
      { id: Date.now(), name, description, price, vat, category },
    ]);
    form.reset();
  };

  // Modal component for editing product
  function EditProductModal({
    open,
    onClose,
    form,
    setForm,
    onSave,
    onDelete,
  }: {
    open: boolean;
    onClose: () => void;
    form: EditProductForm | null;
    setForm: (f: EditProductForm) => void;
    onSave: () => void;
    onDelete: () => void;
  }) {
    if (!open || !form) return null;
    return (
      <GenericModal
        open={open}
        title="Edytuj produkt"
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
              Nazwa produktu
            </span>
            <input
              id="edit-name"
              className="w-full rounded border-2 border-gray-300 px-4 py-2 shadow-sm sm:text-sm dark:border-gray-600 dark:bg-gray-900 dark:text-white"
              value={form.name}
              onChange={(e) => setForm({ ...form, name: e.target.value })}
              placeholder="Nazwa produktu"
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
      {/* Form of adding new products */}
      <form
        onSubmit={handleAdd}
        className="mb-6 flex flex-wrap justify-around rounded-lg border-2 border-gray-100 bg-white p-4 shadow dark:border-gray-700 dark:bg-gray-800"
      >
        <input
          name="name"
          required
          placeholder="Nazwa produktu"
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
          <ButtonSuccess text="Dodaj produkt" type="submit" />
        </div>
      </form>

      {/* Search and filter bar */}
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

      {/* Products table*/}
      <div className="overflow-x-auto rounded-lg bg-white shadow dark:bg-gray-800">
        <table className="min-w-full text-sm">
          <thead>
            <tr className="bg-gray-100 dark:bg-gray-700">
              <th className="px-4 py-2 text-left">Nazwa</th>
              <th className="px-4 py-2 text-left">Opis</th>
              <th className="px-4 py-2 text-left">Kategoria</th>
              <th className="px-4 py-2 text-right">Cena</th>
              <th className="px-4 py-2 text-right">VAT</th>
            </tr>
          </thead>
          <tbody>
            {paginated.length === 0 ? (
              <tr>
                <td colSpan={5} className="py-6 text-center text-gray-400">
                  Brak wyników
                </td>
              </tr>
            ) : (
              paginated.map((p) => (
                <tr
                  key={p.id}
                  className="cursor-pointer border-b border-gray-100 transition hover:bg-indigo-50/40 dark:border-gray-700 dark:hover:bg-gray-700/40"
                  onClick={() => {
                    setEditingId(p.id);
                    setEditForm({ ...p, price: p.price.toString() });
                    setModalOpen(true);
                  }}
                >
                  <td className="px-4 py-2 font-medium">{p.name}</td>
                  <td className="px-4 py-2">{p.description}</td>
                  <td className="px-4 py-2">{p.category}</td>
                  <td className="px-4 py-2 text-right">
                    {p.price.toFixed(2)} zł
                  </td>
                  <td className="px-4 py-2 text-right">{p.vat}</td>
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

      {/* Modal of product editing */}
      <EditProductModal
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
          setProducts(
            products.map((pr) =>
              pr.id === editingId
                ? { ...pr, ...editForm, price: parseFloat(editForm.price) }
                : pr,
            ),
          );
          setModalOpen(false);
          setEditingId(null);
          setEditForm(null);
        }}
        onDelete={() => {
          if (editingId == null) return;
          setProducts(products.filter((pr) => pr.id !== editingId));
          setModalOpen(false);
          setEditingId(null);
          setEditForm(null);
        }}
      />
    </div>
  );
};
