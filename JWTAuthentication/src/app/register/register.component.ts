import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { LoginService } from '../login.service';
import { Register } from '../register';
import { RegisterService } from '../register.service';

@Component({
  selector: 'app-register',
  standalone: false,
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  user: Register = new Register();

  constructor(
    private registerService: RegisterService,
    private router: Router
  ) {}

  RegisterClick(): void {

    this.registerService.register(this.user).subscribe(
      (response)=>{
        this.router.navigateByUrl("/home");
      },
      (error)=>{
        alert('Write Valid Information !');
      }
    );
  }
}
