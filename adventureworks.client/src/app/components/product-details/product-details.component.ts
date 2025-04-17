import { Component, OnInit, signal, Type } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { Product, ProductsService } from '../../services/products.service';
import { InfoPanelComponent, AddToCartEvent } from './info-panel/info-panel.component';
import { CartService } from '../../services/cart.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-product-details',
  standalone: true,
  templateUrl: './product-details.component.html',
  styleUrl: './product-details.component.scss',
  imports: [RouterModule, InfoPanelComponent]
})
export class ProductDetailsComponent implements OnInit {
  private productID: number = NaN;
  public product = signal<Product>({
    id: 0,
    name: "",
    description: "",
    productNumber: "",
    color: "",
    size: "",
    listPrice: "",
    imgUrl: ""
  });
  public orderQty = signal(1); // Signal to track order quantity

  constructor(
    private activeRoute: ActivatedRoute,
    private productsService: ProductsService,
    private cartService: CartService,
    private toastr: ToastrService
  ) { }

  ngOnInit() {
    this.activeRoute.params.subscribe(params => {
      this.productID = +params['id']; // Get the product ID from the URL
      if (!isNaN(this.productID)) {
        this.productsService.getProductByID(this.productID).subscribe(({data}) => {
          this.product.set(data); // Fetch product details
        });
      }
    });
  }

  addToCart(event: AddToCartEvent): void {
    if (!event.productId) return;
    this.cartService.setItemQuantity(event.productId, event.qty)
      .subscribe({
        next: cartItem => {
          this.toastr.success(`Added to cart`, 'Success');
        },
        error: err => {
          console.error(err);
          this.toastr.error('Failed to add product to cart. Please try again.', 'Error');
        }
      });
  }
}
