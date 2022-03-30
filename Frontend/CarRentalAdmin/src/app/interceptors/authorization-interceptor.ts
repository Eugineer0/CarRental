import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {
    HttpRequest,
    HttpHandler,
    HttpEvent,
    HttpInterceptor
} from '@angular/common/http';

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
            // HttpRequest represents an outgoing request, including URL, method, headers, body, and other request configuration options. Instances should be assumed to be immutable. To modify a HttpRequest, the clone method should be used.

            const headers = request.headers.set(
                'Authorization',
                'Bearer ' + accessToken
            ); // A clone of the HTTP headers object with the newly set header value.

            request = request.clone({ headers }); // A clone of the HTTP request object with the updated headers value.
        }

        return request;
    }
}