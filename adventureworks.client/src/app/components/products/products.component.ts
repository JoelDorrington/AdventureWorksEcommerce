import { Component, signal, OnInit, inject } from '@angular/core';
import { ProductsService, ProductPartial } from '../../services/products.service';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CurrencyPipe } from '@angular/common';

@Component({
  selector: 'app-products',
  standalone: true,
  templateUrl: './products.component.html',
  styleUrl: './products.component.scss',
  imports: [RouterLink, CurrencyPipe]
})

export class ProductsComponent implements OnInit {
  products = signal<ProductPartial[]>([]);
  categoryId: string | null = null;
  pageNumber: number = 1;
  pageLength: number = 20;
  sort: string = "price";
  reverse: boolean = true;
  search: string = "";

  constructor(private route: ActivatedRoute, private productsService: ProductsService) { }

  ngOnInit(): void {
    // Retrieve the 'category' query parameter
    this.route.queryParamMap.subscribe(params => {
      this.categoryId = params.get('category');
      this.fetchProducts();
    });
  }

  fetchProducts(): void {
    this.productsService.getProducts({
      category: this.categoryId,
      search: this.search,
      page: this.pageNumber,
      length: this.pageLength,
      sort: this.sort,
      reverse: this.reverse
    }).subscribe(response => {
      this.products.set(response);
    });
  }
  
}
