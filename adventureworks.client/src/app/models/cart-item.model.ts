export type CartItem = {
  cartId: string;
  productID: number;
  product: { name: string, listPrice: number }
  quantity: number;
  createdDate: Date;
}
