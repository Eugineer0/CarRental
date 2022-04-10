import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Injectable({
    providedIn: 'root'
})
export class FormErrorsHandlerService {

    constructor() {
    }

    public static checkIfErrorsOccurred(formGroup: FormGroup, formControlName: string): boolean {
        const formControl = formGroup.controls[formControlName];
        return formControl.invalid && (formControl.dirty || formControl.touched);
    }
}