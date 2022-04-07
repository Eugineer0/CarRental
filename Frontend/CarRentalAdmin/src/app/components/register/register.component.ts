import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { AuthService } from '../../services/auth.service';

import { FormErrorsHandlerService } from '../../services/form-errors-handler.service';
import { fieldTemplateValidator } from '../../validators/fieldTemplateValidator';

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
    public registerForm = new FormGroup({
        email: new FormControl(
            '',
            [
                Validators.required,
                Validators.minLength(3),
                Validators.maxLength(254),
                fieldTemplateValidator(/.*@.+/)
            ]
        ),
        username: new FormControl(
            '',
            [
                Validators.required,
                Validators.maxLength(25),
                fieldTemplateValidator(/^\w*$/)
            ]
        ),
        password: new FormControl(
            '',
            [
                Validators.required,
                Validators.minLength(5),
                Validators.maxLength(25),
                fieldTemplateValidator(/(?=.*[A-Z])(?=.*[0-9])(?=.*[a-z])/)
            ]
        ),
        name: new FormControl(
            '',
            [
                Validators.required,
                Validators.maxLength(64),
                fieldTemplateValidator(/^[A-Z][a-z]*(-[A-Z])?[a-z]*$/)
            ]
        ),
        surname: new FormControl(
            '',
            [
                Validators.required,
                Validators.maxLength(64),
                fieldTemplateValidator(/^[A-Z][a-z]*((-[A-Z])?[a-z]*)*$/)
            ]
        ),
        passportNumber: new FormControl(
            '',
            [
                Validators.required,
                Validators.minLength(9),
                Validators.maxLength(9),
                fieldTemplateValidator(/^[A-Z]{2}[0-9]{7}$/)
            ]
        ),
        dateOfBirth: new FormControl(
            '',
            [
                Validators.required,
                Validators.maxLength(25)
            ]
        )
    });

    public registerFailedMessage: string = '';
    public registerFailed: boolean = false;
    public passwordVisible: boolean = false;

    constructor(
        private authService: AuthService,
        private router: Router
    ) {
    }

    ngOnInit(): void {
    }

    public resetRegisterStatus(): void {

        this.registerFailedMessage = '';
        this.registerFailed = false;
    }

    public changePasswordType(): void {
        this.passwordVisible = !this.passwordVisible;
    }

    public onSubmit(): void {
        this.registerForm.markAsPristine();

        this.authService.register(this.registerForm.value)
            .subscribe(
                _ => {
                    this.router.navigateByUrl('');
                },
                _ => {
                    this.registerFailed = true;
                    this.registerFailedMessage = 'Something went wrong';
                }
            );
    }

    public checkIfErrorsOccurred(formControlName: string): boolean {
        return FormErrorsHandlerService.checkIfErrorsOccurred(this.registerForm, formControlName);
    }

    public getControlBy(formControlName: string): AbstractControl {
        return FormErrorsHandlerService.getControlBy(this.registerForm, formControlName);
    }
}
