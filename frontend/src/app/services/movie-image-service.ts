import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Movie, PagedResponse, FilterParams } from '../models/movie.model';
import { MyConfig } from '../my-config';

@Injectable({
    providedIn: 'root'
})

export class MovieImageService {
    constructor(private http: HttpClient) { }


    loadImageByMovieId(id:number){
        return this.http.post(`${MyConfig.APIurl}/api/MoviesImage/GetImagesByMovieId`, id);
    }

}
