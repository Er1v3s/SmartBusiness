import type { FieldConfig } from "./GenericEntitySection";

// --- Service ---
export type ServiceForm = {
  name: string;
  description: string;
  category: string;
  price: string;
  vat: string;
  duration: string;
};

export const serviceFields: FieldConfig<ServiceForm>[] = [
  { name: "name", label: "Nazwa usługi", placeholder: "Nazwa usługi" },
  { name: "description", label: "Opis", placeholder: "Opis" },
  { name: "category", label: "Kategoria", placeholder: "Kategoria" },
  { name: "duration", label: "Czas trwania (min)", placeholder: "Czas", type: "number", inputProps: { min: 1, step: 1 } },
  { name: "price", label: "Cena", placeholder: "Cena", type: "number", inputProps: { min: 0, step: 0.01 } },
  { name: "vat", label: "VAT", placeholder: "VAT", type: "number", inputProps: { min: 0, step: 1 } },
];

// --- Product ---
export type ProductForm = {
  name: string;
  description: string;
  category: string;
  price: string;
  vat: string;
};

export const productFields: FieldConfig<ProductForm>[] = [
  { name: "name", label: "Nazwa produktu", placeholder: "Nazwa produktu" },
  { name: "description", label: "Opis", placeholder: "Opis" },
  { name: "category", label: "Kategoria", placeholder: "Kategoria" },
  { name: "price", label: "Cena", placeholder: "Cena", type: "number", inputProps: { min: 0, step: 0.01 } },
  { name: "vat", label: "VAT", placeholder: "VAT", type: "number", inputProps: { min: 0, step: 1 } },
];
