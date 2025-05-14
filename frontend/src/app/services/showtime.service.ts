import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface Showtime {
  id: number;
  movieId: number;
  movieTitle: string;
  startTime: string;
  endTime: string;
  hallId: number;
  hallName: string;
  price: number;
}

@Injectable({
  providedIn: 'root'
})
export class ShowtimeService {
  private apiUrl = `${environment.apiUrl}/showtimes`;

  constructor(private http: HttpClient) {}

  getUpcomingShowtimes(): Observable<Showtime[]> {
    return this.http.get<Showtime[]>(`${this.apiUrl}/upcoming`);
  }

  getShowtimeById(id: number): Observable<Showtime> {
    return this.http.get<Showtime>(`${this.apiUrl}/${id}`);
  }

  getShowtimesByMovieId(movieId: number): Observable<Showtime[]> {
    return this.http.get<Showtime[]>(`${this.apiUrl}/movie/${movieId}`);
  }
} 