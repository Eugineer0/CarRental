import { Injectable } from '@angular/core';
import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { TokenService } from "./token.service";
import { Observable } from "rxjs";
import { catchError } from "rxjs/operators";
import { AuthService } from "./auth.service";
import { RefreshRequest } from "../_models/refresh-request";
import { ActivatedRoute, Router } from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class RefreshAccessInterceptor implements HttpInterceptor {

  constructor(
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute,
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
          this.handleError(`${ request.method } ${ request.url }`)
        )
      );
  }

  private handleError(action: string) {
    return (error: HttpErrorResponse) => {
      console.error(`${ action } failed: ${ error.error }`);

      if (error.status === 401) {
        const refreshToken = this.tokenService.getRefreshToken();
        this.authService.closeSession();

        if (refreshToken) {
          const token: RefreshRequest = {
            token: refreshToken
          }

          this.authService.refresh(token).subscribe();
        } else {
          this.router.navigate([], {relativeTo: this.route });
        }
      }

      throw error;
    }
  }
}
