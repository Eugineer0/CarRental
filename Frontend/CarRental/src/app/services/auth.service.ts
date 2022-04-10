import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

import { LoginRequest } from '../models/requests/login-request';
import { RegisterRequest } from '../models/requests/register-request';
import { RefreshTokenRequest } from '../models/requests/refresh-token-request';
import { CompleteRegistrationRequest } from '../models/requests/complete-registration-request';
import { AuthResponse } from '../models/responses/auth-response';

import { LocalStorageService } from './local-storage.service';

@Injectable()
export class AuthService {
    private readonly baseUrl: string = '/api/auth';
    private readonly loggedIn: BehaviorSubject<boolean> = new BehaviorSubject(this.localStorageService.hasTokens());

    constructor(
        private http: HttpClient,
        private localStorageService: LocalStorageService,
        private router: Router
    ) {
    }

    public getLoggedInStatus(): BehaviorSubject<boolean> {
        return this.loggedIn;
    }

    public register(registerRequest: RegisterRequest): Observable<void> {
        return this.http.post<void>(`${ this.baseUrl }/register`, registerRequest);
    }

    public completeRegistration(driverLicense: CompleteRegistrationRequest, token: string): Observable<void> {
        return this.http.post<void>(
            `${ this.baseUrl }/complete-registration`,
            driverLicense,
            { headers: { 'Authorization': `Bearer ${ token }` } }
        );
    }

    public login(loginRequest: LoginRequest): Observable<AuthResponse> {
        return this.http.post<AuthResponse>(`${ this.baseUrl }/login`, loginRequest)
            .pipe(
                tap(
                    response => {
                        this.setSession(response);
                    },
                    error => {
                        if (error.status === 308) {
                            this.router.navigate(
                                ['complete-registration'],
                                { queryParams: { token: error.error } }
                            );
                        }
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
        this.localStorageService.removeTokens();
        this.loggedIn.next(false);
    }

    private setSession(authResult: AuthResponse): void {
        this.localStorageService.setTokens(authResult);
        this.loggedIn.next(true);
    }
}