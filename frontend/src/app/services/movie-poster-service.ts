import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MyConfig } from '../my-config';
import { CreateMoviePosterDto, MoviePosterResponseDto } from '../models/dto/move-poster.dto';
import { tap } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})

export class MoviePosterService {
    constructor(private http: HttpClient) { }
    ///api/MoviesPosters/

    addMoviePoster(posterData: CreateMoviePosterDto): Observable<boolean> {
        return this.http.post<boolean>(`${MyConfig.APIurl}/api/MoviesPosters/AddMoviePoster`, posterData);
    }

    deleteMoviePoster(id: number): Observable<boolean> {
        return this.http.post<boolean>(`${MyConfig.APIurl}/api/MoviesPosters//DeleteMoviePosterById?id=${id}`, [{}]);
    }

    getAllMoviePosters(): Observable<MoviePosterResponseDto[]> {
        return this.http.get<MoviePosterResponseDto[]>(`${MyConfig.APIurl}/api/MoviesPosters/GetAllMoviePosters`);
    }

    getPosterByMovieId(movieId: number): Observable<MoviePosterResponseDto> {
        const params = new HttpParams()
            .set('movieId', movieId.toString());
        return this.http.get<MoviePosterResponseDto>(`${MyConfig.APIurl}/api/MoviesPosters/GetPosterByMovieId`, { params })
            .pipe(
                tap(response => console.log('Poster response:', response))
            );
    }

    /*
    getPosterByMovieId(movieId: number): Observable<MoviePosterResponseDto> {
        return this.http.get<MoviePosterResponseDto>(`${MyConfig.APIurl}/api/MoviesImage/GetPosterByMovieId?id=${movieId}`);
    } */


    getMoviePosterByTitle(title: string): Observable<MoviePosterResponseDto> {
        let params = new HttpParams()
            .set('title', title);

        return this.http.get<MoviePosterResponseDto>(`${MyConfig.APIurl}/api/MoviesImage/GetMoviePosterByMovieTitle`, { params });
    }
}
