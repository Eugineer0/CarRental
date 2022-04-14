import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import {
    HttpHandler,
    HttpRequest,
    HttpInterceptor,
    HttpEvent
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
        next: HttpHandler
    ): Observable<HttpEvent<any>> {
        return next.handle(request)
            .pipe(
                catchError(
                    error => {
                        console.error(`${ request.method } ${ request.url } failed: ${ error.error }`);

                        if (error.status === 401) {
                            if (this.authService.accessRefreshing) {
                                console.log(request.url + ' IS WAITING REFRESHING');

                                return this.authService.accessRefreshed
                                    .pipe(
                                        switchMap(
                                            value => {
                                                console.log(request.url + ' received value: ' + value);
                                                if (value) {
                                                    return next.handle(request);
                                                }

                                                throw error;
                                            }
                                        )
                                    );
                            } else {
                                const refreshToken = this.localStorageService.getRefreshToken();

                                console.log(request.url + ' IS REFRESHING');

                                if (refreshToken) {
                                    this.authService.accessRefreshing = true;

                                    const refreshTokenRequest: RefreshTokenRequest = {
                                        refreshToken: refreshToken
                                    };
                                    return this.refreshAccess(request, next, refreshTokenRequest);
                                }
                            }
                        }

                        console.log('outer throw invoked');
                        throw error;
                    }
                )
            );
    }

    private refreshAccess(
        request: HttpRequest<any>,
        handler: HttpHandler,
        refreshTokenRequest: RefreshTokenRequest
    ): Observable<any> {
        return this.authService.refresh(refreshTokenRequest)
            .pipe(
                catchError(
                    error => {
                        console.log('refresh catch error');

                        this.authService.accessRefreshed.next(false);
                        this.authService.accessRefreshing = false;
                        throw error;
                    }
                ),
                switchMap(
                    _ => {
                        console.log('refresh switch map');

                        this.authService.accessRefreshed.next(true);
                        this.authService.accessRefreshing = false;
                        return handler.handle(request);
                    }
                )
            );
    }
}