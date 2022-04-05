import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AuthGuard } from './_services/auth.guard';

import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { CompleteRegistrationComponent } from './complete-registration/complete-registration.component';
import { UsersComponent } from './users/users.component';
import { UserInfoComponent } from './user-info/user-info.component';
import { RentalCentersComponent } from './rental-centers/rental-centers.component';
import { RentalCenterInfoComponent } from './rental-center-info/rental-center-info.component';

const routes: Routes = [
    {path: '', pathMatch: 'full', redirectTo: 'home'},
    {path: 'register', component: RegisterComponent},
    {path: 'login', component: LoginComponent},
    {path: 'completeRegistration', component: CompleteRegistrationComponent},
    {path: 'users', component: UsersComponent},
    {path: 'users/:username', component: UserInfoComponent},
    {path: 'rental-centers', component: RentalCentersComponent},
    {path: 'rental-centers/:name', component: RentalCenterInfoComponent}
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule {
}
