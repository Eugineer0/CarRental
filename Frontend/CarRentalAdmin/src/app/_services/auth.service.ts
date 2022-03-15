import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

import { LoginRequest } from '../_models/auth/login';
import { RegisterRequest } from "../_models/auth/register";
import { CompleteRegistrationRequest } from "../_models/auth/complete-registration";
import { RefreshTokenRequest } from "../_models/auth/refresh-token-request";
import { AuthResponse } from '../_models/auth/auth-responce';

import { TokenService } from "./token.service";

@Injectable()
export class AuthService {

  constructor(
    private http: HttpClient,
    private tokenService: TokenService
  ) {
  }

  public isLoggedIn(): boolean {
    return this.tokenService.hasAccessToken() && this.tokenService.hasRefreshToken();
  }

  public isLoggedOut(): boolean {
    return !this.isLoggedIn();
  }

  public register(admin: RegisterRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>('/api/auth/register-admin', admin)
      .pipe(
        tap(
          response => {
            this.setSession(response);
          }
        )
      )
  }

  public completeRegistration(admin: CompleteRegistrationRequest, token: string): Observable<void> {
    return this.http.post<void>(
      '/api/auth/complete-registration',
      admin,
      {headers: {'Authorization': `Bearer ${token}`}}
    )
  }

  public login(admin: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>('/api/auth/login', admin)
      .pipe(
        tap(
          response => {
            this.setSession(response);
          }
        )
      )
  }

  public logout(refreshToken: RefreshTokenRequest): Observable<void> {
    return this.http.post<void>('/api/auth/logout', refreshToken)
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
