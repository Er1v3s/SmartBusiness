import React, { useEffect, useState } from "react";
import { useTransaction } from "../../../context/transaction/TransactionContext";
import { useProduct } from "../../../context/product/ProductContext";
import { useService } from "../../../context/service/ServiceContext";
import type { Product } from "../../../models/product";
import type { Service } from "../../../models/service";
import { ButtonSuccess } from "../../General/Buttons";
import type { ApiResponseError } from "../../../models/authErrors";
import { useAlert } from "../../../context/alert/useAlert";

export const RegisterSaleSection = () => {
  const { createTransaction } = useTransaction();
  const { fetchProducts } = useProduct();
  const { fetchServices } = useService();
  const { showAlert } = useAlert();

  // Product sale form state
  const [products, setProducts] = useState<Product[]>([]);
  const [productId, setProductId] = useState("");
  const [productQuantity, setProductQuantity] = useState(1);
  const [productTax, setProductTax] = useState(0);
  const [productTotalAmount, setProductTotalAmount] = useState(0);
  const [productLoading, setProductLoading] = useState(false);

  // Service sale form state
  const [services, setServices] = useState<Service[]>([]);
  const [serviceId, setServiceId] = useState("");
  const [serviceQuantity, setServiceQuantity] = useState(1);
  const [serviceTax, setServiceTax] = useState(0);
  const [serviceTotalAmount, setServiceTotalAmount] = useState(0);
  const [serviceLoading, setServiceLoading] = useState(false);

  useEffect(() => {
    fetchProducts({}).then(setProducts);
    fetchServices({}).then(setServices);
  }, [fetchProducts, fetchServices]);

  // Nowe: automatyczne ustawianie tax i price po wyborze produktu/usługi
  useEffect(() => {
    const selected = products.find((p) => p.id === productId);
    if (selected) {
      setProductTax(selected.tax);
      // Zaokrąglanie w górę do 2 miejsc po przecinku i formatowanie na string z dwoma miejscami
      const total = Math.ceil(selected.price * productQuantity * 100) / 100;
      setProductTotalAmount(Number(total.toFixed(2)));
    } else {
      setProductTax(0);
      setProductTotalAmount(0);
    }
  }, [productId, productQuantity, products]);

  useEffect(() => {
    const selected = products.find((p) => p.id === productId);
    if (selected) {
      const total = Math.ceil(selected.price * productQuantity * 100) / 100;
      setProductTotalAmount(Number(total.toFixed(2)));
    }
  }, [productQuantity, productId, products]);

  useEffect(() => {
    const selected = services.find((s) => s.id === serviceId);
    if (selected) {
      setServiceTax(selected.tax);
      const total = Math.ceil(selected.price * serviceQuantity * 100) / 100;
      setServiceTotalAmount(Number(total.toFixed(2)));
    } else {
      setServiceTax(0);
      setServiceTotalAmount(0);
    }
  }, [serviceId, serviceQuantity, services]);

  useEffect(() => {
    const selected = services.find((s) => s.id === serviceId);
    if (selected) {
      const total = Math.ceil(selected.price * serviceQuantity * 100) / 100;
      setServiceTotalAmount(Number(total.toFixed(2)));
    }
  }, [serviceQuantity, serviceId, services]);

  // Form validation
  const isProductFormValid =
    !!productId && productQuantity > 0 && productTotalAmount > 0;
  const isServiceFormValid =
    !!serviceId && serviceQuantity > 0 && serviceTotalAmount > 0;

  const handleProductSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!isProductFormValid) return;
    setProductLoading(true);
    try {
      await createTransaction({
        productId,
        quantity: productQuantity,
        totalAmount: productTotalAmount,
        tax: productTax,
      });

      showAlert({
        title: "Sukces",
        message: "Sprzedaż produktu została zarejestrowana!",
        type: "success",
        duration: 3000,
      });
      setProductId("");
      setProductQuantity(1);
      setProductTax(0);
      setProductTotalAmount(0);
    } catch (err) {
      const error = err as ApiResponseError;

      showAlert({
        title: error?.title || "Błąd",
        message:
          error.detail ||
          "Wystąpił błąd podczas rejestracji sprzedaży produktu.",
        type: "error",
        duration: 3000,
      });
    } finally {
      setProductLoading(false);
    }
  };

  const handleServiceSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!isServiceFormValid) return;
    setServiceLoading(true);
    try {
      await createTransaction({
        productId: serviceId, // for service, we use productId field as well
        quantity: serviceQuantity,
        totalAmount: serviceTotalAmount,
        tax: serviceTax,
      });

      showAlert({
        title: "Sukces",
        message: "Sprzedaż usługi została zarejestrowana!",
        type: "success",
        duration: 3000,
      });
      setServiceId("");
      setServiceQuantity(1);
      setServiceTax(0);
      setServiceTotalAmount(0);
    } catch (err) {
      const error = err as ApiResponseError;

      showAlert({
        title: error?.title || "Błąd",
        message:
          error.detail || "Wystąpił błąd podczas rejestracji sprzedaży usługi.",
        type: "error",
        duration: 3000,
      });
    } finally {
      setServiceLoading(false);
    }
  };

  return (
    <div className="flex h-full w-full flex-col gap-8 md:flex-row">
      {/* Product Sale Form */}
      <div className="flex-1 rounded-lg bg-white p-6 shadow transition-colors dark:bg-gray-800 dark:shadow-lg">
        <h2 className="mb-4 text-xl font-semibold text-gray-800 dark:text-gray-100">
          Rejestracja sprzedaży produktu
        </h2>
        <form onSubmit={handleProductSubmit} className="flex flex-col gap-4">
          <label className="flex flex-col text-gray-800 dark:text-gray-200">
            Produkt:
            <select
              value={productId}
              onChange={(e) => setProductId(e.target.value)}
              required
              className="mt-1 rounded-lg border-2 border-gray-300 bg-white p-2 text-gray-800 transition-colors dark:border-gray-600 dark:bg-gray-800 dark:text-gray-100"
            >
              <option value="">Wybierz produkt</option>
              {products.map((p) => (
                <option
                  key={p.id}
                  value={p.id}
                  className="bg-white text-gray-800 dark:bg-gray-800 dark:text-gray-100"
                >
                  {p.name}
                </option>
              ))}
            </select>
          </label>
          <label className="flex flex-col text-gray-800 dark:text-gray-200">
            Ilość:
            <input
              type="number"
              min={1}
              value={productQuantity}
              onChange={(e) => setProductQuantity(Number(e.target.value))}
              required
              className="mt-1 rounded-lg border-2 border-gray-300 bg-white p-2 text-gray-800 transition-colors dark:border-gray-600 dark:bg-gray-800 dark:text-gray-100"
            />
          </label>
          <label className="flex flex-col text-gray-800 dark:text-gray-200">
            Podatek (%):
            <input
              type="number"
              min={0}
              value={productTax}
              readOnly
              className="mt-1 cursor-not-allowed rounded-lg border-2 border-gray-300 bg-white p-2 text-gray-800 opacity-80 transition-colors dark:border-gray-600 dark:bg-gray-800 dark:text-gray-100"
            />
          </label>
          <label className="flex flex-col text-gray-800 dark:text-gray-200">
            Kwota całkowita:
            <input
              type="text"
              min={0}
              value={productTotalAmount.toFixed(2)}
              readOnly
              className="mt-1 cursor-not-allowed rounded-lg border-2 border-gray-300 bg-white p-2 text-gray-800 opacity-80 transition-colors dark:border-gray-600 dark:bg-gray-800 dark:text-gray-100"
            />
          </label>
          <ButtonSuccess
            text={
              productLoading ? "Rejestruję..." : "Zarejestruj sprzedaż produktu"
            }
            type="submit"
            disabled={productLoading || !isProductFormValid}
          />
        </form>
      </div>

      {/* Service Sale Form */}
      <div className="flex-1 rounded-lg bg-white p-6 shadow transition-colors dark:bg-gray-800 dark:shadow-lg">
        <h2 className="mb-4 text-xl font-semibold text-gray-800 dark:text-gray-100">
          Rejestracja sprzedaży usługi
        </h2>
        <form onSubmit={handleServiceSubmit} className="flex flex-col gap-4">
          <label className="flex flex-col text-gray-800 dark:text-gray-200">
            Usługa:
            <select
              value={serviceId}
              onChange={(e) => setServiceId(e.target.value)}
              required
              className="mt-1 rounded-lg border-2 border-gray-300 bg-white p-2 text-gray-800 transition-colors dark:border-gray-600 dark:bg-gray-800 dark:text-gray-100"
            >
              <option value="">Wybierz usługę</option>
              {services.map((s) => (
                <option
                  key={s.id}
                  value={s.id}
                  className="bg-white text-gray-800 dark:bg-gray-800 dark:text-gray-100"
                >
                  {s.name}
                </option>
              ))}
            </select>
          </label>
          <label className="flex flex-col text-gray-800 dark:text-gray-200">
            Ilość:
            <input
              type="number"
              min={1}
              value={serviceQuantity}
              onChange={(e) => setServiceQuantity(Number(e.target.value))}
              required
              className="mt-1 rounded-lg border-2 border-gray-300 bg-white p-2 text-gray-800 transition-colors dark:border-gray-600 dark:bg-gray-800 dark:text-gray-100"
            />
          </label>
          <label className="flex flex-col text-gray-800 dark:text-gray-200">
            Podatek (%):
            <input
              type="number"
              min={0}
              value={serviceTax}
              readOnly
              className="mt-1 cursor-not-allowed rounded-lg border-2 border-gray-300 bg-white p-2 text-gray-800 opacity-80 transition-colors dark:border-gray-600 dark:bg-gray-800 dark:text-gray-100"
            />
          </label>
          <label className="flex flex-col text-gray-800 dark:text-gray-200">
            Kwota całkowita:
            <input
              type="text"
              min={0}
              value={serviceTotalAmount.toFixed(2)}
              readOnly
              className="mt-1 cursor-not-allowed rounded-lg border-2 border-gray-300 bg-white p-2 text-gray-800 opacity-80 transition-colors dark:border-gray-600 dark:bg-gray-800 dark:text-gray-100"
            />
          </label>
          <ButtonSuccess
            text={
              serviceLoading ? "Rejestruję..." : "Zarejestruj sprzedaż usługi"
            }
            type="submit"
            disabled={serviceLoading || !isServiceFormValid}
          />
        </form>
      </div>
    </div>
  );
};
