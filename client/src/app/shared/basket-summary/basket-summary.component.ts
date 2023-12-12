import { Component, EventEmitter, Input, Output } from '@angular/core';
import { BasketItem } from '../models/basket';
import { BasketService } from 'src/app/basket/basket.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-basket-summary',
  templateUrl: './basket-summary.component.html',
  styleUrls: ['./basket-summary.component.scss']
})
export class BasketSummaryComponent {
  @Output() addItem = new EventEmitter<BasketItem>();
  @Output() removeItem = new EventEmitter<{ id: number, quantity: number }>();

  @Input() isBasket = true;
  
  constructor(public basketService:BasketService, public router:Router){}

  addBasketItem(item: BasketItem) {
    console.log(item);
    
    this.addItem.emit(item);
  }

  removeBsketItem(id: number, quantity = 1) {
    console.log(id, quantity);
    
    this.removeItem.emit({ id, quantity });
  }

}
