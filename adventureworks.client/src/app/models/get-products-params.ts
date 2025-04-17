import {CountProductsParams} from '@models/count-products-params';

export type GetProductsParams = {
  page: number;
  length: number;
  sort: string;
  reverse: boolean;
} & CountProductsParams;
