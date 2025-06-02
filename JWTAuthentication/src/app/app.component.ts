import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { LoginService } from './login.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'JWTAuthentication';
    constructor(private router:Router,public loginService:LoginService){}

  logoutClick()
  {
    this.loginService.currentUserName="";
    sessionStorage.clear();
    this.router.navigateByUrl("/login");
  }
}
