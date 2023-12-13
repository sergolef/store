import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Order } from '../shared/models/order';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class OrdersService {
  baseUrl = environment.apiUrl;

  

  constructor(private http:HttpClient) { }

  getOrdersList(){
    return this.http.get<Order[]>(this.baseUrl + "orders");
  }

  getOrder(id:number){
    return this.http.get<Order>(this.baseUrl+'orders/'+id);
  }
}
