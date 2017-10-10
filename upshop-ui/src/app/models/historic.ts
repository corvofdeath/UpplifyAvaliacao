import { ProductDetail } from './productDetail';

export class Historic {
  id: string;
  description: string;
  total: number;
  dateTransaction: string;
  products: ProductDetail[];
}
