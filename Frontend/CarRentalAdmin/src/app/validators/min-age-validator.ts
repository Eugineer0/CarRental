import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function minAgeValidator(minAgeYears: number): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        const now = new Date();
        const dateOfBirth = new Date(control.value);
        const criticalDate = new Date(
            dateOfBirth.setFullYear(
                dateOfBirth.getFullYear() + minAgeYears
            )
        );

        const isValid = criticalDate.getTime() < now.getTime();

        return isValid ? null : { minAge: { value: control.value } };
    };
}