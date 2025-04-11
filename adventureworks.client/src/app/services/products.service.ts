import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

export type CountProductsParams = {
  search?: string
  category?: string | null
}
export type GetProductsParams = {
  page: number;
  length: number;
  sort: string;
  reverse: boolean;
} & CountProductsParams;

export interface ProductPartial {
  id: number;
  name: string;
  listPrice: string;
  imgUrl: string;
}

export type ProductCategory = {
  id: number;
  productSubcategoryID: number;
  categoryName: string;
  name: string;
}

export interface Product extends ProductPartial {

}

@Injectable({
  providedIn: 'root'
})
export class ProductsService {
  http: HttpClient = inject(HttpClient);

  getProducts(params: GetProductsParams): Observable<ProductPartial[]> {
    let queryParams = new HttpParams();
    if (params.search) queryParams = queryParams.append('search', params.search);
    if (params.category) queryParams = queryParams.append('category', params.category);
    const { page, length, sort, reverse } = params;
    queryParams = queryParams.appendAll({ page, length, sort, reverse });

    return this.http.get<ProductPartial[]>('/api/Products', {
      params: queryParams,
      headers: {
        'Accept': 'application/json'
      }
    });
  }

  getProductCategories(): Observable<ProductCategory[]> {
    return this.http.get<ProductCategory[]>('/api/ProductCategories')
  }

}
