import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {Captcha} from "../model/captcha";

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor(private httpClient: HttpClient) {
  }

  getCaptcha(): Observable<Captcha> {
    return this.httpClient.get<Captcha>('login/captcha');
  }

  validToken(data: any): Observable<boolean> {
    return this.httpClient.post<boolean>('login/validateToken', data);
  }
}
