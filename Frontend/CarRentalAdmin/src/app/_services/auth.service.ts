import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

import { LoginDTO } from '../_models/loginDTO';
import { AuthResponse } from '../_models/auth-responce';
import { RefreshTokenRequest } from "../_models/refresh-token-request";

import { TokenService } from "./token.service";
import { RegisterDTO } from "../_models/registerDTO";

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

  public register(admin: RegisterDTO): Observable<any> {
    return this.http.post<AuthResponse>('/api/register', admin)
      .pipe(
        tap(
          response => {
            this.setSession(response);
          }
        )
      )
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

  public logout(refreshToken: RefreshTokenRequest): Observable<any> {
    return this.http.post<RefreshTokenRequest>('/api/logout', refreshToken)
      .pipe(
        tap(
          _ => {
            this.closeSession();
          }
        )
      )
  }

  public refresh(refreshToken: RefreshTokenRequest): Observable<any> {
    return this.http.post<AuthResponse>('/api/refresh', refreshToken)
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
