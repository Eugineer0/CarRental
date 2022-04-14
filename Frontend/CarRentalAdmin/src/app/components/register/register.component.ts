import { Component, OnInit, TemplateRef } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { AuthService } from '../../services/auth.service';
import { FormErrorsRecognizerService } from '../../services/form-errors-recognizer.service';

import { minAgeValidator } from '../../validators/min-age-validator';

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
    public readonly minAge: number = 14;

    public readonly registerForm: FormGroup = new FormGroup({
        email: new FormControl(
            '',
            [
                Validators.required,
                Validators.minLength(3),
                Validators.maxLength(254),
                Validators.pattern(/^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$/)
            ]
        ),
        username: new FormControl(
            '',
            [
                Validators.required,
                Validators.maxLength(25),
                Validators.pattern(/^\w*$/)
            ]
        ),
        password: new FormControl(
            '',
            [
                Validators.required,
                Validators.minLength(5),
                Validators.maxLength(25),
                Validators.pattern(/(?=.*[A-Z])(?=.*[0-9])(?=.*[a-z])/)
            ]
        ),
        name: new FormControl(
            '',
            [
                Validators.required,
                Validators.maxLength(64),
                Validators.pattern(/^[A-Z][a-z]*(-[A-Z])?[a-z]*$/)
            ]
        ),
        surname: new FormControl(
            '',
            [
                Validators.required,
                Validators.maxLength(64),
                Validators.pattern(/^[A-Z][a-z]*((-[A-Z])?[a-z]*)*$/)
            ]
        ),
        passportNumber: new FormControl(
            '',
            [
                Validators.required,
                Validators.minLength(9),
                Validators.maxLength(9),
                Validators.pattern(/^[A-Z]{2}[0-9]{7}$/)
            ]
        ),
        dateOfBirth: new FormControl(
            '',
            [
                Validators.required,
                minAgeValidator(this.minAge)
            ]
        )
    });

    public registerFailedMessage: string = '';
    public registerFailed: boolean = false;
    public passwordVisible: boolean = false;

    constructor(
        private authService: AuthService,
        private router: Router,
        private modalService: NgbModal
    ) {
    }

    public ngOnInit(): void {
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
        return FormErrorsRecognizerService.checkIfErrorsOccurred(this.registerForm, formControlName);
    }

    public getControlBy(formControlName: string): AbstractControl | null {
        return this.registerForm.get(formControlName);
    }

    public open(content: TemplateRef<any>): void {
        this.modalService.open(content);
    }
}