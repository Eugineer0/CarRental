import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';

import { AuthService } from './services/auth.service';

import { AuthorizationInterceptor } from './interceptors/authorization-interceptor';
import { RefreshAccessInterceptor } from './interceptors/refresh-access-interceptor';
import { RegistrationModalComponent } from './components/registration-modal/registration-modal.component';
import { SecretComponent } from './components/secret/secret.component';

@NgModule({
    declarations: [
        AppComponent,
        LoginComponent,
        RegisterComponent,
        RegistrationModalComponent,
        SecretComponent
    ],
    imports: [
        AppRoutingModule,
        HttpClientModule,
        BrowserModule,
        ReactiveFormsModule
    ],
    providers: [
        AuthService,
        { provide: HTTP_INTERCEPTORS, useClass: RefreshAccessInterceptor, multi: true },
        { provide: HTTP_INTERCEPTORS, useClass: AuthorizationInterceptor, multi: true }
    ],
    bootstrap: [AppComponent]
})
export class AppModule {
}