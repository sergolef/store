import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class BusyService {
  spinerCounter:number = 0;

  constructor(private spinner:NgxSpinnerService) { }

  busy(){
    this.spinerCounter++;
    this.spinner.show();
  }

  idle(){
    this.spinerCounter--;
    if(this.spinerCounter<=0){
      this.spinerCounter = 0;
      this.spinner.hide();
    }
  }
}
