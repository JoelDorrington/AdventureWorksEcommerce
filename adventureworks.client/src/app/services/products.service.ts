import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, of, tap } from 'rxjs';

export type CountProductsParams = {
  search?: string
  category?: number | null
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
  description: string;
  productNumber: string;
  color: string;
  size: string;
}

@Injectable({
  providedIn: 'root',
})
export class ProductsService {
  http: HttpClient = inject(HttpClient);
  private categoriesCache: ProductCategory[] | null = null; // Cache for product categories

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

  getProductByID(id: number): Observable<{data:Product}> {
    return this.http.get<{data:Product}>(`/api/Products/${id}`);
  }

  getProductCategories(): Observable<ProductCategory[]> {
    // If categories are already cached, return them as an Observable
    if (this.categoriesCache) {
      return of(this.categoriesCache);
    }

    return this.http.get<ProductCategory[]>('/api/ProductCategories').pipe(
      tap(categories => {
        this.categoriesCache = categories; // Cache the categories
      })
    )
  }

}
