import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MyConfig } from '../my-config';


@Injectable({
    providedIn: 'root'
})
export class BookingService {
    constructor(private http: HttpClient) { }

    addBooking(bookingData: any): Observable<any> {
        return this.http.post(`${MyConfig.APIurl}/api/Bookings/AddBooking`, bookingData);
    }

    deleteBookingById(bookingId: number): Observable<any> {

        const params = new HttpParams().set('id', bookingId.toString());

        return this.http.post(`${MyConfig.APIurl}/api/Bookings/DeleteBookingsById`, null, {
            params: params
        });
    }

    updateBooking(bookingData: any): Observable<any> {
        return this.http.post(`${MyConfig.APIurl}/api/Bookings/UpdateBookings`, bookingData);
    }

    getBookingById(bookingId: number): Observable<any> {
        return this.http.get(`${MyConfig.APIurl}/api/Bookings/GetBookingsById?id=${bookingId}`);
    }

    getAllBookings(): Observable<any> {
        return this.http.get(`${MyConfig.APIurl}/api/Bookings/GetAllBookings`);
    }
}
