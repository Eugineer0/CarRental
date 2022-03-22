import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

import { LoginRequest } from '../models/login-request';
import { AuthResponse } from '../models/auth-responce';
import { RefreshTokenRequest } from '../models/refresh-token-request';

import { LocalStorageService } from './local-storage.service';

@Injectable()
export class AuthService {
    private readonly baseUrl: string = '/api/auth';
    private loggedIn = new BehaviorSubject(false);

    constructor(
        private http: HttpClient,
        private tokenService: LocalStorageService
    ) {
    }

    public getLoggedInStatus(): BehaviorSubject<boolean> {
        return this.loggedIn;
    }

    public login(loginRequest: LoginRequest): Observable<AuthResponse> {
        return this.http.post<AuthResponse>(`${ this.baseUrl }/login-admin`, loginRequest)
            .pipe(
                tap(
                    response => {
                        this.setSession(response);
                    }
                )
            );
    }

    public logout(refreshToken: RefreshTokenRequest): Observable<RefreshTokenRequest> {
        return this.http.post<RefreshTokenRequest>(`${ this.baseUrl }/logout`, refreshToken)
            .pipe(
                tap(
                    _ => {
                        this.closeSession();
                    }
                )
            );
    }

    public refresh(refreshToken: RefreshTokenRequest): Observable<AuthResponse> {
        return this.http.post<AuthResponse>(`${ this.baseUrl }/refresh`, refreshToken)
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
        this.loggedIn.next(false);
    }

    private setSession(authResult: AuthResponse): void {
        this.tokenService.setTokens(authResult);
        this.loggedIn.next(true);
    }
}