import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { UserMinimal } from "../_models/user-minimal";
import { UserFull } from "../_models/user-full";
import { UserRoles } from "../_models/roles";

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

  public putRoles(username: string, roles: UserRoles): Observable<any> {
    return this.http.put(`${this.usersUrl}/${username}`, roles);
  }
}
