import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor(private httpClient: HttpClient) {
  }

  validToken(data: any): Observable<boolean> {
    return this.httpClient.post<boolean>('login/validateToken', data);
  }
}
