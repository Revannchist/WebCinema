import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AppRoutingModule } from './app-routing.module';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { JwtModule } from '@auth0/angular-jwt';
import { AppComponent } from './app.component';
import { AuthInterceptor } from './auth.interceptor';
import { GenresComponent } from './components/genres/genres.component';
import { CountriesComponent } from './components/countries/countries.component';
import { MoviesComponent } from './components/movies/movies.component';
import { ActorsComponent } from './components/actors/actors.component';
import { DirectorsComponent } from './components/directors/directors.component';
import { LandingPageComponent } from './components/landing-page/landing-page.component';
import { AdminPanelComponent } from './admin-panel/admin-panel/admin-panel.component';
import { AdminPanelMoviesComponent } from './admin-panel/admin-panel-movies/admin-panel-movies.component';
import { MovieListComponent } from './components/movie-list/movie-list.component';
import { UsersComponent } from './components/users/users.component';
import { AdminPanelUserAdminComponent } from './admin-panel/admin-panel-user-admin/admin-panel-user-admin.component';
import { LoginComponent } from './components/login/login.component';
import { PasswordStrengthComponent } from './components/password-strength/password-strength.component';
import { AdminPanelShowtimesComponent } from './admin-panel/admin-panel-showtimes/admin-panel-showtimes.component';
import { SidebarComponent } from './admin-panel/sidebar/sidebar.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ShowtimesListComponent } from './components/showtimes-list/showtimes-list.component';
import { SeatsComponent } from './components/seats/seats.component';
import { BookingService } from './services/booking-service';
import { BookingsComponent } from './components/bookings/bookings.component';
import { BookingDetailsComponent } from './components/booking-details/booking-details.component';
import { HomeComponent } from './components/home/home.component';

export function tokenGetter() {
  return localStorage.getItem('token');
}

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}

@NgModule({
  declarations: [
    AppComponent,
    GenresComponent,
    CountriesComponent,
    MoviesComponent,
    ActorsComponent,
    DirectorsComponent,
    LandingPageComponent,
    MovieListComponent,
    UsersComponent,
    LoginComponent,
    PasswordStrengthComponent,
    SidebarComponent,
    AdminPanelComponent,
    AdminPanelShowtimesComponent,
    AdminPanelMoviesComponent,
    AdminPanelUserAdminComponent,
    ShowtimesListComponent,
    SeatsComponent,
    BookingsComponent,
    BookingDetailsComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    TranslateModule.forRoot({
      defaultLanguage: 'en',
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    }),
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: ['localhost:44318'],
        disallowedRoutes: ['https://localhost:44318/api/auth/login'] // Routes where JWT should not be sent
      }
    })
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }