import React, { useEffect, useState, useCallback } from "react";
import GenericModal from "./GenericModal";
import GenericPagination from "./GenericPagination";
import GenericSearchFilterBar from "./GenericSearchFilterBar";
import {
  ButtonSuccess,
  ButtonNeutral,
  ButtonError,
} from "../../../General/Buttons";

export type FieldConfig<TForm> = {
  name: keyof TForm;
  label: string;
  type?: string;
  placeholder?: string;
  validate?: (value: string, form: TForm) => string | undefined;
  inputProps?: React.InputHTMLAttributes<HTMLInputElement>;
};

export type GenericEntitySectionProps<
  TEntity,
  TForm,
  TFormErrors extends Partial<Record<keyof TForm, string | undefined>>,
> = {
  title: string;
  fetchEntities: () => Promise<TEntity[]>;
  createEntity: (form: TForm) => Promise<void>;
  updateEntity: (id: string, form: TForm) => Promise<void>;
  deleteEntity: (id: string) => Promise<void>;
  fields: FieldConfig<TForm>[];
  formToEntity: (form: TForm) => Partial<TEntity>;
  entityToForm: (entity: TEntity) => TForm;
  renderTableRow: (entity: TEntity, onEdit: () => void) => React.ReactNode;
  getEntityId: (entity: TEntity) => string;
  initialForm: TForm;
  validateForm: (form: TForm) => TFormErrors;
  isFormValid: (form: TForm, errors: TFormErrors) => boolean;
  editModalTitle: string;
  addButtonText: string;
};

export default function GenericEntitySection<
  TEntity,
  TForm,
  TFormErrors extends Partial<Record<keyof TForm, string | undefined>>,
