import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { User } from '../_models/user';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new BehaviorSubject<User | null>(null);       // Moze byc User lub null
  currentUser$ = this.currentUserSource.asObservable();              //$ - jest to nieobserwowalny element

  constructor(private http: HttpClient) { }

  login(model:any){
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map((response:User) => {
        const user = response;
        if (user){                               // W klucz-wartosc nie mozemy przechowywac obiektów dlatego zamieniamiy na JSON.
          // localStorage.setItem('user', JSON.stringify(user));
          // this.currentUserSource.next(user);
          this.setCurrentUser(user);
        }
      })
    );
  }

  register(model:any){                                                           // .pipe robic cos z obiektem zanim go wysle.
    return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
      map(user =>{
        if(user){
          this.setCurrentUser(user);
        }
      })
    )
  }

  setCurrentUser(user: User){
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }
  logout(){
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }
}
