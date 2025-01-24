import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Movie, PagedResponse, FilterParams } from '../models/movie.model';
import { MyConfig } from '../my-config';

@Injectable({
    providedIn: 'root'
})
export class MovieService {
    constructor(private http: HttpClient) { }

    private formatDate(date: string | null): string | null {
        if (!date) return null;
        const d = new Date(date);
        return isNaN(d.getTime()) ? null : d.toISOString();
    }

    getMovies(filterParams: FilterParams): Observable<PagedResponse<Movie>> {
        let params = new HttpParams()
            .set('pageNumber', filterParams.pageNumber.toString())
            .set('pageSize', filterParams.pageSize.toString());

        if (filterParams.searchTerm?.trim()) {
            params = params.set('searchTerm', filterParams.searchTerm.trim());
        }

        if (filterParams.directorId) {
            params = params.set('directorId', filterParams.directorId.toString());
        }

        filterParams.genreIds?.forEach(id => {
            params = params.append('genreIds', id.toString());
        });

        filterParams.actorIds?.forEach(id => {
            params = params.append('actorsIds', id.toString());
        });

        const fromDate = this.formatDate(filterParams.fromDate?.toString() ?? null);
        if (fromDate) params = params.set('fromDate', fromDate);

        const toDate = this.formatDate(filterParams.toDate?.toString() ?? null);
        if (toDate) params = params.set('toDate', toDate);

        if (filterParams.language) params = params.set('language', filterParams.language);
        if (filterParams.ageRating) params = params.set('ageRating', filterParams.ageRating);
        if (filterParams.countryId) params = params.set('countryId', filterParams.countryId.toString());

        return this.http.get<PagedResponse<Movie>>(`${MyConfig.APIurl}/api/Movies/GetAllMovies`, { params });
    }

    createMovie(movie: Movie): Observable<Movie> {
        return this.http.post<Movie>(`${MyConfig.APIurl}/api/Movies/CreateMovie`, movie);
    }

    updateMovie(id: number, movie: Movie): Observable<Movie> {
        return this.http.post<Movie>(`${MyConfig.APIurl}/api/Movies/UpdateMovie?id=${id}`, movie);
    }

    deleteMovie(id: number): Observable<any> {
        return this.http.post(`${MyConfig.APIurl}/api/Movies/DeleteMovieById?id=${id}`, [{}]);
    }
}
