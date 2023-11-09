import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Product } from '../shared/models/product';
import { ShopService } from './shop.service';
import { Brand } from '../shared/models/brand';
import { Type } from '../shared/models/type';
import {ShopParams} from "../shared/models/shopParams";

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {
  @ViewChild('search') searchTerm?: ElementRef;
  products: Product[] = [];
  brands: Brand[] = [];
  types: Type[] = [];
  shopParams: ShopParams = new ShopParams();
  totalItems: number = 0;

  sortSelectItems = [
    {name: "Alphabetical", id: "name"},
    {name: "Price: Low to high", id: "priceAsc"},
    {name: "Price: High to low", id: "priceDesc"}
  ];



  constructor(private shopService: ShopService){}

  ngOnInit(): void {
    this.getProductsList();
    this.getProductTypesList();
    this.getBrandsList();
  }

  onTypeSelected(typeId:number): void{
    this.shopParams.typeId = typeId;
    this.getProductsList();
  }

  onBrandSelected(brandId:number):void{
    this.shopParams.brandId = brandId;
    this.getProductsList();
  }

  onSortSelect(event:any){
    this.shopParams.sort = event.target.value;
    this.getProductsList();
  }

  onPageChange(event:any){
    console.log(event);
    if(event !== this.shopParams.pageIndex){
      this.shopParams.pageIndex = event;
      this.getProductsList();
    }
  }

  getProductsList(): void{
    this.shopService.getProducts(this.shopParams).subscribe({
      next: responce => {
        this.shopParams.pageIndex = responce.pageIndex;
        this.shopParams.pageSize = responce.pageSize;
        this.totalItems = responce.pageCount;
        this.products = responce.data;
      },
      error: error => console.log(error),
      complete: () => {
        console.log('Get Products loaded');
      }
    });
  }

  getBrandsList(): void {
    this.shopService.getBrands().subscribe({
      next: responce => {
        this.brands = [{id: 0, name: "All"}, ...responce];
      },
      error: error => console.log(error),
      complete: () => {
        console.log('Get Brands loaded');
      }
    });
  }

  getProductTypesList(): void {
    this.shopService.getTypes().subscribe({
      next: responce => {
        this.types = [{id: 0, name: "All"}, ...responce];
      },
      error: error => console.log(error),
      complete: () => {
        console.log('Get Product Types loaded');
      }
    });
  }

  onSearch(){
    this.shopParams.seach = this.searchTerm?.nativeElement.value;
    this.getProductsList();
  }

  onReset(){
    if(this.searchTerm) this.searchTerm.nativeElement.value = '';
    this.shopParams = new ShopParams();
    this.getProductsList();
    
  }

}
