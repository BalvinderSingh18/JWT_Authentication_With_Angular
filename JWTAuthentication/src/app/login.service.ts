import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Login } from './login';
import { map, Observable } from 'rxjs';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Register } from './register';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  currentUserName: string = '';
  private apiUrl = 'https://localhost:7218/api/user';

  constructor(
    private httpClient: HttpClient,
    private jwtHelperService: JwtHelperService,
    private router: Router
  ) { }

  login(login: Login): Observable<any> {
    return this.httpClient.post<any>(`${this.apiUrl}/authenticate`, login)
      .pipe(map(user => {
        if (user) {
          this.currentUserName = user.username;
          sessionStorage.setItem('currentUser', JSON.stringify(user));
        }
        return user;
      }));
  }

  isAuthenticated(): boolean {
    const token = sessionStorage.getItem('currentUser');
    if (!token) return false;

    const parsedUser = JSON.parse(token);
    return !this.jwtHelperService.isTokenExpired(parsedUser.token);
  }

}
