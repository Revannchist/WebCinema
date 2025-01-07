import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { RouterModule, Routes } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { GenresComponent } from './components/genres/genres.component';
import { CountriesComponent } from './components/countries/countries.component';
import { FormsModule } from '@angular/forms';

const routes:Routes=[
  //{path:'test',component:TestComponent},
  {path:'genres',component:GenresComponent},
  {path:'countries',component:CountriesComponent}
]
@NgModule({
  declarations: [
    AppComponent,
    //TestComponent,
    GenresComponent,
    CountriesComponent,
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
