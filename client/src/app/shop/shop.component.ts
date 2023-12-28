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
  shopParams: ShopParams;
  totalItems: number = 0;

  sortSelectItems = [
    {name: "Alphabetical", id: "name"},
    {name: "Price: Low to high", id: "priceAsc"},
    {name: "Price: High to low", id: "priceDesc"}
  ];



  constructor(private shopService: ShopService){
    this.shopParams = shopService.getShopParams();
  }

  ngOnInit(): void {
    this.getProductsList();
    this.getProductTypesList();
    this.getBrandsList();
  }

  onTypeSelected(typeId:number): void{
    const params = this.shopService.getShopParams();
    params.typeId = typeId;
    params.pageIndex = 1;
    this.shopParams = params;
    this.shopService.setShopParams(params);

    this.getProductsList();
  }

  onBrandSelected(brandId:number):void{
    const params = this.shopService.getShopParams();
    params.brandId = brandId;
    params.pageIndex = 1;
    this.shopService.setShopParams(params);
    this.shopParams = params;
  
    this.getProductsList();
  }

  onSortSelect(event:any){
    const params = this.shopService.getShopParams();

    params.sort = event.target.value;
    this.shopService.setShopParams(params);
    this.shopParams = params;

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
    this.shopService.getProducts().subscribe({
      next: responce => {
        // this.shopParams.pageIndex = responce.pageIndex;
        // this.shopParams.pageSize = responce.pageSize;
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
