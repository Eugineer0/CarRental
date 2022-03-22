import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './components/app/app.component';
import { LoginComponent } from './components/login/login.component';

import { AuthService } from './services/auth.service';
import { AuthorizationInterceptor } from './interceptors/authorization-interceptor.service';
import { RefreshAccessInterceptor } from './interceptors/refresh-access-interceptor.service';

@NgModule({
    declarations: [
        AppComponent,
        LoginComponent
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