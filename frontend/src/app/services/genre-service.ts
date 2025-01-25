import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Movie, PagedResponse, FilterParams } from '../models/movie.model';
import { MyConfig } from '../my-config';

@Injectable({
    providedIn: 'root'
})

export class GenreService {
    constructor(private http: HttpClient) { }

    getAllGenres(): Observable<any> {
        return this.http.get(`${MyConfig.APIurl}/api/Genres/GetAllGenres`);
    }

    getGenreById(id: number): Observable<any> {
        return this.http.get(`${MyConfig.APIurl}/api/Genres/GetGenresById?id=${id}`);
    }
}