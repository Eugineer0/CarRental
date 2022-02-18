import { Injectable } from '@angular/core';
import { Router } from "@angular/router";
import { Observable } from "rxjs";
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

import { RefreshTokenRequest } from "../_models/refresh-token-request";

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

      console.log(error.status);

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
                handler.handle(request).subscribe();
              }
            );
        } else {
          this.router.navigate(
            ['login'],
            {queryParams: {returnUrl: this.router.routerState.snapshot.url}}
          );
        }
      } else if(error.status === 301)
      {
        const url = error.headers.get('Location');
        console.log(url);

        if(url)
        {
          this.router.navigateByUrl(url);
        }
      }

      throw error;
    }
  }
}
