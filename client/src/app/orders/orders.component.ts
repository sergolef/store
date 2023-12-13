import { Component, OnInit } from '@angular/core';
import { Order } from '../shared/models/order';
import { OrdersService } from './orders.service';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss']
})
export class OrdersComponent implements OnInit {
  orders: Order[] = [];
  
  constructor(private orderService:OrdersService){}

  ngOnInit(): void {
    this.loadOrders();
  }
  loadOrders() {
    this.orderService.getOrdersList().subscribe({
      next: orders => {
        this.orders = orders;
      }
    })
  }


}
