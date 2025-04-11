import { Component, signal, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ProductCategory, ProductsService } from '../../services/products.service';
import { JsonPipe, NgIf } from '@angular/common';

@Component({
  selector: 'app-home',
  standalone: true,
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
  imports: [RouterLink, NgIf, JsonPipe]
})
export class HomeComponent {
  categories = signal([] as ProductCategory[]);
  _productsService = inject(ProductsService);

  getCategories() {
    this._productsService.getProductCategories()
      .subscribe(response => {
        this.categories.set(response);
      })
  }

  getCategoryLink(id: number) {
    return `/products?category=${id}`
  }

  ngOnInit() {
    this.getCategories();
  }
}
