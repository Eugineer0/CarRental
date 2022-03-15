import { Component, OnInit } from '@angular/core';
import { HttpErrorResponse } from "@angular/common/http";
import { Router } from "@angular/router";

import { AuthService } from "../_services/auth.service";

import { RegisterRequest } from "../_models/auth/register";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  public admin: RegisterRequest = {
    email: '',
    username: '',
    password: '',
    name: '',
    surname: '',
    passportNumber: '',
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
    this.authService.register(this.admin)
      .subscribe(
        _ => {
          this.router.navigateByUrl('secret')
        },
        error => {
          this.handleError(error)
        }
      );
  }

  private handleError(error: HttpErrorResponse): void {
    if (error.status > 499) {
      this.registerFailedMessage = 'Something went wrong';
    } else {
      this.registerFailedMessage = error.error;
    }

    this.registerFailed = true;
  }
}
