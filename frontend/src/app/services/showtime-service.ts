import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MyConfig } from '../my-config';
import { AddShowTimeDto, GetShowTimeDto, UpdateShowTimeDto } from '../models/dto/showtime.dto';

@Injectable({
    providedIn: 'root'
})
export class ShowtimeService {
    constructor(private http: HttpClient) { }

    addShowTime(showTime: AddShowTimeDto): Observable<AddShowTimeDto> {
        return this.http.post<AddShowTimeDto>(`${MyConfig.APIurl}/api/ShowTimes/AddShowTime`, showTime);
    }

    updateShowTime(id: number, showTime: UpdateShowTimeDto): Observable<AddShowTimeDto> {
        const url = `${MyConfig.APIurl}/api/ShowTimes/UpdateShowTime?id=${id}`;
        return this.http.post<AddShowTimeDto>(url, showTime);
    }
    

    getShowTimeById(id: number): Observable<GetShowTimeDto> {
        return this.http.get<GetShowTimeDto>(`${MyConfig.APIurl}/api/ShowTimes/GetShowTimeById?id=${id}`);
    }

    getAllShowTimes(): Observable<GetShowTimeDto[]> {
        return this.http.get<GetShowTimeDto[]>(`${MyConfig.APIurl}/api/ShowTimes/GetAllShowTimes`);
    }

    deleteShowTime(id: number): Observable<void> {
        return this.http.post<void>(`${MyConfig.APIurl}/api/ShowTimes/DeleteShowTimeById?id=${id}`, [{}]);
    }
}
