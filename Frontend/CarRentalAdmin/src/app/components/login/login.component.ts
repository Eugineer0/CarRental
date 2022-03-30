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
    public passwordVisible: boolean = false;

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
        this.passwordVisible = !this.passwordVisible;
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

    public checkIfErrorsOccurred(formControlName: string): boolean {
        const formControl = this.getControlBy(formControlName);
        return formControl.invalid && (formControl.dirty || formControl.touched);
    }

    public getControlBy(name: string): AbstractControl {
        return this.loginForm.controls[name];
    }
}