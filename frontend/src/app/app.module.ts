import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { RouterModule, Routes } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { GenresComponent } from './components/genres/genres.component';
import { CountriesComponent } from './components/countries/countries.component';
import { FormsModule } from '@angular/forms';
import { MoviesComponent } from './components/movies/movies.component';
import { ActorsComponent } from './components/actors/actors.component';
import { DirectorsComponent } from './components/directors/directors.component';
import { LandingPageComponent } from './components/landing-page/landing-page.component';
import { AdminPanelComponentComponent } from './admin-panel/admin-panel.component/admin-panel.component.component';
import { AdminPanelMoviesComponent } from './admin-panel/admin-panel-movies/admin-panel-movies.component';
import { MovieListComponent } from './components/movie-list/movie-list.component';

const routes:Routes=[
  //{path:'test',component:TestComponent},
  {path:'genres',component:GenresComponent},
  {path:'countries',component:CountriesComponent},
  {path:'movies',component:MoviesComponent},
  {path:'actors',component:ActorsComponent},
  {path:'directors',component:DirectorsComponent},
  {path:'landing-page',component:LandingPageComponent},
  {path:'admin-panel-movies',component:AdminPanelMoviesComponent},
  {path:'movie-list',component:MovieListComponent}
]
@NgModule({
  declarations: [
    AppComponent,
    //TestComponent,
    GenresComponent,
    CountriesComponent,
    MoviesComponent,
    ActorsComponent,
    DirectorsComponent,
    LandingPageComponent,
    AdminPanelComponentComponent,
    AdminPanelMoviesComponent,
    MovieListComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    RouterModule.forRoot(routes),
    HttpClientModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
