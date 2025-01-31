import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UsersComponent } from './components/users/users.component';
import { LandingPageComponent } from './components/landing-page/landing-page.component';
import { AdminPanelUserAdminComponent } from './admin-panel/admin-panel-user-admin/admin-panel-user-admin.component';

const routes: Routes = [
  { path: '', component: LandingPageComponent },
  { path: 'users', component: UsersComponent },
  { path: 'admin-panel-user-admin', component: AdminPanelUserAdminComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
