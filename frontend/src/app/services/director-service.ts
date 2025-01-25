import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MyConfig } from '../my-config';

@Injectable({
    providedIn: 'root'
})
export class DirectorService {
    constructor(private http: HttpClient) { }

    addDirector(director: any): Observable<any> {
        return this.http.post(`${MyConfig.APIurl}/api/Directors/AddDirector`, director);
    }

    updateDirector(director: any): Observable<any> {
        return this.http.post(`${MyConfig.APIurl}/api/Directors/UpdateDirector`, director);
    }

    getDirectorById(id: number): Observable<any> {
        return this.http.get(`${MyConfig.APIurl}/api/Directors/GetDirectorById?id=${id}`);
    }

    getAllDirectors(): Observable<any> {
        return this.http.get(`${MyConfig.APIurl}/api/Directors/GetAllDirectors`);
    }

    deleteDirector(id: number): Observable<any> {
        return this.http.post(`${MyConfig.APIurl}/api/Directors/DeleteDirectorById`, { id });
    }
}