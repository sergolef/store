import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ShopComponent } from './shop.component';
import { ProductDescriptionComponent } from './product-description/product-description.component';

const routes: Routes = [
  {path: '', component: ShopComponent, data: {breadcrumb: 'Shop'}},
  {path: ':id', component: ProductDescriptionComponent, data: {breadcrumb: {alias: 'productDetails'}}},
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class ShopRoutingModule { }
