import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';

import { LocalStorageService } from '../services/local-storage.service';

@Injectable()
export class AuthorizationInterceptor implements HttpInterceptor {

    constructor(private localStorageService: LocalStorageService) {
    }

    public intercept(
        request: HttpRequest<any>,
        next: HttpHandler
    ): Observable<HttpEvent<any>> {
        const modifiedRequest = this.Authorize(request);
        return next.handle(modifiedRequest);
    }

    private Authorize(request: HttpRequest<any>): HttpRequest<any> {
        const accessToken = this.localStorageService.getAccessToken();

        if (accessToken) {
            const headers = request.headers.set(
                'Authorization',
                'Bearer ' + accessToken
            );

            request = request.clone({ headers });
        }

        return request;
    }
}