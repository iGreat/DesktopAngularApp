import {Component, OnInit} from '@angular/core';
import {LoginService} from "../service/login.service";
import {Captcha} from "../model/captcha";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.less']
})
export class LoginComponent implements OnInit {
  captcha: Captcha;

  constructor(private loginService: LoginService) {
    this.captcha = {} as Captcha;
  }

  ngOnInit() {
    this.loadCaptcha();
  }

  loadCaptcha() {
    this.loginService.getCaptcha().subscribe(i => {
      this.captcha = i;
    });
  }

}
