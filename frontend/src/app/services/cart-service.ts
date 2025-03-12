import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { BookingService } from '../services/booking-service';
import { AuthService } from '../auth.service';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private cartItemCountSubject = new BehaviorSubject<number>(0);
  cartItemCount$ = this.cartItemCountSubject.asObservable();

  constructor(
    private bookingService: BookingService,
    private authService: AuthService
  ) {}

  updateCartCount(count: number): void {
    this.cartItemCountSubject.next(count);
    console.log('Updated cart count:', count);
  }

  refreshCartCount(): void {
    const userName = this.authService.getCurrentUserName();
    if (!userName) {
      this.updateCartCount(0);
      return;
    }

    this.bookingService.getAllBookings().subscribe({
      next: (bookings) => {
        console.log('All bookings for cart count:', bookings);
        
        // Only count bookings that are in 'pending' status
        const count = bookings.filter((booking: any) => 
          booking.userName?.trim().toLowerCase() === userName.trim().toLowerCase() && 
          booking.bookingStatus.toLowerCase() === 'pending'
        ).length;
        
        console.log('Filtered pending bookings count:', count);
        this.updateCartCount(count);
      },
      error: (err) => {
        console.error('Error refreshing cart count:', err);
        this.updateCartCount(0);
      }
    });
  }
}