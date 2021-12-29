import * as moment from 'moment';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';

import { Admin } from './admin';
import { AuthResponse } from './auth-responce';

@Injectable()
export class AuthService {

  constructor(private http: HttpClient) {
  }

  public login(admin: Admin): Observable<any> {
    return this.http.post<AuthResponse>('/api/login', admin)
      .pipe(
        tap(
          response => this.setSession(response)
        )
      )
  }

  private setSession(authResult: AuthResponse) {
    const expiresAt = moment().add(authResult.expiresIn, 'second');

    localStorage.setItem('id_token', authResult.idToken);
    localStorage.setItem('expires_at', JSON.stringify(expiresAt.valueOf()) );
  }

  public logout() {
    localStorage.removeItem('id_token');
    localStorage.removeItem('expires_at');
  }

  public isLoggedIn() {
    return moment().isBefore(this.getExpiration());
  }

  public isLoggedOut() {
    return !this.isLoggedIn();
  }

  private getExpiration() {
    const expiration = localStorage.getItem('expires_at');

    if (expiration == null) {
      return moment(0);
    }

    const expiresAt = JSON.parse(expiration);
    return moment(expiresAt);
  }
}
