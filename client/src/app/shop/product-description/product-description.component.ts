import { Component, OnInit } from '@angular/core';
import { ShopService } from '../shop.service';
import { Product } from 'src/app/shared/models/product';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-product-description',
  templateUrl: './product-description.component.html',
  styleUrls: ['./product-description.component.scss']
})
export class ProductDescriptionComponent implements OnInit {

  product?: Product;
  
  constructor(private shopService: ShopService, private activatedRoute: ActivatedRoute ){}

  ngOnInit(): void {
    this.getProduct();
  }

  getProduct(){
    const id = this.activatedRoute.snapshot.paramMap.get("id");
    if(id)  this.shopService.getProduct(+id).subscribe({
      next: product => this.product = product,
      error: err => console.log(err)
    });
  }

}
