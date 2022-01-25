import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

import { LoginDTO } from '../_models/loginDTO';
import { AuthResponse } from '../_models/auth-responce';
import { TokenService } from "./token.service";
import { RefreshRequest } from "../_models/refresh-request";

@Injectable()
export class AuthService {
  private loggedIn = false

  constructor(
    private http: HttpClient,
    private tokenService: TokenService
  ) {
  }

  public login(admin: LoginDTO): Observable<any> {
    return this.http.post<AuthResponse>('/api/login', admin)
      .pipe(
        tap(
          response => {
            this.setSession(response);
          }
        )
      )
  }

  public closeSession() {
    this.tokenService.removeTokens();
    this.loggedIn = false;
  }

  public refresh(refreshToken: RefreshRequest): Observable<any> {
    return this.http.post<AuthResponse>('/api/refresh', refreshToken)
      .pipe(
        tap(
          response => {
            this.setSession(response);
          }
        )
      )
  }

  public isLoggedIn() {
    return this.loggedIn;
  }

  public isLoggedOut() {
    return !this.loggedIn;
  }

  private setSession(authResult: AuthResponse) {
    this.tokenService.setTokens(authResult);
    this.loggedIn = true;
  }
}
