import { Component } from '@angular/core';
import { Login } from '../login';
import { LoginService } from '../login.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  user:Login=new Login();

  constructor(private loginService:LoginService,private router:Router){}
  loginClick()
  {
    // alert(this.user.username)
    this.loginService.CheckUser(this.user).subscribe(
      (response)=>{
        this.router.navigateByUrl("/home");
      },
      (error)=>{
        alert('Wrong user / pwd');
        this.user.username="";
        this.user.password="";
      }
    );
  }
}
