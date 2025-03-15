import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SeatService } from '../../services/seats-service';
import { ShowtimeService } from '../../services/showtime-service';
import { SeatsDto } from '../../models/dto/seats.dto';
import { GetShowTimeDto } from '../../models/dto/showtime.dto';
import { BookingService } from '../../services/booking-service';
import { AuthService } from '../../auth.service';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-seats',
  templateUrl: './seats.component.html',
  styleUrl: './seats.component.css'
})
export class SeatsComponent implements OnInit {
  Math = Math;
  seatsByHall: { [hallName: string]: SeatsDto[] } = {};
  seatMap: { [hallName: string]: { [row: number]: { [col: number]: SeatsDto } } } = {};
  selectedSeats: number[] = [];
  selectedGroups: { [seatId: number]: number[] } = {};
  selectedHall: string | null = null;
  maxSeats: number = 2; 
  MAX_ALLOWED_SEATS: number = 10; 
  isLoading = true;
  error: string | null = null;
  
  showtime: GetShowTimeDto | null = null;
  totalPrice: number = 0;
  
  reservedSeats: number[] = [];
  
  constructor(
    private seatService: SeatService,
    private showtimeService: ShowtimeService,
    private bookingService: BookingService,
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router
  ) { }
  
  ngOnInit(): void {
    this.route.params.subscribe((params: any) => {
      const showtimeId = +params['id'];
      if (showtimeId) {
        this.loadShowtime(showtimeId);
      } else {
        this.error = 'No showtime selected';
      }
    });
  }
  
  loadShowtime(showtimeId: number): void {
    this.isLoading = true;
    this.showtimeService.getShowTimeById(showtimeId).subscribe({
      next: (showtime: GetShowTimeDto) => {
        this.showtime = showtime;
        this.selectedHall = showtime.hallName;
        // Load seats and reserved seats for this showtime
        this.loadSeatsAndReservations(showtimeId);
      },
      error: (err: any) => {
        this.error = 'Failed to load showtime. Please try again later.';
        this.isLoading = false;
        console.error('Error loading showtime:', err);
      }
    });
  }
  
  // Load both seats and reservations simultaneously
  loadSeatsAndReservations(showtimeId: number): void {
    this.isLoading = true;
    
    // Use forkJoin to make parallel requests
    forkJoin({
      seats: this.seatService.getAllSeats(),
      bookings: this.bookingService.getAllBookings()
    }).subscribe({
      next: (result) => {
        // Process all seats
        this.groupSeatsByHall(result.seats);
        
        // Filter bookings for current showtime
        const currentShowtimeBookings = result.bookings.filter((booking:any) => 
          booking.showDateTime === this.showtime?.showDateTime && 
          booking.hallName === this.showtime?.hallName &&
          (booking.bookingStatus === 'Confirmed' || booking.bookingStatus === 'Pending')
        );
        
        // Extract reserved seat IDs
        this.reservedSeats = [];
        currentShowtimeBookings.forEach((booking:any) => {
          if (booking.bookedSeats && Array.isArray(booking.bookedSeats)) {
            this.reservedSeats.push(...booking.bookedSeats);
          }
        });
        
        // Set the selected hall
        if (this.showtime) {
          this.selectedHall = this.showtime.hallName;
        } else {
          const hallNames = this.getHallNames();
          if (hallNames.length > 0) {
            this.selectedHall = hallNames[0];
          }
        }
        
        this.isLoading = false;
        
        // Calculate initial price
        this.totalPrice = this.calculateTotalPrice();
      },
      error: (err: any) => {
        this.error = 'Failed to load seats or bookings. Please try again later.';
        this.isLoading = false;
        console.error('Error loading data:', err);
      }
    });
  }
  
  loadAllSeats(): void {
    this.isLoading = true;
    this.seatService.getAllSeats().subscribe({
      next: (seats: SeatsDto[]) => {
        this.groupSeatsByHall(seats);

        if (this.showtime) {
          this.selectedHall = this.showtime.hallName;
          // If we have a showtime, load bookings to get reserved seats
          this.loadBookingsForShowtime();
        } else {
          const hallNames = this.getHallNames();
          if (hallNames.length > 0) {
            this.selectedHall = hallNames[0];
          }
          this.isLoading = false;
        }
        
        // Calculate initial price
        this.totalPrice = this.calculateTotalPrice();
      },
      error: (err: any) => {
        this.error = 'Failed to load seats. Please try again later.';
        this.isLoading = false;
        console.error('Error loading seats:', err);
      }
    });
  }
  
