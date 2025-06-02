import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Employee } from './employee';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  constructor(private httpClient:HttpClient) { }
  getEmployees():Observable<any>
  {
    //JWT Token
    // var currentUser={token:""}
    // var headers = new HttpHeaders();
    // var currentUserSession = sessionStorage.getItem('currentUser');
    // if(currentUserSession != null)
    // {
    //   currentUser=JSON.parse(currentUserSession);
    //   headers=headers.set("Authorization","Bearer " + currentUser.token);
    // }
    return this.httpClient.get<any>("https://localhost:7218/api/employee");
  }
  saveEmployee(employee:Employee):Observable<any>
  {
        //JWT Token
        // var currentUser={token:""}
        // var headers = new HttpHeaders();
        // var currentUserSession = sessionStorage.getItem('currentUser');
        // if(currentUserSession != null)
        // {
        //   currentUser=JSON.parse(currentUserSession);
        //   headers=headers.set("Authorization","Bearer " + currentUser.token);
        // }
    return this.httpClient.post<any>("https://localhost:7218/api/employee",
    employee);
  }
  updateEmployee(employee:Employee):Observable<any>
  {
        //JWT Token
        // var currentUser={token:""}
        // var headers = new HttpHeaders();
        // var currentUserSession = sessionStorage.getItem('currentUser');
        // if(currentUserSession != null)
        // {
        //   currentUser=JSON.parse(currentUserSession);
        //   headers=headers.set("Authorization","Bearer " + currentUser.token);
        // }
    return this.httpClient.put<any>("https://localhost:7218/api/employee",
    employee);
  }
  deleteEmployee(id:number):Observable<any>
  {
        //JWT Token
        // var currentUser={token:""}
        // var headers = new HttpHeaders();
        // var currentUserSession = sessionStorage.getItem('currentUser');
        // if(currentUserSession != null)
        // {
        //   currentUser=JSON.parse(currentUserSession);
        //   headers=headers.set("Authorization","Bearer " + currentUser.token);
        // }
    return this.httpClient.delete<any>("https://localhost:7218/api/employee/" + id);
  }
}
