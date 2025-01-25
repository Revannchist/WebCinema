import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MyConfig } from '../my-config';

@Injectable({
    providedIn: 'root'
})
export class CountryService {
    constructor(private http: HttpClient) { }

    addCountry(country: any): Observable<any> {
        return this.http.post(`${MyConfig.APIurl}/api/Countries/AddCountry`, country);
    }

    deleteCountry(id: number): Observable<any> {
        return this.http.post(`${MyConfig.APIurl}/api/Countries/DeleteCountryById`, { id });
    }

    updateCountry(country: any): Observable<any> {
        return this.http.post(`${MyConfig.APIurl}/api/Countries/UpdateCountry`, country);
    }

    getCountryById(id: number): Observable<any> {
        return this.http.get(`${MyConfig.APIurl}/api/Countries/GetCountryById?id=${id}`);
    }

    getAllCountries(): Observable<any> {
        return this.http.get(`${MyConfig.APIurl}/api/Countries/GetAllCountries`);
    }
}