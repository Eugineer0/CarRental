import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

import { LoginRequest } from '../_models/login-request';
import { AuthResponse } from '../_models/auth-responce';
import { RefreshTokenRequest } from '../_models/refresh-token-request';

import { TokenService } from './token.service';

@Injectable()
export class AuthService {
    private loggedIn: boolean = false;

    constructor(
        private http: HttpClient,
        private tokenService: TokenService
    ) {
    }

    public isLoggedIn(): boolean {
        return this.loggedIn;
    }

    public isLoggedOut(): boolean {
        return !this.loggedIn;
    }

    public login(admin: LoginRequest): Observable<AuthResponse> {
        return this.http.post<AuthResponse>('/api/auth/login-admin', admin)
            .pipe(
                tap(
                    response => {
                        this.setSession(response);
                    }
                )
            );
    }

    public logout(refreshToken: RefreshTokenRequest): Observable<RefreshTokenRequest> {
        return this.http.post<RefreshTokenRequest>('/api/auth/logout', refreshToken)
            .pipe(
                tap(
                    _ => {
                        this.closeSession();
                    }
                )
            );
    }

    public refresh(refreshToken: RefreshTokenRequest): Observable<AuthResponse> {
        return this.http.post<AuthResponse>('/api/refresh', refreshToken)
            .pipe(
                tap(
                    response => {
                        this.setSession(response);
                    }
                )
            );
    }

    public closeSession(): void {
        this.tokenService.removeTokens();
        this.loggedIn = false;
    }

    private setSession(authResult: AuthResponse): void {
        this.tokenService.setTokens(authResult);
        this.loggedIn = true;
    }
}