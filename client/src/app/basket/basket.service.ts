import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Basket, BasketItem, BasketTotals } from '../shared/models/basket';
import { HttpClient } from '@angular/common/http';
import { Product } from '../shared/models/product';
import { DeliveryMethod } from '../shared/models/deliveryMethod';
import { ShopRoutingModule } from '../shop/shop-routing.module';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  baseUrl = environment.apiUrl;
  private basketSource = new BehaviorSubject<Basket | null>(null);
  basketSource$ = this.basketSource.asObservable();

  private basketTotalsSource = new BehaviorSubject<BasketTotals | null>(null);
  basketTotalsSource$ = this.basketTotalsSource.asObservable();
  shipping = 0;

  setShippingPrice(deliveryMethod: DeliveryMethod){
    this.shipping = deliveryMethod.price;
    this.calculateTotal();
  }


  constructor(private http: HttpClient) { }

  getBasket(id: string){
    this.http.get<Basket>(this.baseUrl+'basket?id='+id).subscribe({
      next: basket => {
        this.basketSource.next(basket);
        this.calculateTotal();
      }
    });
  }

  setBasket(basket: Basket){
    this.http.post<Basket>(this.baseUrl+'basket', basket).subscribe({
      next: basket => {
        this.basketSource.next(basket);
        this.calculateTotal();
      }
    });
  }

  getCurrentBasketValue(){
    return this.basketSource.value;
  }

  addItemToBasket(item: Product | BasketItem, quantity=1){

    if(isProduct(item))  item = this.mapProductItemToBasketItem(item);
    
    const basket = this.getCurrentBasketValue() ?? this.createBasket();
    basket.items = this.addOrUpdateItem(basket.items, item, quantity);

    this.setBasket(basket);
  }

  removeItemFromBasket(id: number, quantity = 1){
    const basket =  this.getCurrentBasketValue();
    if(!basket) return;

    const item = basket.items.find( x => x.id === id);
    if(item){
      item.quantity -= quantity;
      if(item.quantity === 0){
        basket.items = basket.items.filter(x => x.id !== id);
      }
      if(basket.items.length > 0) this.setBasket(basket);
      else this.deleteBasket(basket);
    }
  }
  deleteBasket(basket:Basket) {
    this.http.delete(this.baseUrl+'basket?id='+basket.id).subscribe({
      next: () => {
        this.deleteLocalBasket()
      }
    })
  }

  deleteLocalBasket(){
    this.basketSource.next(null);
    this.basketTotalsSource.next(null);
    localStorage.removeItem('basket_id');
  }

  private createBasket(): Basket {
    const basket = new Basket();
    localStorage.setItem('basket_id', basket.id);
    return basket;
  }

  private addOrUpdateItem(items: BasketItem[], itemToAdd: BasketItem, quantity: number): BasketItem[] {
    const item = items.find(x => x.id === itemToAdd.id);
    if(item){
      item.quantity += quantity;
    }else{
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
    }

    return items;
  }

  private mapProductItemToBasketItem(item:Product) :BasketItem {
    return {
      id: item.id,
      productName: item.name,
      price: item.price,
      brand: item.productBrand,
      type: item.productType,
      quantity: 0,
      pictureUrl: item.pictureUrl
    }
  }

  private calculateTotal(){
    const basket = this.getCurrentBasketValue();
    if(!basket) return;

    const shipping = this.shipping;
    const subtotal = basket.items.reduce((a, b) => (b.price * b.quantity + a), 0);
    const total = subtotal + this.shipping;
    this.basketTotalsSource.next({shipping: this.shipping, subtotal, total});
  }
}


function isProduct(item: Product | BasketItem): item is Product {
  return (item as Product).productBrand !== undefined;
}

