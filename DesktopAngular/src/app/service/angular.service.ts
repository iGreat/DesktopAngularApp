import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {User} from "../model/user";

@Injectable({
  providedIn: 'root'
})
export class AngularService {

  constructor(private httpClient: HttpClient) {
  }

  registerUser(user: User): Observable<void> {
    return this.httpClient.post<void>('user/register', user);
  }
}
