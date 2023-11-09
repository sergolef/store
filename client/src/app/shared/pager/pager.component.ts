import { Component, Output, Input, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-pager',
  templateUrl: './pager.component.html',
  styleUrls: ['./pager.component.scss']
})
export class PagerComponent {
@Input() totalItems?:number;
@Input() pageSize?:number;

@Output() pageChanged = new EventEmitter<number>();

onPageChange(event:any){
  console.log(event.page);
  this.pageChanged.emit(event.page);
}

}
