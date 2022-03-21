import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';

import { TokenService } from './token.service';

@Injectable()
export class AuthorizationInterceptor implements HttpInterceptor {

    constructor(private tokenService: TokenService) {
    }

    public intercept(
        request: HttpRequest<any>,
        next: HttpHandler
    ): Observable<HttpEvent<any>> {
        const modifiedRequest = this.Authorize(request);
        return next.handle(modifiedRequest);
    }

    private Authorize(request: HttpRequest<any>): HttpRequest<any> {
        const accessToken = this.tokenService.getAccessToken();

        if (!accessToken) {
            return request;
        }

        return request.clone({
            headers: request.headers.set('Authorization',
                'Bearer ' + accessToken)
        });
    }
}