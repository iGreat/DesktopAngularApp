import {Component, ComponentFactoryResolver, OnInit, ViewChild} from '@angular/core';
import {HostContainerDirective} from "./config/host-container.directive";
import {LoginService} from "./service/login.service";
import {LoginComponent} from "./home/login/login.component";
import {IndexComponent} from "./home/index/index.component";

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
        this.loadComponent(IndexComponent);
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
