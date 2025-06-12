import { useService } from "../../../context/service/ServiceContext";
import { useAlert } from "../../../context/alert/useAlert";
import type { Service } from "../../../models/service";
import GenericEntitySection from "./ProductServicesSection/GenericEntitySection";
import { serviceFields } from "./ProductServicesSection/fieldsConfig";
import type { ServiceForm } from "./ProductServicesSection/fieldsConfig";

// --- Validation service form ---
export type ServiceFormErrors = {
  name?: string;
  description?: string;
  category?: string;
  price?: string;
  vat?: string;
  duration?: string;
};

const validateServiceForm = (form: ServiceForm): ServiceFormErrors => {
  const errors: ServiceFormErrors = {};
  if (!form.name) errors.name = "Nazwa usługi jest wymagana.";
  else if (form.name.length < 3)
    errors.name = "Nazwa usługi musi mieć co najmniej 3 znaki.";
  else if (form.name.length > 100)
    errors.name = "Nazwa usługi nie może być dłuższa niż 100 znaków.";

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

  if (!form.duration) errors.duration = "Czas trwania jest wymagany.";
  else if (!/^\d+$/.test(form.duration))
    errors.duration = "Czas trwania musi być liczbą całkowitą.";
  else if (Number(form.duration) < 1)
    errors.duration = "Czas trwania musi być większy niż 0.";
  else if (Number(form.duration) > 600)
    errors.duration = "Czas trwania nie może być dłuższy niż 600 minut.";

  return errors;
};

const isServiceFormValid = (form: ServiceForm, errors: ServiceFormErrors) => {
  return (
    Object.values(errors).every((e) => !e) &&
    Object.values(form).every((v) => v !== "")
  );
};

const initialServiceForm: ServiceForm = {
  name: "",
  description: "",
  category: "",
  price: "",
  vat: "",
  duration: "",
};

const formToService = (form: ServiceForm) => ({
  name: form.name,
  description: form.description,
  category: form.category,
  price: parseFloat(form.price),
  tax: parseInt(form.vat, 10),
  duration: parseInt(form.duration, 10),
});

const serviceToForm = (service: Service): ServiceForm => ({
  name: service.name,
  description: service.description,
  category: service.category,
  price: service.price.toString(),
  vat: service.tax.toString(),
  duration: service.duration?.toString() || "",
});

const renderServiceRow = (service: Service, onEdit: () => void) => (
  <tr
    key={service.id}
    className="cursor-pointer border-b border-gray-100 transition hover:bg-indigo-50/40 dark:border-gray-700 dark:hover:bg-gray-700/40"
    onClick={onEdit}
  >
    <td className="px-4 py-2 text-left font-medium">{service.name}</td>
    <td className="px-4 py-2 text-left">{service.description}</td>
    <td className="px-4 py-2 text-left">{service.category}</td>
    <td className="px-4 py-2 text-left">{service.duration}</td>
    <td className="px-4 py-2 text-left">{service.price.toFixed(2)} zł</td>
    <td className="px-4 py-2 text-left">{service.tax}%</td>
  </tr>
);

export const ServicesSection = () => {
  const { fetchServices, createService, updateService, deleteService } =
    useService();
  const { showAlert } = useAlert();

  return (
    <GenericEntitySection
      title="Usługi"
      fetchEntities={async () => fetchServices({})}
      createEntity={async (form) => {
        await createService(formToService(form));
        showAlert({
          title: "Dodano",
          message: "Usługa została dodana.",
          type: "success",
          duration: 3000,
        });
      }}
      updateEntity={async (id, form) => {
        await updateService({ id, ...formToService(form) });
        showAlert({
          title: "Zaktualizowano",
          message: "Usługa została zaktualizowana.",
          type: "success",
          duration: 3000,
        });
      }}
      deleteEntity={async (id) => {
        await deleteService(id);
        showAlert({
          title: "Usunięto",
          message: "Usługa została usunięta.",
          type: "success",
          duration: 3000,
        });
      }}
      fields={serviceFields}
      formToEntity={formToService}
      entityToForm={serviceToForm}
      renderTableRow={renderServiceRow}
      getEntityId={(s) => s.id}
      initialForm={initialServiceForm}
      validateForm={validateServiceForm}
      isFormValid={isServiceFormValid}
      editModalTitle="Edytuj usługę"
      addButtonText="Dodaj usługę"
    />
  );
};

export default ServicesSection;
