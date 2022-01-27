import { Directive, Input } from '@angular/core';
import {
  AbstractControl,
  NG_VALIDATORS,
  ValidationErrors,
  Validator,
  ValidatorFn
} from '@angular/forms';

@Directive({
  selector: '[appPasswordConstraint]',
  providers: [{
    provide: NG_VALIDATORS,
    useExisting: PasswordConstraintDirective,
    multi: true
  }]
})
export class PasswordConstraintDirective implements Validator {
  @Input('appPasswordConstraint') regexString: string = ".*";

  constructor() {
  }

  public validate(control: AbstractControl): ValidationErrors | null {
    const regex = new RegExp(this.regexString);
    return passwordConstraintValidator(regex)(control);
  }
}

export function passwordConstraintValidator(regex: RegExp): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const isValid = regex.test(control.value);
    return isValid ? null : {passwordConstraint: {value: control.value}};
  };
}
