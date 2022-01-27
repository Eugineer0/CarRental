import { Component } from '@angular/core';

import { AuthService } from './_services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  constructor(private authService: AuthService) {
  }

  public isLoggedIn(): boolean {
    return this.authService.isLoggedIn();
  }

  public isLoggedOut(): boolean {
    return this.authService.isLoggedOut();
  }

  public logout() {
    this.authService.logout().subscribe();
  }
}