  // Method to load bookings and extract reserved seats
  loadBookingsForShowtime(): void {
    this.bookingService.getAllBookings().subscribe({
      next: (bookings) => {
        // Filter bookings for current showtime
        const currentShowtimeBookings = bookings.filter((booking:any) => 
          booking.showDateTime === this.showtime?.showDateTime && 
          booking.hallName === this.showtime?.hallName &&
          (booking.bookingStatus === 'Confirmed' || booking.bookingStatus === 'Pending')
        );
        
        // Extract reserved seat IDs
        this.reservedSeats = [];
        currentShowtimeBookings.forEach((booking:any) => {
          if (booking.bookedSeats && Array.isArray(booking.bookedSeats)) {
            this.reservedSeats.push(...booking.bookedSeats);
          }
        });
        
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error loading bookings:', err);
        this.isLoading = false;
      }
    });
  }
  
  private groupSeatsByHall(seats: SeatsDto[]): void {
    this.seatsByHall = {};
    this.seatMap = {};
    
    seats.forEach(seat => {
      if (!this.seatsByHall[seat.hallName]) {
        this.seatsByHall[seat.hallName] = [];
        this.seatMap[seat.hallName] = {};
      }
      this.seatsByHall[seat.hallName].push(seat);
      
      const row = Math.floor((seat.id - 1) / 15) + 1; // Assuming 15 seats per row
      const col = (seat.id - 1) % 15 + 1;
      
      // Initialize row if it doesn't exist
      if (!this.seatMap[seat.hallName][row]) {
        this.seatMap[seat.hallName][row] = {};
      }
      
      // Store seat in map
      this.seatMap[seat.hallName][row][col] = seat;
    });
  }
  
  getHallNames(): string[] {
    return Object.keys(this.seatsByHall);
  }
  
  selectHall(hallName: string): void {
    this.selectedHall = hallName;
    this.selectedSeats = [];
    this.selectedGroups = {};
    
    // Update price when hall changes
    this.totalPrice = this.calculateTotalPrice();
  }
  
  // Calculate total price
  calculateTotalPrice(): number {
    if (!this.showtime) return 0;
    return this.selectedSeats.length * this.showtime.ticketPrice;
  }
  
  toggleSeatSelection(seat: SeatsDto): void {
    // Don't allow selection of reserved seats
    if (this.isSeatReserved(seat.id)) {
      return;
    }
    
    const index = this.selectedSeats.indexOf(seat.id);
    
    if (index === -1) {
      this.selectSeatAndAdjacent(seat);
    } else {
      this.unselectSeatAndAdjacent(seat);
    }
    
    // Update total price
    this.totalPrice = this.calculateTotalPrice();
  }
  
  selectSeatAndAdjacent(seat: SeatsDto): void {
    if (this.selectedSeats.length < this.maxSeats) {
      const remainingSeats = this.maxSeats - this.selectedSeats.length;
      const row = Math.floor((seat.id - 1) / 15) + 1;
      const col = (seat.id - 1) % 15 + 1;
      
      const groupSeats: number[] = [seat.id];
      
      // Add the clicked seat
      this.selectedSeats.push(seat.id);
      
      if (remainingSeats > 1 && this.selectedHall) {
        // First try to select seats to the right
        let seatsAdded = 0;
        for (let i = 1; i < remainingSeats; i++) {
          const adjacentCol = col + i;
          
          if (this.isSeatAvailable(row, adjacentCol)) {
            const adjacentSeatId = this.seatMap[this.selectedHall][row][adjacentCol].id;
            this.selectedSeats.push(adjacentSeatId);
            groupSeats.push(adjacentSeatId);
            seatsAdded++;
          } else {
            break; // Stop if we hit an unavailable seat
          }
        }
        
        // If we couldn't add all seats to the right, try to the left
        if (seatsAdded < (remainingSeats - 1) && col > 1) {
          for (let i = 1; i <= (remainingSeats - 1 - seatsAdded); i++) {
            const leftCol = col - i;
            
            if (this.isSeatAvailable(row, leftCol)) {
              const leftSeatId = this.seatMap[this.selectedHall][row][leftCol].id;
              this.selectedSeats.push(leftSeatId);
              groupSeats.push(leftSeatId);
            } else {
              break; // Stop if we hit an unavailable seat
            }
          }
        }
      }
      
      // Track which seats were selected as a group
      groupSeats.forEach(seatId => {
        this.selectedGroups[seatId] = groupSeats;
      });
    } else {
      alert(`You can only select up to ${this.maxSeats} seats.`);
    }
  }
  
  isSeatAvailable(row: number, col: number): boolean {
    if (!this.selectedHall) return false;
    
    // Check if seat exists in the map
    if (!this.seatMap[this.selectedHall][row] || 
        !this.seatMap[this.selectedHall][row][col]) {
      return false;
    }
    
    const seatId = this.seatMap[this.selectedHall][row][col].id;
    
    return (
      this.seatMap[this.selectedHall][row][col].seatType !== 'Unavailable' &&
      !this.selectedSeats.includes(seatId) &&
      !this.isSeatReserved(seatId) // Check if seat is reserved
    );
  }
  
