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
import { RouterModule, Routes } from '@angular/router';
import { GenresComponent } from './components/genres/genres.component';
import { CountriesComponent } from './components/countries/countries.component';
import { MoviesComponent } from './components/movies/movies.component';
import { ActorsComponent } from './components/actors/actors.component';
import { DirectorsComponent } from './components/directors/directors.component';
import { LandingPageComponent } from './components/landing-page/landing-page.component';
import { AdminPanelComponentComponent } from './admin-panel/admin-panel/admin-panel.component.component';
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

export function tokenGetter() {
  return localStorage.getItem('token');
}

const routes: Routes = [
  { path: 'genres', component: GenresComponent },
  { path: 'countries', component: CountriesComponent },
  { path: 'actors', component: ActorsComponent },
  { path: 'directors', component: DirectorsComponent },
  { path: 'landing-page', component: LandingPageComponent },
  { path: 'movie-list', component: MovieListComponent },
  { path: 'users', component: UsersComponent },
  { path: 'showtimes-list', component: ShowtimesListComponent },


  { path: 'movies', component: MoviesComponent },
  { path: 'admin-panel-showtimes', component: AdminPanelShowtimesComponent },
  { path: 'admin-panel-movies', component: AdminPanelMoviesComponent },
  { path: 'showtimes', component: AdminPanelShowtimesComponent },
  { path: 'admin-panel-users', component: AdminPanelUserAdminComponent },
  { path: 'seats', component: SeatsComponent },


  {
    path: 'admin',
    component: AdminPanelComponentComponent,
    children: [
      { path: '', redirectTo: 'admin', pathMatch: 'full' }, // Default admin page
      { path: 'movies', component: AdminPanelMoviesComponent },
      { path: 'showtimes', component: AdminPanelShowtimesComponent },
      { path: 'users', component: AdminPanelUserAdminComponent },
    ]
  },

  { path: '**', redirectTo: 'landing-page' }
];

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
    AdminPanelComponentComponent,
    AdminPanelShowtimesComponent,
    AdminPanelMoviesComponent,
    AdminPanelUserAdminComponent,
    ShowtimesListComponent,
    SeatsComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    RouterModule.forRoot(routes, {

      scrollPositionRestoration: 'enabled',
      anchorScrolling: 'enabled',
    }),

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
