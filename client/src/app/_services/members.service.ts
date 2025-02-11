import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { map, of } from 'rxjs';
import { PaginatedResult } from '../_models/pagination';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members: Member[] = [];
  paginatedResult: PaginatedResult<Member[]> = new PaginatedResult<Member[]>;

  constructor(private http: HttpClient) { }

  getMembers(page?:number, itemsPerPage?:number){                                          // of - obiekt observable.
    let params = new HttpParams();

    if(page && itemsPerPage){
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage)
    }

    return this.http.get<Member[]>(this.baseUrl + 'user', {observe: 'response', params}).pipe(
      map(response => {
        if(response.body){
          this.paginatedResult.result = response.body;
        }
        const pagination = response.headers.get('Pagination');
        if(pagination){
          this.paginatedResult.pagination=JSON.parse(pagination);
        }
        return this.paginatedResult;
      })

    )
  }

  getMember(username:String){
    const member = this.members.find(x => x.userName === username);
    if(member) return of(member);

    return this.http.get<Member>(this.baseUrl + 'user/' + username);
  }

  updateMember(member: Member){
    return this.http.put(this.baseUrl + 'user', member).pipe(
      map(() => {
        const index = this.members.indexOf(member);            // ... - Pobiera wszystkie elementy członka
        this.members[index] = {...this.members[index], ...member}
      })
    )
  }
  setMainPhoto(photoId: number){
    return this.http.put(this.baseUrl + 'user/set_main_photo/' + photoId, {});
  }
  deletePhoto(photoId:number){
    return this.http.delete(this.baseUrl+ 'user/delete-photo/' + photoId);
  }
}
