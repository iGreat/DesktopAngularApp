import {Component, OnInit} from '@angular/core';
import {Captcha} from "../../model/captcha";
import {LoginService} from "../../service/login.service";

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

  get captchaImg(): string {
    if (this.captcha && this.captcha.image && this.captcha.image.length) {
      return 'data:image/png;base64,' + this.captcha.image;
    }
    return '';
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
