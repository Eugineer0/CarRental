import { Injectable } from '@angular/core';
import { AuthResponse } from "../_models/auth/auth-responce";

@Injectable({
  providedIn: 'root'
})
export class TokenService {

  constructor() {
  }

  public getAccessToken(): string | null {
    return localStorage.getItem('access_token');
  }

  public getRefreshToken(): string | null {
    return localStorage.getItem('refresh_token');
  }

  public hasAccessToken(): boolean {
    return localStorage.hasOwnProperty('access_token');
  }

  public hasRefreshToken(): boolean {
    return localStorage.hasOwnProperty('refresh_token');
  }

  public setTokens(tokenPair: AuthResponse): void {
    localStorage.setItem('access_token', tokenPair.accessToken);
    localStorage.setItem('refresh_token', tokenPair.refreshToken);
  }

  public removeTokens(): void {
    localStorage.removeItem('access_token');
    localStorage.removeItem('refresh_token');
  }
}
