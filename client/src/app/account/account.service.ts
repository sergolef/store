import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Address, User } from '../shared/models/user';
import {  ReplaySubject, map, of } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private loginUserSource = new ReplaySubject<User | null>(1);
  loginUserSource$ = this.loginUserSource.asObservable();

  constructor(private http:HttpClient, private router:Router) { }

  loadCurrentUser(token:string | null){
    if(token === null){
      this.loginUserSource.next(null);
      return of(null);
    }

    let headers = new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);

    return this.http.get<User>(this.baseUrl+'account', {headers}).pipe(
      map(user => {
        if(user){
          localStorage.setItem("token", user.token.toString());
          this.loginUserSource.next(user);
          return user;
        }else{
          return null;
        }
        
      })
    )
  }

  login(value: any){
    return this.http.post<User>(this.baseUrl + 'account/login', value)
      .pipe(
        map(user => {
          localStorage.setItem("token", user.token.toString());
          this.loginUserSource.next(user)
        })
      )
  }

  register(value: any){
    return this.http.post<User>(this.baseUrl + 'account/register', value)
    .pipe(
      map(user => {
        localStorage.setItem("token", user.token.toString());
        this.loginUserSource.next(user)
      })
    )
  }

  logout(){
    localStorage.removeItem("token");
    this.loginUserSource.next(null);
    this.router.navigateByUrl("/");
  }

  isEmailExists(email:string){
    console.log(this.baseUrl+"account/emailExists?email="+email);
    return this.http.get<boolean>( this.baseUrl + "account/emailExists?email=" + email);
  }

  getUserAddress(){
    return this.http.get<Address>(this.baseUrl+'account/address');
  }

  updateUserAddress(address:Address){
    return this.http.put<Address>(this.baseUrl+'account/address', address);
  }
}
