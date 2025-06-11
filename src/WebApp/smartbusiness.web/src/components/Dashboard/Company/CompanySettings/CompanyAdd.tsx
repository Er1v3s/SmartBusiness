import React, { useState } from "react";
import { useCompany } from "../../../../context/company/CompanyContext";
import { ButtonSuccess } from "../../../General/Buttons";
import { useAlert } from "../../../../context/alert/useAlert";
import type { ApiResponseError } from "../../../../models/authErrors";
import { useAuth } from "../../../../context/auth/AuthContext";

export const CompanyAdd: React.FC = () => {
  const { createCompany, fetchCompanies } = useCompany();
  const { showAlert } = useAlert();
  const { loginUsingRefreshToken } = useAuth();
  const [name, setName] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  // Validation rules for company name
  const validateName = (value: string): string | null => {
    if (value == null || value.trim() === "") {
      return "Nazwa firmy jest wymagana.";
    }
    if (value.length < 3) {
      return "Nazwa firmy musi mieć co najmniej 3 znaki.";
    }
    if (value.length > 100) {
      return "Nazwa firmy nie może być dłuższa niż 100 znaków.";
    }
    if (!/^[a-zA-Z0-9 _\-()]*$/.test(value)) {
      return "Nazwa firmy może zawierać tylko litery, cyfry, spację, podkreślenie (_), myślnik (-) i nawiasy ().";
    }
    return null;
  };

  // Dynamic validation on input change
  const handleNameChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;
    setName(value);
    const validationError = validateName(value);
    setError(validationError);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    const validationError = validateName(name);
    if (validationError) {
      setError(validationError);
      return;
    }
    setLoading(true);
    try {
      await createCompany(name);
      await loginUsingRefreshToken();
      await fetchCompanies();

      showAlert({
        title: "Sukces",
        message: "Firma została dodana!",
        type: "success",
        duration: 5000,
      });
      setName("");
    } catch (err) {
      const error = err as ApiResponseError;

      showAlert({
        title: error.title || "Błąd",
        message: error.detail || "Nie udało się dodać firmy.",
        type: "error",
        duration: 5000,
      });
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="space-y-4">
      <h2 className="text-2xl font-bold text-gray-800 dark:text-gray-100">
        Dodaj nową firmę
      </h2>
      <div className="rounded-lg border-2 border-gray-100 bg-white p-6 shadow-xl dark:border-gray-700 dark:bg-gray-800">
        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label className="block text-gray-700 dark:text-gray-200">
              Nazwa firmy
            </label>
            <input
              className="mt-1 w-full rounded border p-2 dark:bg-gray-800 dark:text-gray-100"
              value={name}
              onChange={handleNameChange}
              required
              minLength={3}
              maxLength={100}
            />
            {error && (
              <p className="mt-2 text-sm text-red-600 dark:text-red-400">
                {error}
              </p>
            )}
          </div>
          <ButtonSuccess
            text={loading ? "Dodawanie..." : "Stwórz firmę"}
            type="submit"
            disabled={loading || !!error}
          />
        </form>
      </div>
    </div>
  );
};
