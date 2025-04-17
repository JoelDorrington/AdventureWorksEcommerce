import { Injectable, inject, signal } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { CartItem } from '@models/cart-item.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private cartId: string = "";
  public cartItems = signal<CartItem[]>([]);
  private http = inject(HttpClient);

  constructor() {
    let cartId = localStorage.getItem('shopping-cart-id');
    if (!cartId) {
      let bytes = new Uint8Array(25);
      crypto.getRandomValues(bytes);
      cartId = btoa(String.fromCharCode(...bytes));

      localStorage.setItem('shopping-cart-id', cartId);
    }
    if(cartId == null) throw new Error('Unable to generate cartId')
    this.cartId = cartId;
  }

  public setItemQuantity(productId: number, qty: number): Observable<CartItem> {
    return this.http.put<CartItem>(`/api/shoppingcart/${encodeURIComponent(this.cartId)}/${productId}/${qty}`, null)
  }

  public removeItemFromCart(productId: number): Observable<void> {
    return this.http.delete<void>(`/api/shoppingcart/${encodeURIComponent(this.cartId)}/${productId}`);
  }

  public getCartItems(): Observable<CartItem[]> {
    return this.http.get<CartItem[]>(`/api/shoppingcart/${encodeURIComponent(this.cartId)}`)
      .pipe(
        tap((items:CartItem[]) => {
          this.cartItems.set(items);
          console.log(this.cartItems());
        })
      );
  }

  /**
   * TODO: (Add To Cart)
   * - Get cart number if none stored (unique string MAX 50 char)
   * - Store it in local storage
   * - Create ShoppingCartItem POST api
   * - Create shopping cart item read api
   * - create cart page
   */
}
