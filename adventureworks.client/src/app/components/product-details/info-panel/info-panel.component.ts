import { Component, input, output, signal } from '@angular/core';
import { Product } from '../../../services/products.service';
import { CurrencyPipe } from '@angular/common';

export class AddToCartEvent extends MouseEvent{
  productId?: number;
  qty: number = 1; // Default quantity
  constructor(type: string, options: MouseEventInit) {
    super(type, options);
  }

  static FromMouseEvent(e: MouseEvent) {
    return e as AddToCartEvent;
  }
}

@Component({
  selector: 'app-info-panel',
  standalone: true,
  templateUrl: './info-panel.component.html',
  styleUrl: './info-panel.component.scss',
  imports: [CurrencyPipe]
})
export class InfoPanelComponent {
  public product = input.required<Product>();
  public readonly onAddToCart = output<AddToCartEvent>();
  public orderQty = signal<number>(1); // Default quantity

  protected onButtonClick(event: MouseEvent): void {
    var e = AddToCartEvent.FromMouseEvent(event);
    e.qty = this.orderQty();
    e.productId = this.product().id;
    this.onAddToCart.emit(e);
  }

  protected setQty(e: Event): void {
    if (!e.target) return;
    var input = e.target as HTMLInputElement;
    this.orderQty.set(input.valueAsNumber);
  }
}
