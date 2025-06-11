import { createContext, useContext } from "react";
import type { ProductContextType } from "../../models/product.ts";

export const ProductContext = createContext<ProductContextType | undefined>(
  undefined,
);

export const useProduct = () => {
  const context = useContext(ProductContext);
  if (context === undefined) {
    throw new Error("useProduct must be used within a ProductProvider");
  }
  return context;
};
