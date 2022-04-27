import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import {
    HttpErrorResponse,
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
                    this.handleError(request, next)
                )
            );
    }

    private handleError(
        request: HttpRequest<any>,
        handler: HttpHandler
    ): (error: HttpErrorResponse) => never {
        return (error: HttpErrorResponse) => {
            console.error(`${ request.method } ${ request.url } failed: ${ error.error }`);

            if (error.status === 401) {
                const refreshToken = this.localStorageService.getRefreshToken();
                this.authService.closeSession();

                if (refreshToken) {
                    const refreshTokenRequest: RefreshTokenRequest = {
                        refreshToken: refreshToken
                    };

                    this.authService.refresh(refreshTokenRequest)
                        .subscribe(
                            _ => {
                                handler.handle(request);
                            }
                        );
                } else {
                    this.router.navigate(
                        ['login'],
                        { queryParams: { returnUrl: this.router.routerState.snapshot.url } }
                    );
                }
            }

            throw error;
        };
    }
}