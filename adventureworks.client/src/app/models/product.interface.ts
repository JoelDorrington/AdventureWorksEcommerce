import {ProductPartial} from '@models/product-partial.interface';

export interface Product extends ProductPartial {
  description: string;
  productNumber: string;
  color: string;
  size: string;
}
