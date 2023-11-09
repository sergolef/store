import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ShopComponent } from './shop.component';
import { ProductDescriptionComponent } from './product-description/product-description.component';

const routes: Routes = [
  {path: '', component: ShopComponent},
  {path: ':id', component: ProductDescriptionComponent},
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
