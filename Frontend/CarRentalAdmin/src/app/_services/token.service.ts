import { Injectable } from '@angular/core';
import { AuthService } from "./auth.service";
import { AuthResponse } from "../_models/auth-responce";

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

  public setTokens(tokenPair: AuthResponse) {
    localStorage.setItem('access_token', tokenPair.accessToken);
    localStorage.setItem('refresh_token', tokenPair.refreshToken);
  }

  public removeTokens() {
    localStorage.removeItem('access_token');
    localStorage.removeItem('refresh_token');
  }
}
