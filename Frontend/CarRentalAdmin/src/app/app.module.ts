import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';

import {AuthService} from './services/auth.service';

import { ForbiddenNameDirective } from './forbidden-name.directive';

import { AppComponent } from './app.component';
import { WelcomeComponent } from './welcome/welcome.component';
import { SecretComponent } from './secret/secret.component';
import { LoginComponent } from './login/login.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    WelcomeComponent,
    SecretComponent,
    ForbiddenNameDirective
  ],
  imports: [
    AppRoutingModule,
    HttpClientModule,
    BrowserModule,
    FormsModule
  ],
  providers: [
    AuthService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
