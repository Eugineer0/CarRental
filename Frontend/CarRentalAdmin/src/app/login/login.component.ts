import { Component, OnInit } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';

import { Admin } from '../admin';

import { AuthService } from '../auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  public admin: Admin = {
    login: '',
    password: ''
  };
  authFailed = false;
  returnUrl: string = '/';

  constructor(
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  public ngOnInit(): void {
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '';
  }

  public resetAuthStatus() {
    this.authFailed = false;
  }

  public onSubmit() {
    this.authService.login(this.admin)
      .subscribe(
        result => {
          console.log('result: ' + result);
          this.router.navigateByUrl(this.returnUrl)
        },
        error => {
          console.log('error: ' + error);
          this.authFailed = true;
        }
      );
  }
}