  // Method to check if a seat is reserved
  isSeatReserved(seatId: number): boolean {
    return this.reservedSeats.includes(seatId);
  }
  
  unselectSeatAndAdjacent(seat: SeatsDto): void {
    // Get the group of seats that were selected together
    const groupSeats = this.selectedGroups[seat.id] || [seat.id];
    
    // Remove all seats in the group from the selection
    groupSeats.forEach(seatId => {
      const index = this.selectedSeats.indexOf(seatId);
      if (index !== -1) {
        this.selectedSeats.splice(index, 1);
      }
      // Clean up the group tracking
      delete this.selectedGroups[seatId];
    });
  }

  // Update the max seats with validation
  updateMaxSeats(increment: number): void {
    const newValue = this.maxSeats + increment;
    
    if (newValue >= 1 && newValue <= this.MAX_ALLOWED_SEATS) {
      this.maxSeats = newValue;
      
      // If reducing max seats, unselect excess seats
      if (increment < 0 && this.selectedSeats.length > this.maxSeats) {
        // Find the last group selected and remove it
        const lastSelectedId = this.selectedSeats[this.selectedSeats.length - 1];
        const lastGroup = this.selectedGroups[lastSelectedId] || [lastSelectedId];
        
        this.unselectSeatAndAdjacent(this.findSeatById(lastSelectedId));
      }
    }
  }
  
  findSeatById(seatId: number): SeatsDto {
    if (this.selectedHall) {
      return this.seatsByHall[this.selectedHall].find(seat => seat.id === seatId)!;
    }
    throw new Error('No seat found with that ID');
  }

  createBooking(): void {
    if (!this.showtime) {
      this.error = 'Cannot create booking: No showtime selected';
      return;
    }
    
    if (this.selectedSeats.length === 0) {
      this.error = 'Cannot create booking: No seats selected';
      return;
    }
  
    const userId = this.authService.getCurrentUserId();
    
    // Check if user is logged in
    if (userId === null) {
      alert('Please log in to complete your booking.');
      this.router.navigate(['/login'], { 
        queryParams: { 
          returnUrl: this.router.url 
        } 
      });
      return;
    }
  
    // Create the booking data object according to the API requirements
    const bookingData = {
      usersId: userId,
      showTimesId: this.showtime.id,
      bookedSeatsIds: this.selectedSeats,
      ticketQuantity: this.selectedSeats.length,
      totalPrice: this.totalPrice,
      bookingStatus: "Pending",
      bookingDate: new Date().toISOString()
    };
  
    this.isLoading = true;
    this.bookingService.addBooking(bookingData).subscribe({
      next: (response) => {
        this.isLoading = false;
        
        // to reflect the change in the UI
        this.reservedSeats = [...this.reservedSeats, ...this.selectedSeats];
        
        // Clear the selection
        this.selectedSeats = [];
        this.selectedGroups = {};
        this.totalPrice = 0;
        
        alert(`Booking initiated! Your booking ID is: ${response.id}`);
        
        //this.router.navigate(['/booking-confirmation', response.id]); //ovo kad djeno pravio placanje
        this.router.navigate(['/bookings', response.id]); //privremena ruta nakon sto user selektira sjedista 
      },
      error: (err) => {
        this.isLoading = false;
        console.error('Error creating booking:', err);
        
        // Check if there's an error message from the API
        if (err && err.error) {
          this.error = `Booking error: ${err.error}`;
          alert(this.error);
          
          // Refresh the seat data to get the latest availability
          if (this.showtime) {
            this.loadSeatsAndReservations(this.showtime.id);
          }
        } else {
          this.error = 'Failed to create booking. Please try again.';
        }
      }
    });
  }
  
  isSeatSelected(seatId: number): boolean {
    return this.selectedSeats.includes(seatId);
  }
  
  isSeatAccessible(seat: SeatsDto): boolean {
    return seat.seatType === 'Accessible';
  }
  
  isLoveSeat(seat: SeatsDto): boolean {
    return seat.seatType === 'Love';
  }
  
  isRegularSeat(seat: SeatsDto): boolean {
    return seat.seatType === 'Regular';
  }
  
  getSelectedSeatsCount(): number {
    return this.selectedSeats.length;
  }
  
  confirmSelection(): void {
    if (!this.showtime) {
      alert('No showtime selected');
      return;
    }
    
    if (this.selectedSeats.length === 0) {
      alert('Please select at least one seat');
      return;
    }
  
    // Show confirmation dialog
    const message = `You have selected ${this.getSelectedSeatsCount()} seats.
      Total price: $${this.totalPrice.toFixed(2)}
      Seats: ${this.selectedSeats.join(', ')}
      Movie: ${this.showtime.movieTitle}
      Showtime: ${new Date(this.showtime.showDateTime).toLocaleString()}`;
      
    // Ask for confirmation before proceeding
    if (confirm(`${message}\n\nProceed with booking?`)) {
      this.createBooking();
    }
  }
}