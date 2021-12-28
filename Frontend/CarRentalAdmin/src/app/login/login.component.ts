import {Component, OnInit} from '@angular/core';
import { Admin } from '../admin';
import {ActivatedRoute, Router} from "@angular/router";
import {AuthService} from "../auth.service";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  admin: Admin = {
    login: "",
    password: ""
  };
  authFailed = false;
  returnUrl: string = '/';

  constructor(private authService: AuthService,
              private route: ActivatedRoute,
              private router: Router) { }

  ngOnInit(): void {
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

  resetAuthStatus() {
    this.authFailed = false;
  }

  onSubmit() {
    this.authService.login(this.admin)
      .subscribe(
        (_) => {
          this.router.navigateByUrl(this.returnUrl);
        },
        (_) => {
          this.authFailed = true;
        }
      );
  }
}
