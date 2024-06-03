import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { map, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members: Member[] = [];

  constructor(private http: HttpClient) { }

  getMembers(){                                          // of - obiekt observable.
    if(this.members.length>0) return of(this.members);
    return this.http.get<Member[]>(this.baseUrl + 'user').pipe(
      map(members =>{
        this.members = members;
        return members;
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
