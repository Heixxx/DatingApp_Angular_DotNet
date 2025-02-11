export interface Pagination{
  currentPage: number;
  itemsPerPage: number;
  totalItems:number;
  totalPages: number;
}

export class PaginatedResult<T> {    //T w naszym przypadku to lista rzeczy.
  result?: T;
  pagination?: Pagination;

}
