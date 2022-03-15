import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';

import { TokenService } from "./token.service";

@Injectable()
export class AuthorizationInterceptor implements HttpInterceptor {

  constructor(private tokenService: TokenService) {
  }

  public intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const accessToken = this.tokenService.getAccessToken();

    if (!accessToken) {
      return next.handle(request);
    }

    const modifiedRequest = request.clone({
      headers: request.headers.set("Authorization",
        "Bearer " + accessToken)
    });

    return next.handle(modifiedRequest);
  }
}
