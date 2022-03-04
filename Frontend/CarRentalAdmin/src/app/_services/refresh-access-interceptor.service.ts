import { Injectable } from '@angular/core';
import { Router } from "@angular/router";
import { Observable, of } from "rxjs";
import { catchError, tap } from "rxjs/operators";
import {
  HttpErrorResponse,
  HttpHandler,
  HttpRequest,
  HttpInterceptor,
  HttpEvent
} from "@angular/common/http";

import { AuthService } from "./auth.service";
import { TokenService } from "./token.service";

import { RefreshTokenRequest } from "../_models/auth/refresh-token-request";

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
  ): (error: HttpErrorResponse) => never {
    return (error: HttpErrorResponse) => {
      console.error(`${ request.method } ${ request.url } failed: ${ error.error }`);

      if (error.status === 401) {
        const refreshToken = this.tokenService.getRefreshToken();
        this.authService.closeSession();

        if (refreshToken) {
          const refreshTokenRequest: RefreshTokenRequest = {
            token: refreshToken
          }

          this.authService.refresh(refreshTokenRequest)
            .subscribe(
              _ => {
                handler.handle(request)
                  .subscribe(
                    response => {
                      return of(response);
                    },
                    err => {
                      error = err;
                    }
                  );
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
