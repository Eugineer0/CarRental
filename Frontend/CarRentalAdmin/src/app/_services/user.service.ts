import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { UserMinimal } from "../_models/user/user-minimal";
import { UserFull, Roles } from "../_models/user/user-full";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private usersUrl: string = 'api/users';

  constructor(
    private http: HttpClient
  ) {
  }

  public getUsers(): Observable<UserMinimal[]> {
    return this.http.get<UserMinimal[]>(this.usersUrl);
  }

  public getUser(username: string): Observable<UserFull> {
    return this.http.get<UserFull>(`${this.usersUrl}/${username}`);
  }

  public putRoles(username: string, roles: Roles[]): Observable<any> {
    return this.http.put(`${this.usersUrl}/${username}/roles`, {roles: roles});
  }
}
