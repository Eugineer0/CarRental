import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { LoginDTO } from '../_models/loginDTO';

import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  public admin: LoginDTO = {
    username: '',
    password: ''
  };

  private authFailed: boolean = false;
  private returnUrl: string = '/';

  private isVisiblePassword: boolean = false

  constructor(
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router
  ) {
  }

  public ngOnInit(): void {
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '';
  }

  public isAuthFailed(): boolean {
    return this.authFailed;
  }

  public resetAuthStatus(): void {
    this.authFailed = false;
  }

  public onSubmit(): void  {
    this.authService.login(this.admin)
      .subscribe(
        _ => {
          this.router.navigateByUrl(this.returnUrl)
        },
        _ => {
          this.authFailed = true;
        }
      );
  }

  public changePasswordType(): void {
    this.isVisiblePassword = !this.isVisiblePassword;
  }

  public getPasswordType(): string {
    return this.isVisiblePassword ? 'text' : 'password';
  }

  getButtonText() {
    return this.isVisiblePassword ? 'Hide' : 'Show';
  }
}
