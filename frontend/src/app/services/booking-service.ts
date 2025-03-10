import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
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
        return this.http.post(`${MyConfig.APIurl}/api/Bookings/DeleteBookingsById`, { id: bookingId });
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
