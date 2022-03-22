import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { AuthService } from '../../services/auth.service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
    public loginForm = new FormGroup({
        username: new FormControl(
            '',
            [
                Validators.required,
                Validators.maxLength(25)
            ]
        ),
        password: new FormControl(
            '',
            [
                Validators.required,
                Validators.maxLength(25)
            ]
        )
    });

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
        this.loginForm.markAsPristine();

        this.authService.login(this.loginForm.value)
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

    public checkIfErrorsOccurred(formControl: AbstractControl): boolean {
        return formControl.invalid && (formControl.dirty || formControl.touched)
    }
}