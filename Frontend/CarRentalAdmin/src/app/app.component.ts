import { Component } from '@angular/core';

import { AuthService } from './_services/auth.service';
import { Router } from "@angular/router";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  constructor(
    private authService: AuthService,
    private router: Router
  ) {
  }

  public isLoggedIn(): boolean {
    return this.authService.isLoggedIn();
  }

  public isLoggedOut(): boolean {
    return this.authService.isLoggedOut();
  }

  public logout() {
    this.authService.logout()
      .subscribe(
        _ => {
          this.router.navigate(['welcome']);
        }
      );
  }
}
