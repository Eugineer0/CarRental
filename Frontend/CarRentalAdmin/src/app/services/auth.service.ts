import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { tap } from 'rxjs/operators';

import { LoginRequest } from '../models/requests/login-request';
import { RegisterRequest } from '../models/requests/register-request';
import { RefreshTokenRequest } from '../models/requests/refresh-token-request';
import { AuthResponse } from '../models/responses/auth-response';

import { LocalStorageService } from './local-storage.service';

@Injectable()
export class AuthService {
    private readonly baseUrl: string = '/api/auth';

    public readonly registerUrl: string = this.baseUrl + '/register-admin';
    public readonly loginUrl: string = this.baseUrl + '/login-admin';
    public readonly logoutUrl: string = this.baseUrl + '/logout';
    public readonly refreshUrl: string = this.baseUrl + '/refresh';

    public loggedIn: BehaviorSubject<boolean> = new BehaviorSubject(this.localStorageService.hasTokens());
    public accessRefreshing: boolean = false;
    public accessRefreshed: Subject<boolean> = new Subject<boolean>();

    constructor(
        private http: HttpClient,
        private localStorageService: LocalStorageService
    ) {
    }

    public register(registerRequest: RegisterRequest): Observable<void> {
        return this.http.post<void>(this.registerUrl, registerRequest);
    }

    public login(loginRequest: LoginRequest): Observable<AuthResponse> {
        return this.http.post<AuthResponse>(this.loginUrl, loginRequest)
            .pipe(
                tap(
                    response => {
                        this.setSession(response);
                    }
                )
            );
    }

    public logout(refreshToken: RefreshTokenRequest): Observable<RefreshTokenRequest> {
        return this.http.post<RefreshTokenRequest>(this.logoutUrl, refreshToken)
            .pipe(
                tap(
                    _ => {
                        this.closeSession();
                    }
                )
            );
    }

    public refresh(refreshToken: RefreshTokenRequest): Observable<AuthResponse> {
        return this.http.post<AuthResponse>(this.refreshUrl, refreshToken)
            .pipe(
                tap(
                    response => {
                        this.setSession(response);
                    },
                    _ => {
                        this.closeSession();
                    }
                )
            );
    }

    public closeSession(): void {
        this.localStorageService.removeTokens();
        this.loggedIn.next(false);
    }

    private setSession(authResult: AuthResponse): void {
        this.localStorageService.setTokens(authResult);
        this.loggedIn.next(true);
    }
}