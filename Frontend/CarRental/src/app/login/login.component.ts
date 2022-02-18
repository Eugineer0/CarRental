import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpErrorResponse } from "@angular/common/http";

import { LoginDTO } from '../_models/login';

import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  public client: LoginDTO = {
    username: '',
    password: ''
  };

  public authFailed: boolean = false;
  public authFailedMessage: string = '';
  private isVisiblePassword: boolean = false;


  constructor(
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router
  ) {
  }

  public ngOnInit(): void {
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
    const returnUrl: string = this.route.snapshot.queryParams['returnUrl'];

    this.authService.login(this.client)
      .subscribe(
        _ => {
          this.router.navigateByUrl(returnUrl);
        },
        error => {
          this.handleError(error);
        }
      );
  }

  private handleError(error: HttpErrorResponse): void {
    if (error.status === 308) {
      this.router.navigate(['completeRegistration'], {queryParams: {token: error.error}});
    } else if (error.status > 499) {
      this.authFailedMessage = 'Something went wrong';
    } else {
      this.authFailedMessage = error.error || error.message;
    }

    this.authFailed = true;
  }
}
