import { Component } from '@angular/core';
import { Router } from "@angular/router";

import { AuthService } from './_services/auth.service';
import { TokenService } from "./_services/token.service";

import { RefreshTokenRequest } from "./_models/refresh-token-request";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  constructor(
    private authService: AuthService,
    private router: Router,
    private tokenService: TokenService
  ) {
  }

  public isLoggedIn(): boolean {
    return this.authService.isLoggedIn();
  }

  public isLoggedOut(): boolean {
    return this.authService.isLoggedOut();
  }

  public logout(): void {
    const refreshToken = this.tokenService.getRefreshToken();

    if (refreshToken) {
      const refreshTokenRequest: RefreshTokenRequest = {
        token: refreshToken
      }

      this.authService.logout(refreshTokenRequest)
        .subscribe(
          _ => {
            this.router.navigateByUrl('welcome');
          }
        );
    }
  }
}
