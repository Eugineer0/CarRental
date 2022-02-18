import { Component, OnInit } from '@angular/core';
import { HttpErrorResponse } from "@angular/common/http";
import { Router } from "@angular/router";

import { AuthService } from "../_services/auth.service";

import { RegisterClientDTO } from "../_models/register";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  public client: RegisterClientDTO = {
    email: '',
    username: '',
    password: '',
    name: '',
    surname: '',
    passportNumber: '',
    driverLicenseSerialNumber: '',
    dateOfBirth: new Date()
  };

  public registerFailed: boolean = false;
  public registerFailedMessage: string = '';
  private isVisiblePassword: boolean = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {
  }

  ngOnInit(): void {
  }

  public resetRegisterStatus(): void {

    this.registerFailedMessage = '';
    this.registerFailed = false;
  }

  public changePasswordType(): void {
    this.isVisiblePassword = !this.isVisiblePassword;
  }

  public getPasswordType(): string {
    return this.isVisiblePassword ? 'text' : 'password';
  }

  public getButtonText(): string {
    return this.isVisiblePassword ? 'Hide' : 'Show';
  }

  public onSubmit(): void {
    if (this.validateAge(new Date(this.client.dateOfBirth))) {
      this.authService.register(this.client)
        .subscribe(
          _ => {
            this.router.navigateByUrl('login')
          },
          error => {
            this.handleError(error)
          }
        );
    }
  }

  private handleError(error: HttpErrorResponse): void {
    if (error.status > 499) {
      this.registerFailedMessage = 'Something went wrong';
    } else if(error.status < 401) {
      this.registerFailedMessage = error.message;
    } else {
      this.registerFailedMessage = error.error;
    }

    this.registerFailed = true;
  }

  private validateAge(dateOfBirth: Date): boolean {
    const year: number = dateOfBirth.getFullYear();
    const criticalDate: number = dateOfBirth.setFullYear(year + 19);
    if (criticalDate < Date.now()) {
      return true;
    }

    this.registerFailedMessage = 'Client have to reach 19 years'
    this.registerFailed = true;

    return false;
  }
}
