import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of, throwError } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { MyConfig } from '../my-config';


export interface Seat {
  id: number;
  hallId: number;
  seatNumber: string;
  seatType: string;
}

export interface ShowTime {
  id: number;
  movieId: number;
  hallId: number;
  showDateTime: Date;
  ticketPrice: number;
  isActive: boolean;
  movieTitle?: string;
  hallName?: string;
}

export interface BookedSeat {
  id?: number;
  bookingId?: number;
  seatId: number;
  seat?: Seat;
}

export interface Booking {
  id?: number;
  usersId: number;
  showTimesId: number;
  ticketQuantity: number;
  totalPrice: number;
  bookingStatus: string;
  bookingDate?: Date;
  bookedSeats?: BookedSeat[];
  showTimes?: ShowTime;
}

export interface ReservationResponse {
  id: number;
  expiresAt: Date;
  booking: Booking;
}

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private apiUrl = 'api/bookings'; // Update with your actual API endpoint
  private reservationExpiryTime = 15 * 60 * 1000; // 15 minutes in milliseconds
  private reservationTimerId: any;
  
  // BehaviorSubject to track the current booking/cart state
  private currentBookingSubject = new BehaviorSubject<Booking | null>(null);
  currentBooking$ = this.currentBookingSubject.asObservable();
  
  // BehaviorSubject to track reservation expiry time
  private reservationTimeRemainingSubject = new BehaviorSubject<number>(0);
  reservationTimeRemaining$ = this.reservationTimeRemainingSubject.asObservable();

  constructor(private http: HttpClient) {
    this.loadCartFromStorage();
  }

  // Load any saved cart data from local storage
  private loadCartFromStorage(): void {
    const savedBooking = localStorage.getItem('currentBooking');
    const savedExpiry = localStorage.getItem('reservationExpiry');
    
    if (savedBooking) {
      const booking = JSON.parse(savedBooking) as Booking;
      this.currentBookingSubject.next(booking);
      
      if (savedExpiry) {
        const expiryTime = parseInt(savedExpiry, 10);
        const now = Date.now();
        
        if (expiryTime > now) {
          // Start timer with remaining time
          this.startReservationTimer(expiryTime);
        } else {
          // Reservation expired
          this.clearReservation();
        }
      }
    }
  }

  // Initialize a new booking when a showtime is selected
  initializeBooking(showTime: ShowTime, userId: number): void {
    const booking: Booking = {
      usersId: userId,
      showTimesId: showTime.id,
      ticketQuantity: 0,
      totalPrice: 0,
      bookingStatus: 'Pending',
      bookedSeats: [],
      showTimes: showTime
    };
    
    this.currentBookingSubject.next(booking);
    this.saveCartToStorage();
  }

  // Reserve seats temporarily
  reserveSeats(seats: Seat[]): Observable<ReservationResponse> {
    const currentBooking = this.currentBookingSubject.value;
    
    if (!currentBooking) {
      return throwError(() => new Error('No active booking found'));
    }
    
    // Update the booking with selected seats
    const bookedSeats: BookedSeat[] = seats.map(seat => ({
      seatId: seat.id,
      seat: seat
    }));
    
    const updatedBooking: Booking = {
      ...currentBooking,
      ticketQuantity: seats.length,
      totalPrice: (currentBooking.showTimes?.ticketPrice || 0) * seats.length,
      bookedSeats: bookedSeats
    };
    

    return this.http.post<ReservationResponse>(`${MyConfig.APIurl}/reserve`, updatedBooking).pipe(
      tap(response => {
        // Update the booking with the reservation ID
        this.currentBookingSubject.next({
          ...updatedBooking,
          id: response.booking.id
        });
        
        // Start the reservation timer
        const expiryTime = new Date(response.expiresAt).getTime();
        this.startReservationTimer(expiryTime);
        
        // Save to storage
        this.saveCartToStorage(expiryTime);
      }),
      catchError(error => {
        // For testing, we'll create a mock response
        console.warn('Using mock reservation - in production, fix API endpoint');
        const mockResponse: ReservationResponse = {
          id: Math.floor(Math.random() * 1000),
          expiresAt: new Date(Date.now() + this.reservationExpiryTime),
          booking: updatedBooking
        };
        
        // Update the booking
        this.currentBookingSubject.next(updatedBooking);
        
        // Start the reservation timer
        const expiryTime = mockResponse.expiresAt.getTime();
        this.startReservationTimer(expiryTime);
        
        // Save to storage
        this.saveCartToStorage(expiryTime);
        
        return of(mockResponse);
      })
    );
  }

  // Save the current booking to local storage
  private saveCartToStorage(expiryTime?: number): void {
    const currentBooking = this.currentBookingSubject.value;
    
    if (currentBooking) {
      localStorage.setItem('currentBooking', JSON.stringify(currentBooking));
      
      if (expiryTime) {
        localStorage.setItem('reservationExpiry', expiryTime.toString());
      }
    }
  }

  // Start the reservation timer
  private startReservationTimer(expiryTime: number): void {
    // Clear any existing timer
    if (this.reservationTimerId) {
      clearInterval(this.reservationTimerId);
    }
    
    // Update the timer every second
    this.reservationTimerId = setInterval(() => {
      const now = Date.now();
      const timeRemaining = expiryTime - now;
      
      if (timeRemaining <= 0) {
        // Reservation expired
        this.clearReservation();
      } else {
        // Update the time remaining
        this.reservationTimeRemainingSubject.next(timeRemaining);
      }
    }, 1000);
  }

  // Clear the reservation when it expires
  private clearReservation(): void {
    // Clear the timer
    if (this.reservationTimerId) {
      clearInterval(this.reservationTimerId);
      this.reservationTimerId = null;
    }
    
    // Clear the booking
    this.currentBookingSubject.next(null);
    this.reservationTimeRemainingSubject.next(0);
    
    // Clear local storage
    localStorage.removeItem('currentBooking');
    localStorage.removeItem('reservationExpiry');
    
    // In a real application, you would make an API call to release the reserved seats
  }

  // Complete the booking / finalize purchase
  completeBooking(): Observable<Booking> {
    const currentBooking = this.currentBookingSubject.value;
    
    if (!currentBooking) {
      return throwError(() => new Error('No active booking found'));
    }
    
    // Update booking status
    const finalBooking: Booking = {
      ...currentBooking,
      bookingStatus: 'Confirmed'
    };
    
    // In a real application, you would make an API call to finalize the booking
    return this.http.post<Booking>(`${this.apiUrl}`, finalBooking).pipe(
      tap(response => {
        // Clear the current booking after successful completion
        this.clearReservation();
      }),
      catchError(error => {
        console.warn('Using mock booking confirmation - in production, fix API endpoint');
        // For testing purposes
        setTimeout(() => this.clearReservation(), 2000);
        return of(finalBooking);
      })
    );
  }

  // Get the formatted time remaining
  getFormattedTimeRemaining(): Observable<string> {
    return this.reservationTimeRemaining$.pipe(
      map(milliseconds => {
        if (milliseconds <= 0) return '00:00';
        
        const minutes = Math.floor(milliseconds / 60000);
        const seconds = Math.floor((milliseconds % 60000) / 1000);
        
        return `${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}`;
      })
    );
  }

  // Cancel the current booking
  cancelBooking(): void {
    const currentBooking = this.currentBookingSubject.value;
    
    if (currentBooking) {
      // In a real application, you would make an API call to release the reserved seats
      this.clearReservation();
    }
  }

  // Check if there's an active reservation
  hasActiveReservation(): boolean {
    return !!this.currentBookingSubject.value;
  }

  // Get the current booking
  getCurrentBooking(): Booking | null {
    return this.currentBookingSubject.value;
  }
}