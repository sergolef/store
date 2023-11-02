import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Product } from './models/product';
import { Pagination } from './models/pagination';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Embroidary shop';
  products: Product[] = [];

  constructor(private http: HttpClient){

  }
  ngOnInit(): void {
    this.http.get<Pagination<Product[]>>("https://localhost:5001/api/products").subscribe({
      next: responce => {
          console.log(responce);
          this.products = responce.data;
      },
      error: error => {
        console.log(error);
      },
      complete: () => {
        console.log('Request completed');
      }
    })
  }
}
