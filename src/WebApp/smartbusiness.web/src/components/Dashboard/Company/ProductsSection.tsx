import { useState, useEffect, useCallback } from "react";
import type { FormEvent } from "react";
import {
  ButtonError,
  ButtonNeutral,
  ButtonSuccess,
} from "../../General/Buttons";
import GenericModal from "./ProductServicesSection/GenericModal";
import GenericPagination from "./ProductServicesSection/GenericPagination";
import GenericSearchFilterBar from "./ProductServicesSection/GenericSearchFilterBar";
import { useProduct } from "../../../context/product/ProductContext";
import type { GetProductsByParamsQuery, Product } from "../../../models";
import type { ApiResponseError } from "../../../models/authErrors";
import { useAlert } from "../../../context/alert/useAlert";

export const ProductsSection = () => {
  const [search, setSearch] = useState("");
  const [category, setCategory] = useState("Wszystkie");
  const [showFilters, setShowFilters] = useState(false);
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);

  type EditProductForm = {
    name: string;
    description: string;
    price: string;
    tax: string;
    category: string;
  };

  const [editingId, setEditingId] = useState<string | null>(null);
  const [editForm, setEditForm] = useState<EditProductForm | null>(null);
  const [modalOpen, setModalOpen] = useState(false);

  const { fetchProducts, createProduct, updateProduct, deleteProduct } =
    useProduct();

  const { showAlert } = useAlert();

  // Fetch products from ProductContext
  const fetchProductsFromContext = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const params: GetProductsByParamsQuery = {};
      if (search) params.Name = search;
      if (category && category !== "Wszystkie") params.Category = category;

      // Możesz dodać inne parametry jeśli chcesz
      const data = await fetchProducts(params);
      setProducts(data);
    } catch (err) {
      const error = err as ApiResponseError;
      setError(error?.detail || "Błąd podczas pobierania produktów");
    } finally {
      setLoading(false);
    }
  }, [search, category, fetchProducts]);

  useEffect(() => {
    fetchProductsFromContext();
  }, [fetchProductsFromContext]);

  // Paginuj tylko na froncie:
  const paginated = products.slice((page - 1) * pageSize, page * pageSize);
  const pageCount = Math.ceil(products.length / pageSize);

  // --- Add Product Form State & Validation ---
  type AddForm = {
    name: string;
    description: string;
    category: string;
    price: string;
    vat: string;
  };
  type AddFormErrors = {
    name?: string;
    description?: string;
    category?: string;
    price?: string;
    vat?: string;
  };
  const [addForm, setAddForm] = useState<AddForm>({
    name: "",
    description: "",
    category: "",
    price: "",
    vat: "",
  });
  const [addFormErrors, setAddFormErrors] = useState<AddFormErrors>({});
  const [addFormTouched, setAddFormTouched] = useState<{
    [K in keyof AddForm]?: boolean;
  }>({});
  const [addFormSubmitting, setAddFormSubmitting] = useState(false);

  // Validation functions
  function validateName(name: string): string | undefined {
    if (!name) return "Nazwa produktu jest wymagana.";
    if (name.length < 3) return "Nazwa produktu musi mieć co najmniej 3 znaki.";
    if (name.length > 100)
      return "Nazwa produktu nie może być dłuższa niż 100 znaków.";
    return undefined;
  }

  function validateDescription(description: string): string | undefined {
    if (!description) return "Opis jest wymagany.";
    if (description.length < 3) return "Opis musi mieć co najmniej 3 znaki.";
    if (description.length > 500)
      return "Opis nie może być dłuższy niż 500 znaków.";
    return undefined;
  }

  function validateCategory(category: string): string | undefined {
    if (!category) return "Kategoria jest wymagana.";
    if (category.length < 3) return "Kategoria musi mieć co najmniej 3 znaki.";
    if (category.length > 50)
      return "Kategoria nie może być dłuższa niż 50 znaków.";
    return undefined;
  }

  function validatePrice(price: string): string | undefined {
    if (!price) return "Cena jest wymagana.";
    const num = Number(price);
    if (isNaN(num)) return "Cena musi być liczbą.";
    if (num <= 0) return "Cena musi być większa niż 0.";
    // Max 7 digits before decimal, 2 after
    const match = price.match(/^\d{1,7}(\.\d{1,2})?$/);
    if (!match)
      return "Cena może mieć maksymalnie 7 cyfr przed przecinkiem i 2 po przecinku.";
    return undefined;
  }

  function validateVat(vat: string): string | undefined {
    if (!vat) return "VAT jest wymagany.";
    if (!/^\d+$/.test(vat)) return "VAT musi być liczbą całkowitą.";
    const num = Number(vat);
    if (num < 0 || num > 100) return "VAT musi być w zakresie 0-100.";
    return undefined;
  }

  const validateAddForm = useCallback((form: AddForm): AddFormErrors => {
    return {
      name: validateName(form.name),
      description: validateDescription(form.description),
      category: validateCategory(form.category),
      price: validatePrice(form.price),
      vat: validateVat(form.vat),
    };
  }, []);

  // Update errors on change
  useEffect(() => {
    setAddFormErrors(validateAddForm(addForm));
  }, [addForm, validateAddForm]);
  // Check if form is valid
  const isAddFormValid =
    Object.values(addFormErrors).every((e) => !e) &&
    Object.values(addForm).every((v) => v !== "");

  // --- Add new product handler ---
  const handleAdd = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setAddFormTouched({
      name: true,
      description: true,
      category: true,
      price: true,
      vat: true,
    });
    const errors = validateAddForm(addForm);
    setAddFormErrors(errors);
    if (Object.values(errors).some((e) => e)) return;
    setAddFormSubmitting(true);
    try {
      await createProduct({
        name: addForm.name,
        description: addForm.description,
        price: parseFloat(addForm.price),
        tax: parseInt(addForm.vat, 10),
        category: addForm.category,
      });
      await fetchProductsFromContext();
      setAddForm({
        name: "",
        description: "",
        category: "",
        price: "",
        vat: "",
      });
      setAddFormTouched({});
    } catch (err) {
      const error = err as ApiResponseError;
      showAlert({
        title: error?.title || "Błąd",
        message: error?.detail || "Błąd podczas dodawania produktu",
        type: "error",
        duration: 3000,
      });
    } finally {
      setAddFormSubmitting(false);
    }
  };

  // Modal component for editing product
  function EditProductModal({
    open,
    onClose,
    form,
    onSave,
    onDelete,
  }: {
    open: boolean;
    onClose: () => void;
    form: EditProductForm | null;
    onSave: (form: EditProductForm) => Promise<boolean>;
    onDelete: () => Promise<boolean>;
  }) {
    const [localForm, setLocalForm] = useState<EditProductForm | null>(form);
    const [saving, setSaving] = useState(false);
    const [deleting, setDeleting] = useState(false);
    // --- Validation state for edit modal ---
    type EditFormErrors = {
      name?: string;
      description?: string;
      category?: string;
      price?: string;
      tax?: string;
    };
    const [editFormErrors, setEditFormErrors] = useState<EditFormErrors>({});
    const [editFormTouched, setEditFormTouched] = useState<{
      [K in keyof EditProductForm]?: boolean;
    }>({});

    // Validation functions (reuse from add form)
    function validateEditName(name: string): string | undefined {
      if (!name) return "Nazwa produktu jest wymagana.";
      if (name.length < 3)
        return "Nazwa produktu musi mieć co najmniej 3 znaki.";
      if (name.length > 100)
        return "Nazwa produktu nie może być dłuższa niż 100 znaków.";
      return undefined;
    }
    function validateEditDescription(description: string): string | undefined {
      if (!description) return "Opis jest wymagany.";
      if (description.length < 3) return "Opis musi mieć co najmniej 3 znaki.";
      if (description.length > 500)
        return "Opis nie może być dłuższy niż 500 znaków.";
      return undefined;
    }
    function validateEditCategory(category: string): string | undefined {
      if (!category) return "Kategoria jest wymagana.";
      if (category.length < 3)
        return "Kategoria musi mieć co najmniej 3 znaki.";
      if (category.length > 50)
        return "Kategoria nie może być dłuższa niż 50 znaków.";
      return undefined;
    }
    function validateEditPrice(price: string): string | undefined {
      if (!price) return "Cena jest wymagana.";
      const num = Number(price);
      if (isNaN(num)) return "Cena musi być liczbą.";
      if (num <= 0) return "Cena musi być większa niż 0.";
      const match = price.match(/^\d{1,7}(\.\d{1,2})?$/);
      if (!match)
        return "Cena może mieć maksymalnie 7 cyfr przed przecinkiem i 2 po przecinku.";
      return undefined;
    }
    function validateEditTax(tax: string): string | undefined {
      if (!tax) return "VAT jest wymagany.";
      if (!/^\d+$/.test(tax)) return "VAT musi być liczbą całkowitą.";
      const num = Number(tax);
      if (num < 0 || num > 100) return "VAT musi być w zakresie 0-100.";
      return undefined;
    }
    const validateEditForm = useCallback(
      (form: EditProductForm): EditFormErrors => {
        return {
          name: validateEditName(form.name),
          description: validateEditDescription(form.description),
          category: validateEditCategory(form.category),
          price: validateEditPrice(form.price),
          tax: validateEditTax(form.tax),
        };
      },
      [],
    );
    useEffect(() => {
      if (localForm) setEditFormErrors(validateEditForm(localForm));
    }, [localForm, validateEditForm]);
    const isEditFormValid =
      localForm &&
      Object.values(editFormErrors).every((e) => !e) &&
      Object.values(localForm).every((v) => v !== "");

    useEffect(() => {
      if (open) {
        setLocalForm(form);
        setSaving(false);
        setDeleting(false);
        setEditFormTouched({});
      }
    }, [open, form]);

    if (!open || !localForm) return null;
    return (
      <GenericModal
        open={open}
        title="Edytuj produkt"
        onClose={onClose}
        actions={null} // Remove actions from GenericModal, move all buttons to form footer
      >
        <form
          className="flex flex-col gap-2"
          onSubmit={async (e) => {
            e.preventDefault();
            setEditFormTouched({
              name: true,
              description: true,
              category: true,
              price: true,
              tax: true,
            });
            const errors = validateEditForm(localForm);
            setEditFormErrors(errors);
            if (Object.values(errors).some((e) => e)) return;
            setSaving(true);
            const success = await onSave(localForm);
            setSaving(false);
            if (success) onClose();
          }}
        >
          <label htmlFor="edit-name">
            <span className="flex flex-col text-sm font-medium text-gray-700 dark:text-gray-200">
              Nazwa produktu
            </span>
            <input
              id="edit-name"
              className={`w-full rounded border-2 px-4 py-2 shadow-sm sm:text-sm dark:border-gray-600 dark:bg-gray-900 dark:text-white ${editFormErrors.name && editFormTouched.name ? "border-red-500 dark:border-red-400" : "border-gray-300"}`}
              value={localForm.name}
              onChange={(e) => {
                setLocalForm({ ...localForm, name: e.target.value });
                setEditFormTouched((t) => ({ ...t, name: true }));
              }}
              onBlur={() => setEditFormTouched((t) => ({ ...t, name: true }))}
              placeholder="Nazwa produktu"
              autoFocus
            />
            {editFormErrors.name && editFormTouched.name && (
              <span className="mt-1 text-xs text-red-500">
                {editFormErrors.name}
              </span>
            )}
          </label>
          <label htmlFor="edit-description">
            <span className="mb-0.5 flex flex-col text-sm font-medium text-gray-700 dark:text-gray-200">
              Opis
            </span>
            <input
              id="edit-description"
              className={`w-full rounded border-2 px-4 py-2 shadow-sm sm:text-sm dark:border-gray-600 dark:bg-gray-900 dark:text-white ${editFormErrors.description && editFormTouched.description ? "border-red-500 dark:border-red-400" : "border-gray-300"}`}
              value={localForm.description}
              onChange={(e) => {
                setLocalForm({ ...localForm, description: e.target.value });
                setEditFormTouched((t) => ({ ...t, description: true }));
              }}
              onBlur={() =>
                setEditFormTouched((t) => ({ ...t, description: true }))
              }
              placeholder="Opis"
            />
            {editFormErrors.description && editFormTouched.description && (
              <span className="mt-1 text-xs text-red-500">
                {editFormErrors.description}
              </span>
            )}
          </label>
          <label htmlFor="edit-category">
            <span className="mb-0.5 text-sm font-medium text-gray-700 dark:text-gray-200">
              Kategoria
            </span>
            <input
              id="edit-category"
              className={`w-full rounded border-2 px-4 py-2 shadow-sm sm:text-sm dark:border-gray-600 dark:bg-gray-900 dark:text-white ${editFormErrors.category && editFormTouched.category ? "border-red-500 dark:border-red-400" : "border-gray-300"}`}
              value={localForm.category}
              onChange={(e) => {
                setLocalForm({ ...localForm, category: e.target.value });
                setEditFormTouched((t) => ({ ...t, category: true }));
              }}
              onBlur={() =>
                setEditFormTouched((t) => ({ ...t, category: true }))
              }
              placeholder="Kategoria"
            />
            {editFormErrors.category && editFormTouched.category && (
              <span className="mt-1 text-xs text-red-500">
                {editFormErrors.category}
              </span>
            )}
          </label>
          <label htmlFor="edit-price">
            <span className="mb-0.5 text-sm font-medium text-gray-700 dark:text-gray-200">
              Cena
            </span>
            <input
              id="edit-price"
              className={`w-full rounded border-2 px-4 py-2 shadow-sm sm:text-sm dark:border-gray-600 dark:bg-gray-900 dark:text-white ${editFormErrors.price && editFormTouched.price ? "border-red-500 dark:border-red-400" : "border-gray-300"}`}
              type="number"
              min="0"
              step="0.01"
              value={localForm.price}
              onChange={(e) => {
                setLocalForm({ ...localForm, price: e.target.value });
                setEditFormTouched((t) => ({ ...t, price: true }));
              }}
              onBlur={() => setEditFormTouched((t) => ({ ...t, price: true }))}
              placeholder="Cena"
            />
            {editFormErrors.price && editFormTouched.price && (
              <span className="mt-1 text-xs text-red-500">
                {editFormErrors.price}
              </span>
            )}
          </label>
          <label htmlFor="edit-tax">
            <span className="mb-0.5 text-sm font-medium text-gray-700 dark:text-gray-200">
              VAT
            </span>
            <input
              id="edit-tax"
              className={`w-full rounded border-2 px-4 py-2 shadow-sm sm:text-sm dark:border-gray-600 dark:bg-gray-900 dark:text-white ${editFormErrors.tax && editFormTouched.tax ? "border-red-500 dark:border-red-400" : "border-gray-300"}`}
              type="number"
              min="0"
              step="1"
              value={localForm.tax}
              onChange={(e) => {
                setLocalForm({ ...localForm, tax: e.target.value });
                setEditFormTouched((t) => ({ ...t, tax: true }));
              }}
              onBlur={() => setEditFormTouched((t) => ({ ...t, tax: true }))}
              placeholder="VAT"
            />
            {editFormErrors.tax && editFormTouched.tax && (
              <span className="mt-1 text-xs text-red-500">
                {editFormErrors.tax}
              </span>
            )}
          </label>
          <div className="mt-4 flex flex-row justify-end gap-2">
            <div className="flex flex-1 justify-start">
              <ButtonError
                text={deleting ? "Usuwanie..." : "Usuń"}
                type="button"
                onClick={async () => {
                  setDeleting(true);
                  const success = await onDelete();
                  setDeleting(false);
                  if (success) onClose();
                }}
                disabled={deleting || saving}
              />
            </div>
            <div className="flex gap-2">
              <ButtonNeutral
                text="Anuluj"
                type="button"
                onClick={onClose}
                disabled={deleting || saving}
              />
              <ButtonSuccess
                text={saving ? "Zapisywanie..." : "Zapisz"}
                type="submit"
                disabled={saving || deleting || !isEditFormValid}
              />
            </div>
          </div>
        </form>
      </GenericModal>
    );
  }

  return (
    <div className="h-full w-full p-4">
      {/* Form of adding new products */}
      <form
        onSubmit={handleAdd}
        className="mb-6 grid grid-cols-1 gap-2 rounded-lg border-2 border-gray-100 bg-white p-4 shadow sm:grid-cols-7 sm:gap-4 dark:border-gray-700 dark:bg-gray-800"
        noValidate
      >
        <div className="col-span-1 flex flex-col sm:col-span-2">
          <input
            name="name"
            value={addForm.name}
            onChange={(e) => {
              setAddForm({ ...addForm, name: e.target.value });
              setAddFormTouched((t) => ({ ...t, name: true }));
            }}
            onBlur={() => setAddFormTouched((t) => ({ ...t, name: true }))}
            required
            placeholder="Nazwa produktu"
            className={`input rounded-lg border-2 px-3 py-2 text-gray-800 dark:text-gray-100 ${addFormErrors.name && addFormTouched.name ? "border-red-500 dark:border-red-400" : "border-gray-200 dark:border-gray-700"}`}
          />
          {addFormErrors.name && addFormTouched.name && (
            <span className="mt-1 text-xs text-red-500">
              {addFormErrors.name}
            </span>
          )}
        </div>
        <div className="col-span-1 flex flex-col sm:col-span-2">
          <input
            name="description"
            value={addForm.description}
            onChange={(e) => {
              setAddForm({ ...addForm, description: e.target.value });
              setAddFormTouched((t) => ({ ...t, description: true }));
            }}
            onBlur={() =>
              setAddFormTouched((t) => ({ ...t, description: true }))
            }
            required
            placeholder="Opis"
            className={`input rounded-lg border-2 px-3 py-2 text-gray-800 dark:text-gray-100 ${addFormErrors.description && addFormTouched.description ? "border-red-500 dark:border-red-400" : "border-gray-200 dark:border-gray-700"}`}
          />
          {addFormErrors.description && addFormTouched.description && (
            <span className="mt-1 text-xs text-red-500">
              {addFormErrors.description}
            </span>
          )}
        </div>
        <div className="col-span-1 flex flex-col sm:col-span-1">
          <input
            name="category"
            value={addForm.category}
            onChange={(e) => {
              setAddForm({ ...addForm, category: e.target.value });
              setAddFormTouched((t) => ({ ...t, category: true }));
            }}
            onBlur={() => setAddFormTouched((t) => ({ ...t, category: true }))}
            required
            placeholder="Kategoria"
            className={`input rounded-lg border-2 px-3 py-2 text-gray-800 dark:text-gray-100 ${addFormErrors.category && addFormTouched.category ? "border-red-500 dark:border-red-400" : "border-gray-200 dark:border-gray-700"}`}
          />
          {addFormErrors.category && addFormTouched.category && (
            <span className="mt-1 text-xs text-red-500">
              {addFormErrors.category}
            </span>
          )}
        </div>
        <div className="col-span-1 flex flex-col sm:col-span-1">
          <input
            name="price"
            value={addForm.price}
            onChange={(e) => {
              setAddForm({ ...addForm, price: e.target.value });
              setAddFormTouched((t) => ({ ...t, price: true }));
            }}
            onBlur={() => setAddFormTouched((t) => ({ ...t, price: true }))}
            required
            type="number"
            min="0"
            step="0.01"
            placeholder="Cena"
            className={`input rounded-lg border-2 px-3 py-2 text-gray-800 dark:text-gray-100 ${addFormErrors.price && addFormTouched.price ? "border-red-500 dark:border-red-400" : "border-gray-200 dark:border-gray-700"}`}
          />
          {addFormErrors.price && addFormTouched.price && (
            <span className="mt-1 text-xs text-red-500">
              {addFormErrors.price}
            </span>
          )}
        </div>
        <div className="col-span-1 flex flex-col sm:col-span-1">
          <input
            name="vat"
            value={addForm.vat}
            onChange={(e) => {
              setAddForm({ ...addForm, vat: e.target.value });
              setAddFormTouched((t) => ({ ...t, vat: true }));
            }}
            onBlur={() => setAddFormTouched((t) => ({ ...t, vat: true }))}
            required
            type="number"
            min="0"
            step="1"
            placeholder="VAT"
            className={`input rounded-lg border-2 px-3 py-2 text-gray-800 dark:text-gray-100 ${addFormErrors.vat && addFormTouched.vat ? "border-red-500 dark:border-red-400" : "border-gray-200 dark:border-gray-700"}`}
          />
          {addFormErrors.vat && addFormTouched.vat && (
            <span className="mt-1 text-xs text-red-500">
              {addFormErrors.vat}
            </span>
          )}
        </div>
        <div className="col-span-1 flex justify-center sm:col-span-7">
          <ButtonSuccess
            text={addFormSubmitting ? "Dodawanie..." : "Dodaj produkt"}
            type="submit"
            disabled={!isAddFormValid || addFormSubmitting}
          />
        </div>
      </form>

      {/* Search and filter bar */}
      <GenericSearchFilterBar
        search={search}
        onSearchChange={setSearch}
        filterLabel="Filtruj"
        filterValue={category}
        filterOptions={[]}
        onFilterChange={setCategory}
        showFilter={showFilters}
        onToggleFilter={() => setShowFilters((v) => !v)}
      />

      {/* Products table*/}
      <div className="overflow-x-auto rounded-lg bg-white shadow dark:bg-gray-800">
        {loading ? (
          <div className="p-8 text-center text-gray-400">Ładowanie...</div>
        ) : error ? (
          <div className="p-8 text-center text-red-500">{error}</div>
        ) : (
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
                      setEditForm({
                        name: p.name,
                        description: p.description,
                        price: p.price.toString(),
                        tax: p.tax.toString(),
                        category: p.category,
                      });
                      setModalOpen(true);
                    }}
                  >
                    <td className="px-4 py-2 font-medium">{p.name}</td>
                    <td className="px-4 py-2">{p.description}</td>
                    <td className="px-4 py-2">{p.category}</td>
                    <td className="px-4 py-2 text-right">
                      {p.price.toFixed(2)} zł
                    </td>
                    <td className="px-4 py-2 text-right">{p.tax}%</td>
                  </tr>
                ))
              )}
            </tbody>
          </table>
        )}
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
        key={editingId || "modal"}
        open={modalOpen}
        onClose={() => {
          setModalOpen(false);
          setEditingId(null);
          setEditForm(null);
        }}
        form={editForm}
        onSave={async (localForm) => {
          if (!localForm || editingId == null) return false;
          // Validate price/tax
          const price = parseFloat(localForm.price);
          const tax = parseInt(localForm.tax, 10);
          if (isNaN(price) || isNaN(tax)) {
            showAlert({
              title: "Błąd danych",
              message: "Cena i VAT muszą być liczbami.",
              type: "error",
              duration: 3000,
            });
            return false;
          }
          try {
            await updateProduct({
              id: editingId,
              name: localForm.name,
              description: localForm.description,
              category: localForm.category,
              price,
              tax,
            });
            await fetchProductsFromContext();
            showAlert({
              title: "Zaktualizowano",
              message: "Produkt został zaktualizowany.",
              type: "success",
              duration: 2000,
            });
            setModalOpen(false);
            setEditingId(null);
            setEditForm(null);
            return true;
          } catch (err) {
            const error = err as ApiResponseError;
            showAlert({
              title: error?.title || "Błąd",
              message: error?.detail || "Błąd podczas aktualizacji produktu",
              type: "error",
              duration: 3000,
            });
            return false;
          }
        }}
        onDelete={async () => {
          if (!editingId) return false;
          try {
            await deleteProduct(editingId);
            await fetchProductsFromContext();
            showAlert({
              title: "Usunięto",
              message: "Produkt został usunięty.",
              type: "success",
              duration: 2000,
            });
            setModalOpen(false);
            setEditingId(null);
            setEditForm(null);
            return true;
          } catch (err) {
            const error = err as ApiResponseError;
            showAlert({
              title: error?.title || "Błąd",
              message: error?.detail || "Błąd podczas usuwania produktu",
              type: "error",
              duration: 3000,
            });
            return false;
          }
        }}
      />
    </div>
  );
};
