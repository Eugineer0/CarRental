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

  authFailed = false;
  returnUrl: string = '/';

  constructor(
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router
  ) {
  }

  public ngOnInit(): void {
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '';
  }

  public isAuthFailed() {
    return this.authFailed;
  }

  public resetAuthStatus() {
    this.authFailed = false;
  }

  public onSubmit() {
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
}
