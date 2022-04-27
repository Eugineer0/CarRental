import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { AuthService } from '../../services/auth.service';
import { FormErrorsRecognizerService } from '../../services/form-errors-recognizer.service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
    public loginForm: FormGroup = new FormGroup({
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

    public onSubmit(): void {
        this.authService.login(this.loginForm.value)
            .subscribe(
                _ => {
                    this.router.navigateByUrl(this.returnUrl);
                },
                _ => {
                    this.authFailed = true;
                    this.authFailedMessage = 'Something went wrong';
                }
            );
    }

    public checkIfErrorsOccurred(formControlName: string): boolean {
        return FormErrorsRecognizerService.checkIfErrorsOccurred(this.loginForm, formControlName);
    }

    public getControlBy(formControlName: string): AbstractControl | null {
        return this.loginForm.get(formControlName);
    }
}