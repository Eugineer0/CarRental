import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import * as moment from 'moment';

export function minAgeValidator(minAgeYears: number): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        const criticalDate = moment(control.value).add(minAgeYears, 'year');
        const isValid = moment().isAfter(criticalDate);
        return isValid ? null : { minAge: { value: control.value } };
    };
}