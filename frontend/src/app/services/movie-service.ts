import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Movie, PagedResponse, FilterParams } from '../models/movie.model';
import { MyConfig } from '../my-config';
import { map } from 'rxjs/operators';
//import { MovieCreateDto, MovieUpdateDto, MovieGetDto, MovieParameters, MoviePagedResponse } from '../../models/dto/movie.dto';
import { MovieCreateDto, MovieUpdateDto, MovieGetDto, MovieParameters, MoviePagedResponse } from '../models/dto/movie.dto';

@Injectable({
    providedIn: 'root'
})
export class MovieService {
    constructor(private http: HttpClient) { }

    private getHeaders(): HttpHeaders {

        const token = localStorage.getItem('token');

        //console.log('Auth token:', token);

        return new HttpHeaders({
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        });
    }

    getMovies(filterParams: MovieParameters): Observable<MoviePagedResponse<MovieGetDto>> {
        let params = new HttpParams()
            .set('pageNumber', filterParams.pageNumber.toString())
            .set('pageSize', filterParams.pageSize.toString());

        if (filterParams.searchTerm) {
            params = params.set('searchTerm', filterParams.searchTerm);
        }
        if (filterParams.directorId) {
            params = params.set('directorId', filterParams.directorId.toString());
        }
        if (filterParams.genreIds?.length) {
            filterParams.genreIds.forEach(id => {
                params = params.append('genreIds', id.toString());
            });
        }
        if (filterParams.actorIds?.length) {
            filterParams.actorIds.forEach(id => {
                params = params.append('actorIds', id.toString());
            });
        }
        if (filterParams.fromDate) params = params.set('fromDate', filterParams.fromDate);
        if (filterParams.toDate) params = params.set('toDate', filterParams.toDate);
        if (filterParams.language) params = params.set('language', filterParams.language);
        if (filterParams.ageRating) params = params.set('ageRating', filterParams.ageRating);
        if (filterParams.countryId) params = params.set('countryId', filterParams.countryId.toString());

        return this.http.get<MoviePagedResponse<MovieGetDto>>(`${MyConfig.APIurl}/api/Movies/GetAllMovies`, { params });
    }

    getAllMoviesSimple(): Observable<MovieGetDto[]> {
        return this.http.get<MoviePagedResponse<MovieGetDto>>(`${MyConfig.APIurl}/api/Movies/GetAllMovies`)
            .pipe(
                map(response => response.items)
            );
    }

    createMovie(movie: MovieCreateDto): Observable<MovieGetDto> {
        return this.http.post<MovieGetDto>(`${MyConfig.APIurl}/api/Movies/CreateMovie`, movie);
    }



    /*
    updateMovie(id: number, movie: MovieUpdateDto): Observable<MovieGetDto> {
        const url = `${MyConfig.APIurl}/api/Movies/UpdateMovie?id=${id}`;
        return this.http.post<MovieGetDto>(url, movie);
    }
    */

    updateMovie(id: number, movie: MovieUpdateDto): Observable<MovieGetDto> {
        //console.log('Using updated method with headers');
        const headers = this.getHeaders();
        const url = `${MyConfig.APIurl}/api/Movies/UpdateMovie?id=${id}`;
        return this.http.post<MovieGetDto>(url, movie, { headers });
    }
    


    getMovieById(id: number): Observable<MovieGetDto> {
        return this.http.get<MovieGetDto>(`${MyConfig.APIurl}/api/Movies/GetMovieById?id=${id}`);
    }

    deleteMovie(id: number): Observable<any> {
        return this.http.post(`${MyConfig.APIurl}/api/Movies/DeleteMovieById?id=${id}`, [{}]);
    }
} 