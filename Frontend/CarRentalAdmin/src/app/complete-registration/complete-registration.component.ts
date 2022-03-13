import { Component, OnInit } from '@angular/core';
import { HttpErrorResponse } from "@angular/common/http";
import { ActivatedRoute, Router } from "@angular/router";

import { CompleteRegistrationRequest } from '../_models/auth/complete-registration'

import { AuthService } from "../_services/auth.service";

@Component({
  selector: 'app-complete-registration',
  templateUrl: './complete-registration.component.html',
  styleUrls: ['./complete-registration.component.css']
})
export class CompleteRegistrationComponent implements OnInit {
  public userInfo: CompleteRegistrationRequest = {
    driverLicenseSerialNumber: ''
  };

  public completionFailed: boolean = false;
  public completionFailedMessage: string = '';

  constructor(
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router
  ) {
  }

  public ngOnInit(): void {
  }

  public resetCompletionStatus(): void {
    this.completionFailed = false;
    this.completionFailedMessage = '';
  }

  public onSubmit(): void {
    const token = this.route.snapshot.queryParams['token'];

    this.authService.completeRegistration(this.userInfo, token)
      .subscribe(
        _ => {
          this.router.navigateByUrl('secret');
        },
        error => {
          this.handleError(error);
        }
      );
  }

  private handleError(error: HttpErrorResponse): void {
    if (error.status > 499) {
      this.completionFailedMessage = 'Something went wrong';
    } else {
      this.completionFailedMessage = error.error || error.message;
    }

    this.completionFailed = true;
  }
}
