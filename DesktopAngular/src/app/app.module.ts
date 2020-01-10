import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {AppComponent} from './app.component';
import {NgZorroAntdModule, NZ_I18N, zh_CN} from 'ng-zorro-antd';
import {FormsModule} from '@angular/forms';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {registerLocaleData} from '@angular/common';
import zh from '@angular/common/locales/zh';
import {LoginComponent} from './login/login.component';
import {HomeComponent} from './home/home.component';
import {HttpConfig} from "./config/http-config";
import {NgProgressRouterModule} from "@ngx-progressbar/router";
import {NgProgressHttpModule} from "@ngx-progressbar/http";
import {NgProgressModule} from "@ngx-progressbar/core";
import {HostContainerDirective} from './config/host-container.directive';
import {RouterModule} from "@angular/router";

registerLocaleData(zh);

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HomeComponent,
    HostContainerDirective
  ],
  imports: [
    BrowserModule,
    NgZorroAntdModule,
    FormsModule,
    HttpClientModule,
    RouterModule.forRoot([
      {path: '', component: AppComponent}
    ]),
    BrowserAnimationsModule,
    NgProgressModule,
    NgProgressRouterModule.withConfig({
      id: 'routerProgress'
    }),
    NgProgressHttpModule.withConfig({
      id: 'httpProgress'
    })
  ],
  providers: [
    {provide: NZ_I18N, useValue: zh_CN},
    {provide: HTTP_INTERCEPTORS, useClass: HttpConfig, multi: true}
  ],
  entryComponents: [
    HomeComponent,
    LoginComponent
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
