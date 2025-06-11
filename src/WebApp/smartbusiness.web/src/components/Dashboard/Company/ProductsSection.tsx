import { useProduct } from "../../../context/product/ProductContext";
import { useAlert } from "../../../context/alert/useAlert";
import type { Product } from "../../../models/product";
import GenericEntitySection from "./ProductServicesSection/GenericEntitySection";
import { productFields } from "./ProductServicesSection/fieldsConfig";
import type { ProductForm } from "./ProductServicesSection/fieldsConfig";

// --- Validation product form ---
export type ProductFormErrors = {
  name?: string;
  description?: string;
  category?: string;
  price?: string;
  vat?: string;
};

const validateProductForm = (form: ProductForm): ProductFormErrors => {
  const errors: ProductFormErrors = {};
  if (!form.name) errors.name = "Nazwa produktu jest wymagana.";
  else if (form.name.length < 3)
    errors.name = "Nazwa produktu musi mieć co najmniej 3 znaki.";
  else if (form.name.length > 100)
    errors.name = "Nazwa produktu nie może być dłuższa niż 100 znaków.";

  if (!form.description) errors.description = "Opis jest wymagany.";
  else if (form.description.length < 3)
    errors.description = "Opis musi mieć co najmniej 3 znaki.";
  else if (form.description.length > 500)
    errors.description = "Opis nie może być dłuższy niż 500 znaków.";

  if (!form.category) errors.category = "Kategoria jest wymagana.";
  else if (form.category.length < 3)
    errors.category = "Kategoria musi mieć co najmniej 3 znaki.";
  else if (form.category.length > 50)
    errors.category = "Kategoria nie może być dłuższa niż 50 znaków.";

  if (!form.price) errors.price = "Cena jest wymagana.";
  else if (isNaN(Number(form.price))) errors.price = "Cena musi być liczbą.";
  else if (Number(form.price) <= 0)
    errors.price = "Cena musi być większa niż 0.";
  else if (!/^\d{1,7}(\.\d{1,2})?$/.test(form.price))
    errors.price =
      "Cena może mieć maksymalnie 7 cyfr przed przecinkiem i 2 po przecinku.";

  if (!form.vat) errors.vat = "VAT jest wymagany.";
  else if (!/^\d+$/.test(form.vat))
    errors.vat = "VAT musi być liczbą całkowitą.";
  else if (Number(form.vat) < 0 || Number(form.vat) > 100)
    errors.vat = "VAT musi być w zakresie 0-100.";

  return errors;
};

const isProductFormValid = (form: ProductForm, errors: ProductFormErrors) => {
  return (
    Object.values(errors).every((e) => !e) &&
    Object.values(form).every((v) => v !== "")
  );
};

const initialProductForm: ProductForm = {
  name: "",
  description: "",
  category: "",
  price: "",
  vat: "",
};

const formToProduct = (form: ProductForm) => ({
  name: form.name,
  description: form.description,
  category: form.category,
  price: parseFloat(form.price),
  tax: parseInt(form.vat, 10),
});

const productToForm = (product: Product): ProductForm => ({
  name: product.name,
  description: product.description,
  category: product.category,
  price: product.price.toString(),
  vat: product.tax.toString(),
});

const renderProductRow = (product: Product, onEdit: () => void) => (
  <tr
    key={product.id}
    className="cursor-pointer border-b border-gray-100 transition hover:bg-indigo-50/40 dark:border-gray-700 dark:hover:bg-gray-700/40"
    onClick={onEdit}
  >
    <td className="px-4 py-2 font-medium">{product.name}</td>
    <td className="px-4 py-2">{product.description}</td>
    <td className="px-4 py-2">{product.category}</td>
    <td className="px-4 py-2 text-right">{product.price.toFixed(2)} zł</td>
    <td className="px-4 py-2 text-right">{product.tax}%</td>
  </tr>
);

export const ProductsSection = () => {
  const { fetchProducts, createProduct, updateProduct, deleteProduct } =
    useProduct();
  const { showAlert } = useAlert();

  return (
    <GenericEntitySection
      title="Produkty"
      fetchEntities={async () => fetchProducts({})}
      createEntity={async (form) => {
        await createProduct(formToProduct(form));
        showAlert({
          title: "Dodano",
          message: "Produkt został dodany.",
          type: "success",
          duration: 3000,
        });
      }}
      updateEntity={async (id, form) => {
        await updateProduct({ id, ...formToProduct(form) });
        showAlert({
          title: "Zaktualizowano",
          message: "Produkt został zaktualizowany.",
          type: "success",
          duration: 3000,
        });
      }}
      deleteEntity={async (id) => {
        await deleteProduct(id);
        showAlert({
          title: "Usunięto",
          message: "Produkt został usunięty.",
          type: "success",
          duration: 3000,
        });
      }}
      fields={productFields}
      formToEntity={formToProduct}
      entityToForm={productToForm}
      renderTableRow={renderProductRow}
      getEntityId={(p) => p.id}
      initialForm={initialProductForm}
      validateForm={validateProductForm}
      isFormValid={isProductFormValid}
      editModalTitle="Edytuj produkt"
      addButtonText="Dodaj produkt"
    />
  );
};

export default ProductsSection;
