import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';

import { AuthService } from './_services/auth.service';
import { AuthorizationInterceptor } from "./_services/authorization-interceptor.service";
import { RefreshAccessInterceptor } from "./_services/refresh-access-interceptor.service";

import { FieldConstraintDirective } from './field-constraint.directive';

import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { CompleteRegistrationComponent } from './complete-registration/complete-registration.component';
import { UsersComponent } from './users/users.component';
import { UserInfoComponent } from './user-info/user-info.component';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { RentalCentersComponent } from './rental-centers/rental-centers.component';
import { RentalCenterInfoComponent } from './rental-center-info/rental-center-info.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    FieldConstraintDirective,
    RegisterComponent,
    CompleteRegistrationComponent,
    UsersComponent,
    UserInfoComponent,
    RentalCentersComponent,
    RentalCenterInfoComponent,
  ],
  imports: [
    AppRoutingModule,
    HttpClientModule,
    BrowserModule,
    FormsModule,
    NoopAnimationsModule
  ],
  providers: [
    AuthService,
    {provide: HTTP_INTERCEPTORS, useClass: AuthorizationInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: RefreshAccessInterceptor, multi: true},
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
