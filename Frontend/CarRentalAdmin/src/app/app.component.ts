import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { RefreshTokenRequest } from './models/requests/refresh-token-request';

import { AuthService } from './services/auth.service';
import { LocalStorageService } from './services/local-storage.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
    public loggedIn: boolean = false;

    constructor(
        private authService: AuthService,
        private router: Router,
        private localStorageService: LocalStorageService
    ) {
    }

    public ngOnInit(): void {
        this.authService.loggedIn
            .subscribe(
                status => {
                    this.loggedIn = status;
                }
            );
    }

    public logout(): void {
        const refreshToken = this.localStorageService.getRefreshToken();

        if (refreshToken) {
            const refreshTokenRequest: RefreshTokenRequest = {
                refreshToken: refreshToken
            };

            this.authService.logout(refreshTokenRequest)
                .subscribe(
                    _ => {
                        this.router.navigateByUrl('');
                    }
                );
        }
    }
}