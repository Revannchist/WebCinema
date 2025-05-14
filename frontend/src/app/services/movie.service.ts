import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface Movie {
  id: number;
  title: string;
  description: string;
  duration: number;
  releaseDate: string;
  posterUrl: string;
  rating: number;
  language: string;
}

@Injectable({
  providedIn: 'root'
})
export class MovieService {
  private apiUrl = `${environment.apiUrl}/api/Movies`;

  constructor(private http: HttpClient) {}

  getFeaturedMovies(): Observable<Movie[]> {
    return this.http.get<Movie[]>(`${this.apiUrl}/featured`);
  }

  getMovieById(id: number): Observable<Movie> {
    return this.http.get<Movie>(`${this.apiUrl}/GetMovieById?id=${id}`);
  }

  getAllMovies(): Observable<Movie[]> {
    return this.http.get<Movie[]>(`${this.apiUrl}/GetAllMovies`);
  }
} 