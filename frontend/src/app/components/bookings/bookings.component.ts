import { Component, OnInit } from '@angular/core';
import { BookingService } from '../../services/booking-service';
import { AuthService } from '../../auth.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { CartService } from '../../services/cart-service';

@Component({
  selector: 'app-bookings',
  templateUrl: './bookings.component.html',
  styleUrl: './bookings.component.css',
})
export class BookingsComponent implements OnInit {
  bookings: any[] = [];
  isLoading: boolean = false;
  error: string | null = null;

  constructor(
    private bookingService: BookingService,
    private authService: AuthService,
    private router: Router,
    private cartService: CartService
  ) { }

  ngOnInit(): void {
    this.loadUserBookings();
  }

  loadUserBookings(): void {
    this.isLoading = true;
    this.error = null;

    const token = this.authService.getDecodedToken();
    console.log('Full token payload:', token);
  
    const userName = this.authService.getCurrentUserName();
    if (!userName) {
      this.error = 'You must be logged in to view bookings';
      this.isLoading = false;
      console.log('Current User:', userName);
      return;
    }
  
    console.log('Current User:', userName);
    this.bookingService.getAllBookings().subscribe({
      next: (bookings) => {
        console.log('Raw bookings data:', bookings);
        this.bookings = bookings.filter((booking: any) => booking.userName === userName);
        console.log('Filtered bookings:', this.bookings);
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error loading bookings:', err);
        this.error = 'Failed to load bookings. Please try again.';
        this.isLoading = false;
      }
    });
  }
  

  deleteBooking(bookingId: number): void {
    if (confirm('Are you sure you want to cancel this booking?')) {
      this.bookingService.deleteBookingById(bookingId).subscribe({
        next: () => {
          this.bookings = this.bookings.filter(booking => booking.id !== bookingId);
          alert('Booking cancelled successfully!');
          
          // Update the cart count after deleting a booking
          const pendingBookings = this.bookings.filter(
            booking => booking.bookingStatus.toLowerCase() === 'pending'
          ).length;
          this.cartService.updateCartCount(pendingBookings);
        },
        error: (err) => {
          console.error('Error cancelling booking:', err);
          alert('Failed to cancel booking. Please try again.');
        }
      });
    }
  }

  viewBookingDetails(bookingId: number): void {
    this.router.navigate(['/booking-details', bookingId]);
  }

  proceedToCheckout(bookingId: number): void {
    this.router.navigate(['/checkout', bookingId]);
  }

  getBookingStatusClass(status: string): string {
    switch (status.toLowerCase()) {
      case 'pending':
        return 'status-pending';
      case 'confirmed':
        return 'status-confirmed';
      case 'cancelled':
        return 'status-cancelled';
      default:
        return '';
    }
  }

  formatDate(dateString: string): string {
    return new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }
}