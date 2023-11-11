import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-error-test',
  templateUrl: './error-test.component.html',
  styleUrls: ['./error-test.component.scss']
})
export class ErrorTestComponent {
  apiUrl = environment.apiUrl;
  validationErrors: string[] = [];

  constructor(private http: HttpClient){}

  get404Error(){
    this.http.get(this.apiUrl + 'products/55').subscribe({
      next: responce => console.log(responce),
      error: error => console.log(error)
    })
  }

  get400Error(){
    this.http.get(this.apiUrl + 'buggy/badrequest').subscribe({
      next: responce => console.log(responce),
      error: error => console.log(error)
    })
  }
  get500Error(){
    this.http.get(this.apiUrl + 'buggy/servererror').subscribe({
      next: responce => console.log(responce),
      error: error => console.log(error)
    })
  }
  get400ValidationError(){
    this.http.get(this.apiUrl + 'buggy/badrequest/{id}').subscribe({
      next: responce => console.log(responce),
      error: error => {
        this.validationErrors = error.errors;
        console.log(error)
      }
    })
  }
}
