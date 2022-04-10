import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { CompleteRegistrationComponent } from './components/complete-registration/complete-registration.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { RegistrationModalComponent } from './components/shared/registration-modal/registration-modal.component';

import { AuthService } from './services/auth.service';

import { AuthorizationInterceptor } from './interceptors/authorization-interceptor';
import { RefreshAccessInterceptor } from './interceptors/refresh-access-interceptor';

@NgModule({
    declarations: [
        AppComponent,
        LoginComponent,
        RegisterComponent,
        CompleteRegistrationComponent,
        RegistrationModalComponent
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