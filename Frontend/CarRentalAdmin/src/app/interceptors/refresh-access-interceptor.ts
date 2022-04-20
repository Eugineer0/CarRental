import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import {
    HttpHandler,
    HttpRequest,
    HttpInterceptor,
    HttpEvent, HttpErrorResponse
} from '@angular/common/http';

import { AuthService } from '../services/auth.service';
import { LocalStorageService } from '../services/local-storage.service';

import { RefreshTokenRequest } from '../models/requests/refresh-token-request';

@Injectable({
    providedIn: 'root'
})
export class RefreshAccessInterceptor implements HttpInterceptor {

    constructor(
        private authService: AuthService,
        private router: Router,
        private localStorageService: LocalStorageService
    ) {
    }

    public intercept(
        request: HttpRequest<any>,
        handler: HttpHandler
    ): Observable<HttpEvent<any>> {
        return handler.handle(request)
            .pipe(
                catchError(
                    error => {
                        if (error.status === 401) {
                            this.renewRequest(request, handler, error);
                        }

                        throw error;
                    }
                )
            );
    }

    private renewRequest(
        request: HttpRequest<any>,
        handler: HttpHandler,
        error: HttpErrorResponse
    ): Observable<any> {
        if (request.url === this.authService.refreshUrl) {
            this.setRefreshFailed();
        } else if (this.authService.accessRefreshing) {
            return this.authService.accessRefreshed
                .pipe(
                    switchMap(
                        refreshedSuccessfully => {
                            if (refreshedSuccessfully) {
                                return handler.handle(request);
                            }

                            throw error;
                        }
                    )
                );
        } else {
            const refreshToken = this.localStorageService.getRefreshToken();
            if (refreshToken) {
                return this.refreshAccess(request, handler, refreshToken);
            }
        }

        throw error;
    }

    private refreshAccess(
        request: HttpRequest<any>,
        handler: HttpHandler,
        refreshToken: string
    ): Observable<any> {
        this.authService.accessRefreshing = true;

        const refreshTokenRequest: RefreshTokenRequest = {
            refreshToken: refreshToken
        };

        return this.authService.refresh(refreshTokenRequest)
            .pipe(
                switchMap(
                    _ => {
                        this.setRefreshSuccessful();
                        return handler.handle(request);
                    }
                )
            );
    }

    private setRefreshSuccessful(): void {
        this.authService.accessRefreshed.next(true);
        this.authService.accessRefreshing = false;
    }

    private setRefreshFailed(): void {
        this.authService.accessRefreshed.next(false);
        this.authService.accessRefreshing = false;
    }
}