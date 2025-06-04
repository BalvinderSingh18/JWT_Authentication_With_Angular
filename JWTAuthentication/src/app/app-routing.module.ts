import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AboutComponent } from './about/about.component';
import { ContactComponent } from './contact/contact.component';
import { EmployeeComponent } from './employee/employee.component';
import { LoginComponent } from './login/login.component';
import { ActivateguardService } from './activateguard.service';
import { RegisterComponent } from './register/register.component';

const routes: Routes = [
  {path:"",redirectTo:"home",pathMatch:"full"},
  {path:"home",component:HomeComponent},
  {path:"about",component:AboutComponent,canActivate:[ActivateguardService]},
  {path:"contact",component:ContactComponent,canActivate:[ActivateguardService]},
  {path:"employee",component:EmployeeComponent,canActivate:[ActivateguardService]},
  {path:"login",component:LoginComponent},
  {path:"register",component:RegisterComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
