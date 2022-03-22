import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { LoginRequest } from '../../models/login-request';

import { AuthService } from '../../services/auth.service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
    public adminCredentials: LoginRequest = {
        username: '',
        password: ''
    };

    public authFailed: boolean = false;
    public isVisiblePassword: boolean = false;

    public authFailedMessage: string = '';
    private returnUrl: string = '';

    constructor(
        private authService: AuthService,
        private route: ActivatedRoute,
        private router: Router
    ) {
    }

    public ngOnInit(): void {
        this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '';
    }

    public resetAuthStatus(): void {
        this.authFailed = false;
        this.authFailedMessage = '';
    }

    public changePasswordType(): void {
        this.isVisiblePassword = !this.isVisiblePassword;
    }

    public onSubmit(): void {
        this.authService.login(this.adminCredentials)
            .subscribe(
                _ => {
                    this.router.navigateByUrl(this.returnUrl);
                },
                _ => {
                    this.authFailed = true;
                    this.authFailedMessage = 'Something went wrong'
                }
            );
    }
}