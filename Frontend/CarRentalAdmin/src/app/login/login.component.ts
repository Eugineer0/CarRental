import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

import { LoginRequest } from '../_models/auth/login-request';

import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  public adminCredentials: LoginRequest = {
    username: '',
    password: ''
  };

  public authFailed: boolean = false;
  public authFailedMessage: string = '';
  private isVisiblePassword: boolean = false;
  private returnUrl: string = '';


  constructor(
      private authService: AuthService,
      private route: ActivatedRoute,
      private router: Router
  ) {
  }

  public ngOnInit(): void {
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '';
  }

  public resetAuthStatus(): void {
    this.authFailed = false;
    this.authFailedMessage = '';
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
    this.authService.login(this.adminCredentials)
        .subscribe(
            _ => {
              this.router.navigateByUrl(this.returnUrl);
            },
            error => {
              this.handleError(error);
              this.authFailed = true;
            }
        );
  }

  private handleError(error: HttpErrorResponse): void {
    this.authFailedMessage = `Something went wrong: {${ error.statusText } - ${ error.status }}`;

    if (error.status >= 500) {
      this.authFailedMessage = `Server error: {${ error.statusText } - ${ error.status }}`;
    } else if (error.status > 400) {
      this.authFailedMessage = error.error;
    } else if (error.status === 400) {
      this.authFailedMessage = LoginComponent.getBadRequestDetails(error);
    }
  }

  private static getBadRequestDetails(error: HttpErrorResponse): string {
    let errorMessage: string = '';
    for (let message of Object.values(error.error.errors)) {
      errorMessage += message + '\n';
    }

    return errorMessage;
  }
}