import { ProductContext } from "./ProductContext.tsx";
import apiSalesConnector from "../../api/apiSalesConnector.ts";
import type {
  GetProductsByParamsQuery,
  Product,
  ProductContextType,
  NewProduct,
} from "../../models/index.ts";

export const ProductProvider: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {
  const createProduct = async (data: NewProduct): Promise<void> => {
    await apiSalesConnector.createProduct(data);
  };

  const updateProduct = async (data: Partial<Product>): Promise<void> => {
    await apiSalesConnector.updateProduct(data);
  };

  const deleteProduct = async (productId: string): Promise<void> => {
    await apiSalesConnector.deleteProduct(productId);
  };

  const fetchProduct = async (productId: string): Promise<Product | null> => {
    const products = await apiSalesConnector.getProductById(productId);

    return products;
  };

  const fetchProducts = async (
    params: GetProductsByParamsQuery,
  ): Promise<Product[]> => {
    const products = await apiSalesConnector.getProducts(params);

    return products;
  };

  // Context value
  const value: ProductContextType = {
    createProduct,
    updateProduct,
    deleteProduct,
    fetchProduct,
    fetchProducts,
  };

  return (
    <ProductContext.Provider value={value}>{children}</ProductContext.Provider>
  );
};
