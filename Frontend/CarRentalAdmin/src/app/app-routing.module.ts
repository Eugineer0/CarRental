import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AuthGuard } from './_services/auth.guard';

import { LoginComponent } from './login/login.component';
import { WelcomeComponent } from './welcome/welcome.component';
import { SecretComponent } from './secret/secret.component';
import { RegisterComponent } from "./register/register.component";
import { CompleteRegistrationComponent } from "./complete-registration/complete-registration.component";
import { UsersComponent } from "./users/users.component";
import { UserInfoComponent } from "./user-info/user-info.component";

const routes: Routes = [
  {path: '', pathMatch: 'full', redirectTo: 'welcome'},
  {path: 'register', component: RegisterComponent},
  {path: 'login', component: LoginComponent},
  {path: 'completeRegistration', component: CompleteRegistrationComponent},
  {path: 'welcome', component: WelcomeComponent},
  {path: 'secret', canActivate: [AuthGuard], component: SecretComponent},
  {path: 'users', component: UsersComponent},
  {path: 'users/:username', component: UserInfoComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
