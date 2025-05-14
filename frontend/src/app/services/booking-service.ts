import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { catchError } from 'rxjs';
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

    updateBooking(id: number, bookingData: any): Observable<any> {
        const params = new HttpParams().set('id', id.toString());
        return this.http.post(`${MyConfig.APIurl}/api/Bookings/UpdateBookings`, bookingData, { params });
    }

    getBookingById(bookingId: number): Observable<any> {
        return this.http.get(`${MyConfig.APIurl}/api/Bookings/GetBookingsById?id=${bookingId}`);
    }

    /*
    getAllBookings(): Observable<any> {
        return this.http.get(`${MyConfig.APIurl}/api/Bookings/GetAllBookings`);
    }
    */

    getAllBookings(): Observable<any[]> {
        return this.http.get<any[]>(`${MyConfig.APIurl}/api/Bookings/GetAllBookings`).pipe(
            catchError(error => {
                if (error.status === 404 && error.error === "No bookings found") {
                    return of([]); // Return an empty array instead of throwing an error
                }
                return throwError(() => error);
            })
        );
    }

}
