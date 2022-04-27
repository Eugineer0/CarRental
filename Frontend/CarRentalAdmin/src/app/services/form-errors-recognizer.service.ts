import { FormGroup } from '@angular/forms';

export class FormErrorsRecognizerService {
    public static checkIfErrorsOccurred(formGroup: FormGroup, formControlName: string): boolean {
        const formControl = formGroup.get(formControlName);

        return formControl!!
            && formControl.invalid
            && (formControl.dirty || formControl.touched);
    }
}