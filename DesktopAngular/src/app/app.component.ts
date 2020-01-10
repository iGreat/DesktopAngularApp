import {Component, ComponentFactoryResolver, OnInit, ViewChild} from '@angular/core';
import {HostContainerDirective} from "./config/host-container.directive";
import {LoginService} from "./service/login.service";
import {HomeComponent} from "./home/home.component";
import {LoginComponent} from "./login/login.component";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less']
})
export class AppComponent implements OnInit {
  @ViewChild(HostContainerDirective, {static: true})
  hostContainer: HostContainerDirective;

  constructor(private componentFactoryResolver: ComponentFactoryResolver,
              private loginService: LoginService) {
  }

  ngOnInit(): void {
    this.loginService.validToken({}).subscribe(i => {
      if (i) {
        this.loadComponent(HomeComponent);
      } else {
        this.loadComponent(LoginComponent);
      }
    })
  }

  private loadComponent(type: any) {
    const componentFactory = this.componentFactoryResolver.resolveComponentFactory(type);
    const viewContainerRef = this.hostContainer.viewContainerRef;
    viewContainerRef.clear();
    viewContainerRef.createComponent(componentFactory);
  }
}
