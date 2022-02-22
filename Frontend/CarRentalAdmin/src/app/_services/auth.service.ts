import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

import { LoginDTO } from '../_models/auth/login';
import { RegisterDTO } from "../_models/auth/register";
import { CompleteRegistrationDTO } from "../_models/auth/complete-registration";
import { AuthResponse } from '../_models/auth/auth-responce';
import { RefreshTokenRequest } from "../_models/auth/refresh-token-request";

import { TokenService } from "./token.service";

@Injectable()
export class AuthService {

  constructor(
    private http: HttpClient,
    private tokenService: TokenService
  ) {
  }

  public isLoggedIn(): boolean {
    return localStorage.hasOwnProperty('access_token') && localStorage.hasOwnProperty('refresh_token');
  }

  public isLoggedOut(): boolean {
    return !this.isLoggedIn();
  }

  public register(admin: RegisterDTO): Observable<AuthResponse> {
    return this.http.post<AuthResponse>('/api/auth/register', admin)
      .pipe(
        tap(
          response => {
            this.setSession(response);
          }
        )
      )
  }

  public completeRegistration(admin: CompleteRegistrationDTO): Observable<AuthResponse> {
    return this.http.post<AuthResponse>('/api/auth/completeRegistration', admin)
      .pipe(
        tap(
          response => {
            this.setSession(response);
          }
        )
      )
  }

  public login(admin: LoginDTO): Observable<AuthResponse> {
    return this.http.post<AuthResponse>('/api/auth/loginAdmin', admin)
      .pipe(
        tap(
          response => {
            this.setSession(response);
          }
        )
      )
  }

  public logout(refreshToken: RefreshTokenRequest): Observable<any> {
    return this.http.post('/api/auth/logout', refreshToken)
      .pipe(
        tap(
          _ => {
            this.closeSession();
          }
        )
      )
  }

  public refresh(refreshToken: RefreshTokenRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>('/api/auth/refresh', refreshToken)
      .pipe(
        tap(
          response => {
            this.setSession(response);
          }
        )
      )
  }

  public closeSession(): void {
    this.tokenService.removeTokens();
  }

  private setSession(authResult: AuthResponse): void {
    this.tokenService.setTokens(authResult);
  }
}
