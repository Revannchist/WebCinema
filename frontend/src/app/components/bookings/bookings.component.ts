import { Component, OnInit } from '@angular/core';
import { BookingService } from '../../services/booking-service';
import { AuthService } from '../../auth.service';
import { Router } from '@angular/router';
import { CartService } from '../../services/cart-service';

@Component({
  selector: 'app-bookings',
  templateUrl: './bookings.component.html',
  styleUrl: './bookings.component.css',
})
export class BookingsComponent implements OnInit {
  bookings: any[] = [];
  pendingBookings: any[] = [];
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
     
    const userName = this.authService.getCurrentUserName();
    //console.log('Current User Name:', userName);
     
    if (!userName) {
      this.error = 'You must be logged in to view bookings';
      this.isLoading = false;
      return;
    }
     
    this.bookingService.getAllBookings().subscribe({
      next: (bookings) => {
        //console.log('All Bookings:', bookings);
         
        // Get all bookings for this user
        this.bookings = bookings.filter((booking: any) =>
          booking.userName?.trim().toLowerCase() === userName.trim().toLowerCase()
        );
         
        // Filter only pending bookings
        this.pendingBookings = this.bookings.filter(
          booking => booking.bookingStatus.toLowerCase() === 'pending'
        );
         
        console.log('Filtered Bookings:', this.bookings);
        console.log('Pending Bookings:', this.pendingBookings);
        this.isLoading = false;
         
        // Update cart count - only include PENDING bookings
        this.cartService.updateCartCount(this.pendingBookings.length);
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
          // Remove the deleted booking from both arrays
          this.bookings = this.bookings.filter(booking => booking.id !== bookingId);
          this.pendingBookings = this.pendingBookings.filter(booking => booking.id !== bookingId);
          alert('Booking cancelled successfully!');
           
          // Update cart count with the new pendingBookings count
          this.cartService.updateCartCount(this.pendingBookings.length);
        },
        error: (err) => {
          console.error('Error cancelling booking:', err);
          alert('Failed to cancel booking. Please try again.');
        }
      });
    }
  }
  
  viewBookingDetails(showtimeId: number | undefined) {
    //console.log('Navigating to seats with showtimeId:', showtimeId);
  
    if (!showtimeId) {
      console.error('Error: showtimeId is undefined!');
      return;
    }
  
    this.router.navigate(['/seats', showtimeId]);
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