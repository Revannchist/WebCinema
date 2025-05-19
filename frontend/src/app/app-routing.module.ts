// app-routing.module.ts
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GenresComponent } from './components/genres/genres.component';
import { CountriesComponent } from './components/countries/countries.component';
import { MoviesComponent } from './components/movies/movies.component';
import { ActorsComponent } from './components/actors/actors.component';
import { DirectorsComponent } from './components/directors/directors.component';
import { LandingPageComponent } from './components/landing-page/landing-page.component';
import { AdminPanelComponent} from './admin-panel/admin-panel/admin-panel.component';
import { AdminDashboardComponent } from './admin-panel/admin-dashboard/admin-dashboard.component';
import { AdminPanelMoviesComponent } from './admin-panel/admin-panel-movies/admin-panel-movies.component';
import { MovieListComponent } from './components/movie-list/movie-list.component';
import { UsersComponent } from './components/users/users.component';
import { AdminPanelUserAdminComponent } from './admin-panel/admin-panel-user-admin/admin-panel-user-admin.component';
import { LoginComponent } from './components/login/login.component';
import { AdminPanelShowtimesComponent } from './admin-panel/admin-panel-showtimes/admin-panel-showtimes.component';
import { ShowtimesListComponent } from './components/showtimes-list/showtimes-list.component';
import { SeatsComponent } from './components/seats/seats.component';
import { BookingsComponent } from './components/bookings/bookings.component';
import { BookingDetailsComponent } from './components/booking-details/booking-details.component';
import { HomeComponent } from './components/home/home.component';
import { AdminGuard } from './auth.guard';
import { AuthGuard } from './auth.guard';
import { PaymentComponent } from './components/payment/payment.component';

const routes: Routes = [
  // Public routes
  { path: '', redirectTo: 'landing-page', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'landing-page', component: LandingPageComponent },
  { path: 'login', component: LoginComponent },
  { path: 'users', component: UsersComponent },
  { path: 'movie-list', component: MovieListComponent },
  { path: 'showtimes-list', component: ShowtimesListComponent },

  // Auth protected routes (user must be logged in)
  {
    path: 'seats/:id',
    component: SeatsComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'bookings',
    component: BookingsComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'booking-details/:id',
    component: BookingDetailsComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'payment',
    component: PaymentComponent,
    canActivate: [AuthGuard]
  },

  // Admin routes - all protected by AdminGuard
  {
    path: 'admin',
    component: AdminPanelComponent,
    canActivate: [AdminGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: AdminDashboardComponent },
      { path: 'movies', component: AdminPanelMoviesComponent },
      { path: 'showtimes', component: AdminPanelShowtimesComponent },
      { path: 'users', component: AdminPanelUserAdminComponent },
      { path: 'genres', component: GenresComponent },
      { path: 'countries', component: CountriesComponent },
      { path: 'actors', component: ActorsComponent },
      { path: 'directors', component: DirectorsComponent },
    ]
  },

  // Wildcard route - redirect to home
  { path: '**', redirectTo: 'home' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {
    scrollPositionRestoration: 'enabled',
    anchorScrolling: 'enabled',
  })],
  exports: [RouterModule]
})
export class AppRoutingModule { }