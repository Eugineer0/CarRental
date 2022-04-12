import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { SecretComponent } from './components/secret/secret.component';
import { AuthGuard } from './services/auth-guard';

const routes: Routes = [
    { path: '', pathMatch: 'full', redirectTo: '' },
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
    { path: 'secret', canActivate: [AuthGuard], component: SecretComponent }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule {
}