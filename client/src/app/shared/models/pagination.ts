import { Product } from "./product"

export interface Pagination<T> {
    pageSize: number
    pageIndex: number
    pageCount: number
    data: T;
  }