import { Injectable } from '@angular/core';
import { AbstractControl, FormGroup } from '@angular/forms';

@Injectable({
    providedIn: 'root'
})
export class FormErrorsHandlerService {

    constructor() {
    }

    public static getControlBy(form: FormGroup, name: string): AbstractControl {
        return form.controls[name];
    }

    public static checkIfErrorsOccurred(formGroup: FormGroup, formControlName: string): boolean {
        const formControl = FormErrorsHandlerService.getControlBy(formGroup, formControlName);
        return formControl.invalid && (formControl.dirty || formControl.touched);
    }
}
