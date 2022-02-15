import { Component, OnInit } from '@angular/core';
import { HttpErrorResponse } from "@angular/common/http";

import { AuthService } from "../_services/auth.service";

import { RegisterDTO } from "../_models/registerDTO";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  public admin: RegisterDTO = {
    email: '',
    username: '',
    password: '',
    name: '',
    surname: '',
    passportNumber: '',
    dateOfBirth: new Date()
  };

  public registerFailed: boolean = false;
  public registerFailedMessage: string = 'something went wrong';
  private isVisiblePassword: boolean = false;

  constructor(private authService: AuthService) {
  }

  ngOnInit(): void {
  }

  public resetRegisterStatus(): void {
    this.registerFailed = false;
    this.registerFailedMessage = 'something went wrong';
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
        error => {
          this.handleError(error)
        }
      );
  }

  private handleError(error: HttpErrorResponse): void {
    switch (error.status) {
      case 401: {
        this.registerFailedMessage = 'Incorrect username or password';
        break;
      }
      case 504: {
        this.registerFailedMessage = 'Cannot connect to the server';
        break;
      }
      default: {
        break;
      }
    }

    this.registerFailed = true;
  }

}
