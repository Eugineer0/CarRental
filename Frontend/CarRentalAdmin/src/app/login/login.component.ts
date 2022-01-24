import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';

import {Admin} from '../admin';

import {AuthService} from '../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  public admin: Admin = {
    username: '',
    password: ''
  };

  authStarted = false;
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
    return this.authStarted && this.authService.isLoggedOut();
  }

  public resetAuthStatus() {
    this.authStarted = false;
  }

  public onSubmit() {
    this.authStarted = true;

    this.authService.login(this.admin)
      .subscribe(
        result => {
          console.log('result: ' + result);
          this.router.navigateByUrl(this.returnUrl)
        },
        error => {
          console.log('error: ' + error);
        }
      );
  }
}
