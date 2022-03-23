import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export const dateConsistencyValidator: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
    const startDate = control.get('startDate');
    const startTime = control.get('startTime');
    const finishDate = control.get('finishDate');
    const finishTime = control.get('finishTime');

    const start: Date = new Date(startDate?.value + ' ' + startTime?.value);
    const finish: Date = new Date(finishDate?.value + ' ' + finishTime?.value);

    console.log(start);
    console.log(finish);

    console.log(start <= finish)

    return start > finish ? { dateInconsistent: true } : null;
};