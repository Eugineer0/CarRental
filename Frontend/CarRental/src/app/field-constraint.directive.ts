import { Directive, Input } from '@angular/core';
import {
  AbstractControl,
  NG_VALIDATORS,
  ValidationErrors,
  Validator,
  ValidatorFn
} from '@angular/forms';

@Directive({
  selector: '[appFieldConstraint]',
  providers: [{
    provide: NG_VALIDATORS,
    useExisting: FieldConstraintDirective,
    multi: true
  }]
})
export class FieldConstraintDirective implements Validator {
  @Input('appFieldConstraint') regexString: string = ".*";

  constructor() {
  }

  public validate(control: AbstractControl): ValidationErrors | null {
    const regex = new RegExp(this.regexString);
    return fieldConstraintValidator(regex)(control);
  }
}

export function fieldConstraintValidator(regex: RegExp): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const isValid = regex.test(control.value);
    return isValid ? null : {fieldConstraint: {value: control.value}};
  };
}
