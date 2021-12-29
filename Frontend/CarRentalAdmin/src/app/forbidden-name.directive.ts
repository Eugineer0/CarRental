import { Directive, Input } from '@angular/core';
import { AbstractControl, NG_VALIDATORS, ValidationErrors, Validator, ValidatorFn } from '@angular/forms';

@Directive({
  selector: '[appForbiddenName]',
  providers: [{
      provide: NG_VALIDATORS,
      useExisting: ForbiddenNameDirective,
      multi: true
  }]
})
export class ForbiddenNameDirective implements Validator {

  @Input('appForbiddenName') forbiddenName = ''

  constructor() { }

  validate(control: AbstractControl): ValidationErrors | null {
    return this.forbiddenName ?
      forbiddenNameValidator(this.forbiddenName)(control) :
      null;
  }
}

export function forbiddenNameValidator(name: string): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    return name === control.value ? {forbiddenName: {value: control.value}} : null;
  };
}
