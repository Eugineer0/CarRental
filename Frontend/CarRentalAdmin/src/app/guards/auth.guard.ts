import { Injectable } from '@angular/core';
import {
    ActivatedRouteSnapshot,
    Router,
    RouterStateSnapshot,
    CanActivate
} from '@angular/router';

import { AuthService } from '../services/auth.service';

@Injectable({
    providedIn: 'root'
})
export class AuthGuard implements CanActivate {

    constructor(
        private authService: AuthService,
        private router: Router
    ) {
    }

    public canActivate(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): boolean {
        let loggedIn: boolean = false;
        this.authService.getLoggedInStatus()
            .subscribe(
                status => {
                    loggedIn = status;
                }
            );

        if (loggedIn) {
            return true;
        }

        this.router.navigate(
            ['login'],
            { queryParams: { returnUrl: state.url } }
        );

        return false;
    }
}