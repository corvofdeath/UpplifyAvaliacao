import { ProductDetail } from './productDetail';

export class ShoppingCart {
  userId: string;
  products: ProductDetail[];
  description: string;
  total: number;
}
