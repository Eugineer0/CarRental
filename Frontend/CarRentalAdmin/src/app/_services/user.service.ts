import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { Order } from '../_models/user/order';

import { Roles, User } from "../_models/user/user";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private usersUrl: string = 'api/users';

  constructor(
    private http: HttpClient
  ) {
  }

  public getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.usersUrl);
  }

  public getOrders(username: string): Observable<Order[]> {
    return this.http.get<Order[]>(`${this.usersUrl}/${username}/orders`);
  }

  public getUser(username: string): Observable<User> {
    return this.http.get<User>(`${this.usersUrl}/${username}`);
  }

  public putRoles(username: string, roles: Roles[]): Observable<any> {
    return this.http.put(`${this.usersUrl}/${username}/roles`, {roles: roles});
  }

  public approveClient(username: string): Observable<any> {
    return this.http.post(`${this.usersUrl}/${username}/approve-client`, null);
  }
}
