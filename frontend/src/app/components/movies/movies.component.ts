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
  
  addMovie(movieData: any): Observable<any> {
    const params = new HttpParams()
      .set('title', movieData.title)
      .set('description', movieData.description)
      .set('releaseDate', movieData.releaseDate)  //YYYY-MM-DD
      .set('duration', movieData.duration.toString())  
      .set('language', movieData.language.toString())
      .set('ageRating',movieData.ageRating)
      .set('directorId', movieData.directorId)  
      .set('countryId', movieData.countryId);  

    return this.http.post<any>(`${MyConfig.APIurl}/api/Movies/AddMovie`, {}, { params });
  }

  filterMovies():void{
    if(!this.search){
      this.filteredMovies = this.movies;
    }
    else{
    this.filteredMovies = this.movies.filter((movie:any) => 
      movie.title.toLowerCase().includes(this.search.toLowerCase())
    );
  }
  }
  
}

