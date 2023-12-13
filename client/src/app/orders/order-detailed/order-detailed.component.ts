import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Order } from 'src/app/shared/models/order';
import { OrdersService } from '../orders.service';
import { BreadcrumbService } from 'xng-breadcrumb';

@Component({
  selector: 'app-order-detailed',
  templateUrl: './order-detailed.component.html',
  styleUrls: ['./order-detailed.component.scss']
})
export class OrderDetailedComponent implements OnInit {
  order?:Order;

  constructor(private orderService:OrdersService, private activatedRoute: ActivatedRoute, private bcService: BreadcrumbService){
    this.bcService.set('@orderDetailed', ' ');
  }
  
  ngOnInit(): void {
    this.loadOrder();
  }
  loadOrder() {
    const id = this.activatedRoute.snapshot.paramMap.get("id");

    if(id){
      this.orderService.getOrder(+id).subscribe({
        next: order => {
          this.order = order;
          this.bcService.set('@orderDetailed', 'Order#'+order.id+' - '+order.status);
        },
        error: err => console.log(err)
      });
    }
    
  }


}
