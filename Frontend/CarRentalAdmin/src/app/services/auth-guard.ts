import { Injectable } from '@angular/core';
import {
    ActivatedRouteSnapshot,
    Router,
    RouterStateSnapshot,
    CanActivate
} from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { AuthService } from './auth.service';

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
    ): Observable<boolean> {
        return this.authService.loggedIn
            .pipe(
                map(
                    isLoggedIn => {
                        if (!isLoggedIn) {
                            this.router.navigate(['login'], { queryParams: { returnUrl: state.url } });
                        }

                        return isLoggedIn;
                    }
                )
            );
    }
}