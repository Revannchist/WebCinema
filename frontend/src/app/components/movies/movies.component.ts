import { Component } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MyConfig } from '../../my-config';
import { Observable, take } from 'rxjs';

@Component({
  selector: 'app-movies',
  templateUrl: './movies.component.html',
  styleUrl: './movies.component.css'
})

export class MoviesComponent {
  constructor(private http:HttpClient){}
  public movies:any
  public movieToAdd:any = { moviesGenresIds: [], moviesActorsIds: [] }
  public search:any
  public filteredMovies:any
  private debounceTimer:any;
  public director: { id: number; firstName: string, lastName: string }[] = [];
  public country: { id: number; name: string }[] = [];
  public genres: any[] = [];
  public actors: any[] = [];

  ngOnInit():void{
    this.http.get(MyConfig.APIurl + '/api/Movies/GetAllMovies').subscribe(response=>{
      this.movies = response;
      this.filteredMovies = response;
      console.log(this.movies);
    })
    this.loadDirectors();
    this.loadCountries();
    this.loadGenres();
    this.loadActors();
  }

  loadGenres(): void {
    this.http.get(MyConfig.APIurl + '/api/Genres/GetAllGenres').subscribe((response : any) => 
      this.genres = response);
  }

  getGenreName(genreId: number): string {
    const genre = this.genres.find((g: any) => g.id === genreId);  
    return genre ? genre.name : 'N/A'; 
  }

  loadActors(): void {
    this.http.get(MyConfig.APIurl + '/api/Actors/GetAllActors').subscribe((response : any) => 
      this.actors = response);
  }

  getActorName(actorId: number): string {
    const actor = this.actors.find((a: any) => a.id === actorId);  
    return actor ? `${actor.firstName} ${actor.lastName}` : 'N/A'; 
  }

  loadDirectors(): void{
    this.http.get(MyConfig.APIurl + '/api/Directors/GetAllDirectors').subscribe((response: any) => {
      this.director = response;
    })
  }

  loadCountries(): void{
    this.http.get(MyConfig.APIurl + '/api/Countries/GetAllCountries').subscribe((response: any) => {
      this.country = response;
    })
  }

  getCountryName(countryId: number) {
    return this.country.find(c => c.id === countryId)?.name || 'N/A';
  }

  getDirectorName(directorId: number) {
    const director = this.director.find(d => d.id === directorId);
    return director ? `${director.firstName} ${director.lastName}` : 'N/A';  
  }

  deleteMovie(movie:any):void{
    this.http.post(MyConfig.APIurl + '/api/Movies/DeleteMovieById?id=' + movie.id, [{}]).subscribe(
      (response)=>{
        console.log('Movie delete response: ' + response);
      }
    )
  }
  
  clearModalTextBox():void{
    this.movieToAdd.name="";
  }
  
  addMovie(movieData: any): void {
    const body = {
      title: movieData.title,
      description: movieData.description,
      releaseDate: movieData.releaseDate,  
      duration: movieData.duration,
      language: movieData.language,
      ageRating: movieData.ageRating,
      directorId: movieData.directorId,
      countryId: movieData.countryId,
      moviesGenresIds: movieData.moviesGenresIds,
      moviesActorsIds: movieData.moviesActorsIds 
    };
    this.http.post<any>(`${MyConfig.APIurl}/api/Movies/CreateMovie`, body).subscribe(
      (response) => {
        this.movieToAdd = {}; 
      },
      (error) => {
        console.error('Error adding movie:', error);
      }
    );     
  }

  updateMovie(movieData: any): void {
    const body = {
      title: movieData.title,
      description: movieData.description,
      releaseDate: movieData.releaseDate,
      duration: movieData.duration,
      ageRating: movieData.ageRating,
      directorId: movieData.directorId,
      countryId: movieData.countryId,
    }; 
    this.http.put<any>(`${MyConfig.APIurl}/api/Movies/UpdateMovie`, body).subscribe(
      (response) => {
        console.log('Movie updated:', response);
        this.ngOnInit(); 
        this.movieToAdd = {};  
      },
      (error) => {
        console.error('Error updating movie:', error);
      }
    );
  }

  filterMovies(): void {
      clearTimeout(this.debounceTimer);
        this.debounceTimer = setTimeout(() => {
            if (!this.search) {
                this.filteredMovies = [...this.movies];
            } else {
                this.filteredMovies = this.movies.filter((movie: any) =>
                    movie.title.toLowerCase().includes(this.search.toLowerCase())
                );
            }
    }, 300); 
  }
}