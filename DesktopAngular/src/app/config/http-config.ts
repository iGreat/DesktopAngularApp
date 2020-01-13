import {HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from "@angular/common/http";
import {Injectable} from "@angular/core";
import {Observable, throwError} from "rxjs";
import {NzNotificationService} from "ng-zorro-antd";
import {environment} from "../../environments/environment";
import {catchError, map} from "rxjs/operators";

@Injectable()
export class HttpConfig implements HttpInterceptor {
  public static readonly AUTHKEYNAME = 'APSXAUTH';

  constructor(private notifyService: NzNotificationService) {
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    let url = req.url;
    if (url.startsWith('/')) {
      url = url.substr(1);
    }

    let token = localStorage.getItem(HttpConfig.AUTHKEYNAME);
    if (!token || !token.length)
      token = sessionStorage.getItem(HttpConfig.AUTHKEYNAME);
    if (!token || !token.length)
      token = '';

    const reqCopy = req.clone({
      url: `${environment.baseUrl}/${url}`,
      headers: req.headers
        .set(HttpConfig.AUTHKEYNAME, token)
        .set('Content-Type', 'application/json; charset=utf-8')
    });

    return next.handle(reqCopy)
      .pipe(map((event: HttpEvent<any>) => {
        if (event instanceof HttpErrorResponse) {
          this.notifyService.error('发生错误', event.error, {nzDuration: 0});
        }
        return event;
      }), catchError((error: HttpErrorResponse) => {1
        if (typeof error.error === 'string') {
          this.notifyService.error('发生错误', error.error, {nzDuration: 0});
        } else {
          this.notifyService.error('发生错误', error.message, {nzDuration: 0});
        }
        return throwError(error);
      }));
  }
}
