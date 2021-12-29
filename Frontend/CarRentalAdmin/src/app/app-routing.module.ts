import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AuthGuard } from './auth.guard';

import { LoginComponent } from './login/login.component';
import { WelcomeComponent } from './welcome/welcome.component';
import { SecretComponent } from './secret/secret.component';

const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'welcome'},
  { path: 'login', component: LoginComponent},
  { path: 'welcome', component: WelcomeComponent},
  { path: 'secret', canActivate: [AuthGuard], component: SecretComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
