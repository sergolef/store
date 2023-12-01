import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ErrorTestComponent } from './core/error-test/error-test.component';
import { NotFoundComponent } from './core/not-found/not-found.component';
import { ServerErrorComponent } from './core/server-error/server-error.component';
import { authGuard } from './core/guards/auth.guard';

const routes: Routes = [
  {path: '', component: HomeComponent, data: {breadcrumb: 'Home'}},
  {path: 'shop', loadChildren: () => import("./shop/shop.module").then(r => r.ShopModule) },
  {path: 'basket', loadChildren: () => import("./basket/basket.module").then(r => r.BasketModule) },
  {path: 'checkout', 
    canActivate: [authGuard],
    loadChildren: () => import("./checkout/checkout.module").then(r => r.CheckoutModule)},
  {path: 'account', loadChildren: () => import("./account/account.module").then(r => r.AccountModule)},
  {path: 'not-found', component: NotFoundComponent}, 
  {path: 'server-error', component: ServerErrorComponent},
  {path: 'errors', component: ErrorTestComponent},
  {path: '**', redirectTo: '', pathMatch:'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
