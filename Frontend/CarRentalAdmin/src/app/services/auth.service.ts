import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { tap } from 'rxjs/operators';

import { LoginRequest } from '../models/requests/login-request';
import { RegisterRequest } from '../models/requests/register-request';
import { AuthResponse } from '../models/responses/auth-response';
import { RefreshTokenRequest } from '../models/requests/refresh-token-request';

import { LocalStorageService } from './local-storage.service';

@Injectable()
export class AuthService {
    private readonly baseUrl: string = '/api/auth';

    public loggedIn: BehaviorSubject<boolean> = new BehaviorSubject(this.localStorageService.hasTokens());
    public accessRefreshing: boolean = false;
    public accessRefreshed: Subject<boolean> = new Subject<boolean>();

    constructor(
        private http: HttpClient,
        private localStorageService: LocalStorageService
    ) {
    }

    public register(registerRequest: RegisterRequest): Observable<void> {
        return this.http.post<void>(`${ this.baseUrl }/register-admin`, registerRequest);
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
                        console.log(JSON.stringify(response));
                        this.setSession(response);
                    },
                    e => {
                        console.log(JSON.stringify(e));
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