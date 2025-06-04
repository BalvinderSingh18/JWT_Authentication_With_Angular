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
export class RegisterService {
  
  currentUserName: string = '';
  private apiUrl = 'https://localhost:7218/api/user';

  constructor(
    private httpClient: HttpClient,
    private jwtHelperService: JwtHelperService,
    private router: Router
  ) { }
    getAllUsers(): Observable<any[]> {
      return this.httpClient.get<any[]>(`${this.apiUrl}/all`);
    }
  
    updateUser(id: string, updateData: any): Observable<any> {
      return this.httpClient.put(`${this.apiUrl}/${id}`, updateData);
    }
  
    deleteUser(id: string): Observable<any> {
      return this.httpClient.delete(`${this.apiUrl}/${id}`);
    }
  
    register(user: Register): Observable<any> {
      return this.httpClient.post<any>(`${this.apiUrl}/register`, user)
        .pipe(map(user => {
          if (user) {
            this.currentUserName = user.username;
            sessionStorage.setItem('currentUser', JSON.stringify(user));
          }
          return user;
        }));
    }
}
