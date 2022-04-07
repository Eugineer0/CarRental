import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function fieldTemplateValidator(template: RegExp): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        const isValid = template.test(control.value);
        return isValid ? null : {fieldTemplate: {value: control.value}};
    };
}