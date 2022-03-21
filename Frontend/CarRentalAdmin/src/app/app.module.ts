import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';

import { AuthService } from './_services/auth.service';
import { AuthorizationInterceptor } from './_services/authorization-interceptor.service';
import { RefreshAccessInterceptor } from './_services/refresh-access-interceptor.service';

@NgModule({
    declarations: [
        AppComponent,
        LoginComponent
    ],
    imports: [
        AppRoutingModule,
        HttpClientModule,
        BrowserModule,
        FormsModule
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