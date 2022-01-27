import { Injectable } from '@angular/core';
import { Router } from "@angular/router";
import { Observable } from "rxjs";
import { catchError } from "rxjs/operators";
import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest
} from "@angular/common/http";

import { AuthService } from "./auth.service";
import { TokenService } from "./token.service";

import { RefreshRequest } from "../_models/refresh-request";

@Injectable({
  providedIn: 'root'
})
export class RefreshAccessInterceptor implements HttpInterceptor {

  constructor(
    private authService: AuthService,
    private router: Router,
    private tokenService: TokenService
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
  ) {
    return (error: HttpErrorResponse) => {
      console.error(`${ request.method } ${ request.url } failed: ${ error.error }`);

      if (error.status === 401) {
        const refreshToken = this.tokenService.getRefreshToken();
        this.authService.closeSession();

        if (refreshToken) {
          const token: RefreshRequest = {
            token: refreshToken
          }

          this.authService.refresh(token)
            .subscribe(
              _ => {
                handler.handle(request).subscribe();
              }
            );
        } else {
          this.router.navigate(
            ['login'],
            {queryParams: {returnUrl: this.router.routerState.snapshot.url}}
          );
        }
      }

      throw error;
    }
  }
}
