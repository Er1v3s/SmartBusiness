export type ApiResponseValidationError = {
  property: string;
  errorMessage: string;
};

export type ApiResponseError = {
  title: string;
  status: string;
  detail: string;
  errors?: ApiResponseValidationError[] | null;
};

