import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function minAgeValidator(minAgeYears: number): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        const age = getAge(control.value);
        const isValid = minAgeYears <= age;
        return isValid ? null : { minAge: { value: control.value } };
    };
}

function getAge(dateOfBirth: string) {
    const now = new Date();
    const date = new Date(dateOfBirth);
    const monthsDiff = now.getMonth() - date.getMonth();

    let age = now.getFullYear() - date.getFullYear();
    if (monthsDiff < 0 || (monthsDiff === 0 && now.getDate() < date.getDate())) {
        age--;
    }

    return age;
}