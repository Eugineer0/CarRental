import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function minAgeValidator(minAgeYears: number): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        const dateOfBirth = new Date(control.value);
        const criticalDate = new Date(dateOfBirth.setFullYear(dateOfBirth.getFullYear() + minAgeYears));
        const now = new Date();

        const isValid = criticalDate.getTime() < now.getTime();

        return isValid ? null : { minAge: { value: control.value } };
    };
}