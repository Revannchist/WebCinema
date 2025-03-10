import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MyConfig } from '../my-config';
import { SeatsDto } from '../models/dto/seats.dto';

@Injectable({
    providedIn: 'root'
})
export class SeatService {
    constructor(private http: HttpClient) { }

    addSeat(seat: any): Observable<any> {
        return this.http.post<any>(`${MyConfig.APIurl}/api/Seats/AddSeats`, seat);
    }

    updateSeat(seat: any): Observable<any> {
        return this.http.post<any>(`${MyConfig.APIurl}/api/Seats/UpdateSeats`, seat);
    }

    getSeatById(id: number): Observable<SeatsDto> {
        return this.http.get<SeatsDto>(`${MyConfig.APIurl}/api/Seats/GetSeatsById?id=${id}`);
    }

    getAllSeats(): Observable<SeatsDto[]> {
        return this.http.get<SeatsDto[]>(`${MyConfig.APIurl}/api/Seats/GetAllSeats`);
    }

    deleteSeat(id: number): Observable<void> {
        return this.http.post<void>(`${MyConfig.APIurl}/api/Seats/DeleteSeatsById`, { id });
    }
}