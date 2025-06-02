import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Login } from './login';
import { map, Observable } from 'rxjs';
import { Router } from '@angular/router';
import { JwtHelperService, JWT_OPTIONS } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  currentUserName:string="";
  constructor(private httpClient:HttpClient,private jwtHelperService:JwtHelperService,
    private router:Router
  ) { }
  CheckUser(login:Login):Observable<any>
  {
    return this.httpClient.post<any>("https://localhost:7218/api/user/authenticate",
    login).pipe(map(user=>{
      if(user)
      {
        this.currentUserName=user.username;
        sessionStorage['currentUser']=JSON.stringify(user);
      }
      return null;
    }))
  }
  public isAuthenticated():boolean
  {
    if(this.jwtHelperService.isTokenExpired())
    {
      return false;
    }
    else
    {
      return true;
    }
  }
}
