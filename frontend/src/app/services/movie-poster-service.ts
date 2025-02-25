import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { MyConfig } from '../my-config';
import { CreateMoviePosterDto, MoviePosterResponseDto } from '../models/dto/move-poster.dto';
import { tap } from 'rxjs/operators';
import { switchMap, catchError } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})

export class MoviePosterService {
    constructor(private http: HttpClient) { }

    addMoviePoster(posterData: CreateMoviePosterDto): Observable<boolean> {
        return this.http.post<boolean>(`${MyConfig.APIurl}/api/MoviesPosters/AddMoviePoster`, posterData);
    }

    deleteMoviePoster(id: number): Observable<boolean> {
        return this.http.post<boolean>(`${MyConfig.APIurl}/api/MoviesPosters/DeleteMoviePosterById?imageId=${id}`, null);
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

    getMoviePosterByTitle(title: string): Observable<MoviePosterResponseDto> {
        let params = new HttpParams()
            .set('title', title);

        return this.http.get<MoviePosterResponseDto>(`${MyConfig.APIurl}/api/MoviesImage/GetMoviePosterByMovieTitle`, { params });
    }


    //--------------------------------------------------------------------------------------

    updateMoviePoster(movieId: number, imageData: string | null): Observable<boolean> {
        if (!imageData) {
            return of(true);
        }

        if (imageData === "DELETE_POSTER") {
            return this.deleteMoviePosterForMovie(movieId);
        }

        return this.addOrReplaceMoviePoster(movieId, imageData);
    }

    private addOrReplaceMoviePoster(movieId: number, imageData: string): Observable<boolean> {
        const posterDto: CreateMoviePosterDto = {
            id: 0,
            movieId: movieId,
            image: imageData
        };

        return this.http.post<boolean>(`${MyConfig.APIurl}/api/MoviesPosters/AddMoviePoster`, posterDto);
    }

    private deleteMoviePosterForMovie(movieId: number): Observable<boolean> {
        return this.getPosterByMovieId(movieId).pipe(
            switchMap(poster => {
                if (poster && poster.id) {
                    return this.deleteMoviePoster(poster.id);
                }
                return of(true);
            }),
            catchError(error => {
                console.error('Error handling poster deletion:', error);
                return of(false);
            })
        );
    }
}
