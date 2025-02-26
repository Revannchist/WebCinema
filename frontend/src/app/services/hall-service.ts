import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MyConfig } from '../my-config';
import { HallDisplayDto } from '../models/dto/showtime.dto';
import { AddHallDto } from '../models/dto/halls.dto';

@Injectable({
    providedIn: 'root'
})
export class HallService {
    constructor(private http: HttpClient) { }

    addHall(hall: AddHallDto): Observable<AddHallDto> {
        return this.http.post<AddHallDto>(`${MyConfig.APIurl}/api/Halls/AddHalls`, hall);
    }

    updateHall(hall: AddHallDto): Observable<AddHallDto> {
        return this.http.post<AddHallDto>(`${MyConfig.APIurl}/api/Halls/UpdateHalls`, hall);
    }

    getHallById(id: number): Observable<HallDisplayDto> {
        return this.http.get<HallDisplayDto>(`${MyConfig.APIurl}/api/Halls/GetHallsById?id=${id}`);
    }

    getAllHalls(): Observable<HallDisplayDto[]> {
        return this.http.get<HallDisplayDto[]>(`${MyConfig.APIurl}/api/Halls/GetAllHalls`);
    }

    deleteHall(id: number): Observable<void> {
        return this.http.post<void>(`${MyConfig.APIurl}/api/Halls/DeleteHallsById`, { id });
    }
}