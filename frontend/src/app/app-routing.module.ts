import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UsersComponent } from './components/users/users.component';
import { LandingPageComponent } from './components/landing-page/landing-page.component';
import { AdminPanelUserAdminComponent } from './admin-panel/admin-panel-user-admin/admin-panel-user-admin.component';
import { LoginComponent } from './components/login/login.component';
import { MovieListComponent } from './components/movie-list/movie-list.component';
import { AuthGuard } from './auth.guard';
import { SeatsComponent } from './components/seats/seats.component';
import { BookingService } from './services/booking-service';
import { BookingsComponent } from './components/bookings/bookings.component';

const routes: Routes = [
  { path: '', component: LandingPageComponent },
  { path: 'login', component: LoginComponent },
  { path: 'users', component: UsersComponent },
  { path: 'admin-panel-user-admin', component: AdminPanelUserAdminComponent },
  { path: 'seats/:id', component: SeatsComponent },
  { path: 'bookings', component: BookingsComponent },

  //,
  //{ path: 'movie-list', component: MovieListComponent, canActivate: [AuthGuard] },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
