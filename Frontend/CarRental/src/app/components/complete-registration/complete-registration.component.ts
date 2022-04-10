import { Component, OnInit, TemplateRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { AuthService } from '../../services/auth.service';
import { FormErrorsHandlerService } from '../../services/form-errors-handler.service';

import { fieldTemplateValidator } from '../../validators/field-template-validator';

@Component({
    selector: 'app-complete-registration',
    templateUrl: './complete-registration.component.html',
    styleUrls: ['./complete-registration.component.css']
})
export class CompleteRegistrationComponent implements OnInit {
    public readonly driverLicenseForm: FormGroup = new FormGroup({
        driverLicenseSerialNumber: new FormControl(
            '',
            [
                Validators.required,
                Validators.minLength(9),
                Validators.maxLength(9),
                fieldTemplateValidator(/[0-9][A-Z]{2}[0-9]{6}/)
            ]
        )
    });

    public completionFailed: boolean = false;
    public completionFailedMessage: string = '';

    constructor(
        private authService: AuthService,
        private route: ActivatedRoute,
        private router: Router,
        private modalService: NgbModal
    ) {
    }

    public ngOnInit(): void {
    }

    public resetCompletionStatus(): void {
        this.completionFailed = false;
        this.completionFailedMessage = '';
    }

    public onSubmit(): void {
        const token = this.route.snapshot.queryParams['token'];

        if (token) {
            this.authService.completeRegistration(this.driverLicenseForm.value, token)
                .subscribe(
                    _ => {
                        this.router.navigateByUrl('login');
                    },
                    _ => {
                        this.completionFailed = true;
                        this.completionFailedMessage = 'Something went wrong';
                    }
                );
        } else {
            this.completionFailed = true;
            this.completionFailedMessage = 'Something went wrong';
        }
    }

    public checkIfErrorsOccurred(formControlName: string): boolean {
        return FormErrorsHandlerService.checkIfErrorsOccurred(this.driverLicenseForm, formControlName);
    }

    public getControlBy(formControlName: string): AbstractControl {
        return this.driverLicenseForm.controls[formControlName];
    }

    public open(content: TemplateRef<any>): void {
        this.modalService.open(content);
    }
}