>(props: GenericEntitySectionProps<TEntity, TForm, TFormErrors>) {
  const {
    fetchEntities,
    createEntity,
    updateEntity,
    deleteEntity,
    fields,
    entityToForm,
    renderTableRow,
    getEntityId,
    initialForm,
    validateForm,
    isFormValid,
    editModalTitle,
    addButtonText,
  } = props;

  // --- State ---
  const [entities, setEntities] = useState<TEntity[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [search, setSearch] = useState("");
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);

  // --- Add form ---
  const [addForm, setAddForm] = useState<TForm>(initialForm);
  const [addFormErrors, setAddFormErrors] = useState<TFormErrors>(
    validateForm(initialForm),
  );
  const [addFormTouched, setAddFormTouched] = useState<
    Partial<Record<keyof TForm, boolean>>
  >({});
  const [addFormSubmitting, setAddFormSubmitting] = useState(false);

  // --- Edit modal ---
  const [editingId, setEditingId] = useState<string | null>(null);
  const [editForm, setEditForm] = useState<TForm | null>(null);
  const [modalOpen, setModalOpen] = useState(false);
  const [editFormErrors, setEditFormErrors] = useState<TFormErrors>(
    validateForm(initialForm),
  );
  const [editFormTouched, setEditFormTouched] = useState<
    Partial<Record<keyof TForm, boolean>>
  >({});
  const [editFormSubmitting, setEditFormSubmitting] = useState(false);

  // --- Fetch entities ---
  const fetchAll = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await fetchEntities();
      setEntities(data);
    } catch (err) {
      setError(
        (err as { detail?: string; message?: string })?.detail ||
          (err as { message?: string })?.message ||
          "Błąd podczas pobierania danych",
      );
    } finally {
      setLoading(false);
    }
  }, [fetchEntities]);

  useEffect(() => {
    fetchAll();
  }, [fetchAll]);

  // --- Search & filter ---
  const filtered = entities.filter((e) => {
    if (!search) return true;
    const s = search.toLowerCase();
    return Object.keys(e as object).some((key) => {
      const v = (e as Record<string, unknown>)[key];
      if (typeof v === "string" && v.toLowerCase().includes(s)) return true;
      if (typeof v === "number" && v.toString().includes(s)) return true;
      return false;
    });
  });

  // --- Pagination ---
  const paginated = filtered.slice((page - 1) * pageSize, page * pageSize);
  const pageCount = Math.max(1, Math.ceil(filtered.length / pageSize));

  // --- Add form validation ---
  useEffect(() => {
    setAddFormErrors(validateForm(addForm));
  }, [addForm, validateForm]);
  const isAddFormValid = isFormValid(addForm, addFormErrors);

  // --- Add entity handler ---
  const handleAdd = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setAddFormTouched(
      Object.fromEntries(
        Object.keys(addForm as object).map((k) => [k, true]),
      ) as Partial<Record<keyof TForm, boolean>>,
    );
    const errors = validateForm(addForm);
    setAddFormErrors(errors);
    if (!isFormValid(addForm, errors)) return;
    setAddFormSubmitting(true);
    try {
      await createEntity(addForm);
      await fetchAll();
      setAddForm(initialForm);
      setAddFormTouched({});
    } catch (err) {
      setError(
        (err as { detail?: string; message?: string })?.detail ||
          (err as { message?: string })?.message ||
          "Błąd podczas dodawania",
      );
    } finally {
      setAddFormSubmitting(false);
    }
  };

  // --- Edit modal logic ---
  const openEditModal = (entity: TEntity) => {
    setEditingId(getEntityId(entity));
    setEditForm(entityToForm(entity));
    setModalOpen(true);
    setEditFormTouched({});
  };
  const closeEditModal = () => {
    setModalOpen(false);
    setEditingId(null);
    setEditForm(null);
    setEditFormTouched({});
  };

  // --- Edit modal form state ---
  useEffect(() => {
    if (editForm) setEditFormErrors(validateForm(editForm));
  }, [editForm, validateForm]);
  const isEditFormValid = editForm
    ? isFormValid(editForm, editFormErrors)
    : false;

  // --- Edit entity handler ---
  const handleEditSave = async () => {
    if (!editForm || !editingId) return false;
    setEditFormTouched(
      Object.fromEntries(
        Object.keys(editForm as object).map((k) => [k, true]),
      ) as Partial<Record<keyof TForm, boolean>>,
    );
    const errors = validateForm(editForm);
    setEditFormErrors(errors);
    if (!isFormValid(editForm, errors)) return false;
    setEditFormSubmitting(true);
    try {
      await updateEntity(editingId, editForm);
      await fetchAll();
      closeEditModal();
      return true;
    } catch (err) {
      setError(
        (err as { detail?: string; message?: string })?.detail ||
          (err as { message?: string })?.message ||
          "Błąd podczas edycji",
      );
      return false;
    } finally {
      setEditFormSubmitting(false);
    }
  };

  // --- Delete entity handler ---
  const handleEditDelete = async () => {
    if (!editingId) return false;
    setEditFormSubmitting(true);
    try {
      await deleteEntity(editingId);
      await fetchAll();
      closeEditModal();
      return true;
    } catch (err) {
      setError(
        (err as { detail?: string; message?: string })?.detail ||
          (err as { message?: string })?.message ||
          "Błąd podczas usuwania",
      );
      return false;
    } finally {
      setEditFormSubmitting(false);
    }
  };

  // --- Render ---
  return (
    <div className="h-full w-full p-4">
      {/* Add new entity form */}
      <form
        onSubmit={handleAdd}
        className="mb-6 grid grid-cols-1 gap-2 rounded-lg border-2 border-gray-100 bg-white p-4 shadow sm:grid-cols-12 sm:gap-4 dark:border-gray-700 dark:bg-gray-800"
        noValidate
      >
        {fields.map((field) => (
          <div
            key={String(field.name)}
            className="col-span-1 flex flex-col sm:col-span-2"
          >
            <input
              name={String(field.name)}
              value={addForm[field.name] as unknown as string}
              onChange={(e) => {
                setAddForm({ ...addForm, [field.name]: e.target.value });
                setAddFormTouched((t) => ({ ...t, [field.name]: true }));
              }}
              onBlur={() =>
                setAddFormTouched((t) => ({ ...t, [field.name]: true }))
              }
              required
              placeholder={field.placeholder}
              type={field.type || "text"}
              {...field.inputProps}
              className={`input rounded-lg border-2 px-3 py-2 text-gray-800 dark:text-gray-100 ${addFormErrors[field.name as keyof TForm] && addFormTouched[field.name] ? "border-red-500 dark:border-red-400" : "border-gray-200 dark:border-gray-700"}`}
            />
            {addFormErrors[field.name as keyof TForm] &&
              addFormTouched[field.name] && (
                <span className="mt-1 text-xs text-red-500">
                  {addFormErrors[field.name as keyof TForm]}
                </span>
              )}
          </div>
        ))}
        <div className="col-span-1 flex justify-center sm:col-span-12">
          <ButtonSuccess
            text={addFormSubmitting ? "Dodawanie..." : addButtonText}
            type="submit"
            disabled={!isAddFormValid || addFormSubmitting}
          />
        </div>
      </form>

      {/* Search/filter bar */}
      <GenericSearchFilterBar
        search={search}
        onSearchChange={setSearch}
        filterValue={""}
        filterOptions={[]}
        onFilterChange={() => {}}
        showFilter={false}
      />

      {/* Table */}
      <div className="overflow-x-auto rounded-lg bg-white shadow dark:bg-gray-800">
        {loading ? (
          <div className="p-8 text-center text-gray-400">Ładowanie...</div>
        ) : error ? (
          <div className="p-8 text-center text-red-500">{error}</div>
        ) : (
          <table className="min-w-full text-sm">
            <thead>
              <tr className="bg-gray-100 dark:bg-gray-700">
                {fields.map((field) => (
                  <th key={String(field.name)} className="px-4 py-2 text-left">
                    {field.label}
                  </th>
                ))}
              </tr>
            </thead>
            <tbody>
              {paginated.length === 0 ? (
                <tr>
                  <td
                    colSpan={fields.length}
                    className="py-6 text-center text-gray-400"
                  >
                    Brak wyników
                  </td>
                </tr>
              ) : (
                paginated.map((entity) =>
                  renderTableRow(entity, () => openEditModal(entity)),
                )
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

      {/* Editing Modal */}
      <GenericModal
        open={modalOpen}
        title={editModalTitle}
        onClose={closeEditModal}
        actions={null}
      >
        {editForm && (
          <form
            className="flex flex-col gap-2"
            onSubmit={async (e) => {
              e.preventDefault();
              setEditFormTouched(
                Object.fromEntries(
                  Object.keys(editForm).map((k) => [k, true]),
                ) as Partial<Record<keyof TForm, boolean>>,
              );
              await handleEditSave();
            }}
          >
            {fields.map((field) => (
              <label
                key={String(field.name)}
                htmlFor={`edit-${String(field.name)}`}
              >
                <span className="flex flex-col text-sm font-medium text-gray-700 dark:text-gray-200">
                  {field.label}
                </span>
                <input
                  id={`edit-${String(field.name)}`}
                  className={`w-full rounded border-2 px-4 py-2 shadow-sm sm:text-sm dark:border-gray-600 dark:bg-gray-900 dark:text-white ${editFormErrors[field.name as keyof TForm] && editFormTouched[field.name] ? "border-red-500 dark:border-red-400" : "border-gray-300"}`}
                  value={editForm[field.name] as unknown as string}
                  onChange={(e) => {
                    setEditForm({ ...editForm, [field.name]: e.target.value });
                    setEditFormTouched((t) => ({ ...t, [field.name]: true }));
                  }}
                  onBlur={() =>
                    setEditFormTouched((t) => ({ ...t, [field.name]: true }))
                  }
                  placeholder={field.placeholder}
                  type={field.type || "text"}
                  {...field.inputProps}
                  required
                />
                {editFormErrors[field.name as keyof TForm] &&
                  editFormTouched[field.name] && (
                    <span className="mt-1 text-xs text-red-500">
                      {editFormErrors[field.name as keyof TForm]}
                    </span>
                  )}
              </label>
            ))}
            <div className="mt-4 flex flex-row justify-end gap-2">
              <div className="flex flex-1 justify-start">
                <ButtonError
                  text={editFormSubmitting ? "Usuwanie..." : "Usuń"}
                  type="button"
                  onClick={handleEditDelete}
                  disabled={editFormSubmitting}
                />
              </div>
              <div className="flex gap-2">
                <ButtonNeutral
                  text="Anuluj"
                  type="button"
                  onClick={closeEditModal}
                  disabled={editFormSubmitting}
                />
                <ButtonSuccess
                  text={editFormSubmitting ? "Zapisywanie..." : "Zapisz"}
                  type="submit"
                  disabled={editFormSubmitting || !isEditFormValid}
                />
              </div>
            </div>
          </form>
        )}
      </GenericModal>
    </div>
  );
}
