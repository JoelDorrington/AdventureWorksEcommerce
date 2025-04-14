import { Component, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { Product, ProductsService } from '../../services/products.service';
import { CurrencyPipe } from '@angular/common';

@Component({
  selector: 'app-product-details',
  standalone: true,
  templateUrl: './product-details.component.html',
  styleUrl: './product-details.component.scss',
  imports: [RouterModule, CurrencyPipe]
})
export class ProductDetailsComponent implements OnInit {
  private productID: number = NaN;
  public product = signal<Product|null>(null);

  constructor(private activeRoute: ActivatedRoute, private productsService: ProductsService) { }

  ngOnInit() {
    this.activeRoute.params.subscribe(params => {
      this.productID = +params['id']; // Get the product ID from the URL
      if (!isNaN(this.productID)) {
        this.productsService.getProductByID(this.productID).subscribe(({data}) => {
          this.product.set(data); // Fetch product details
          console.log(data);
        });
      }
    });
  }
}
