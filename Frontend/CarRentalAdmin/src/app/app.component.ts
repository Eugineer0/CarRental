import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { AuthService } from './auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  public isLoggedIn():boolean {
    return this.authService.isLoggedIn();
  }

  public isLoggedOut():boolean {
    return this.authService.isLoggedOut();
  }

  public logout() {
    this.authService.logout();
    this.router.navigateByUrl('');
  }
}
