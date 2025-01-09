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
  public movieToAdd:any={}
  public search:any
  public filteredMovies:any
  private debounceTimer: any;


  ngOnInit():void{
    this.http.get(MyConfig.APIurl + '/api/Movies/GetAllMovies').subscribe(x=>{
      this.movies = x;
      this.filteredMovies = x;
      console.log(this.movies);
    })
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
    this.http.post<any>(`${MyConfig.APIurl}/api/Movies/AddMovie`, body).subscribe(
      (response) => {
        this.movieToAdd = {}; 
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