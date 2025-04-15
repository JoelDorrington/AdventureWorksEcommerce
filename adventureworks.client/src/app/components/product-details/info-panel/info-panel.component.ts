import { Component, input, output } from '@angular/core';
import { Product } from '../../../services/products.service';
import { CurrencyPipe } from '@angular/common';

export class AddToCartEvent extends MouseEvent{
  productId?: number;
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

  protected onButtonClick(event: MouseEvent): void {
    var e = AddToCartEvent.FromMouseEvent(event);
    e.productId = this.product().id;
    this.onAddToCart.emit(e);
  }
}
