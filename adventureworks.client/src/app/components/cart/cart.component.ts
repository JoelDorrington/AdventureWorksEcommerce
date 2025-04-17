import { Component, computed, OnInit, signal } from '@angular/core';
import { CartService } from '../../services/cart.service';
import { CartItem } from '../../models/cart-item.model';
import { CurrencyPipe } from '@angular/common';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CurrencyPipe],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.scss'
})
export class CartComponent implements OnInit {
  protected cartItems = signal<CartItem[]>([]);
  protected disableForm = signal<boolean>(false);

  protected subtotal = computed<number>(() => this.cartItems().reduce(
    (sum: number, item: CartItem) => sum + item.product.listPrice * item.quantity,
    0
  ))

  constructor(private cartService: CartService, private toastr: ToastrService) { }

  ngOnInit() {
    this.cartService.getCartItems().subscribe(cartItems => {
      this.cartItems.set(cartItems);
    });
  }

  removeFromCart(productId: number) {
    this.disableForm.set(true);
    this.cartService.removeItemFromCart(productId).subscribe({
      next: () => {
        this.cartItems.set(this.cartItems().filter(item => item.productID !== productId));
        this.disableForm.set(false);
        this.toastr.success("Item removed.")
      },
      error: (err: Error) => {
        this.disableForm.set(false);
        console.error(err);
        this.toastr.error("Unable to remove item.")
      },
    });
  }
}
