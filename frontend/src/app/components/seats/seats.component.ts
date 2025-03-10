import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SeatService } from '../../services/seats-service';
import { ShowtimeService } from '../../services/showtime-service';
import { SeatsDto } from '../../models/dto/seats.dto';
import { GetShowTimeDto } from '../../models/dto/showtime.dto';

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
  maxSeats: number = 4;
  isLoading = true;
  error: string | null = null;
  
  // Add new properties for showtime integration
  showtime: GetShowTimeDto | null = null;
  totalPrice: number = 0;
  
  constructor(
    private seatService: SeatService,
    private showtimeService: ShowtimeService,
    private route: ActivatedRoute
  ) { }
  
  ngOnInit(): void {
    // Get showtime ID from route params
    this.route.params.subscribe((params: any) => {
      const showtimeId = +params['id']; // Convert to number
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
        // Set the hall from showtime
        this.selectedHall = showtime.hallName;
        // Now load the seats
        this.loadAllSeats();
      },
      error: (err: any) => {
        this.error = 'Failed to load showtime. Please try again later.';
        this.isLoading = false;
        console.error('Error loading showtime:', err);
      }
    });
  }
  
  loadAllSeats(): void {
    this.isLoading = true;
    this.seatService.getAllSeats().subscribe({
      next: (seats: SeatsDto[]) => {
        this.groupSeatsByHall(seats);
        // Filter seats by hall ID if showtime has a hall
        if (this.showtime) {
          this.selectedHall = this.showtime.hallName;
        } else {
          // Fallback to first hall if no showtime
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
        this.error = 'Failed to load seats. Please try again later.';
        this.isLoading = false;
        console.error('Error loading seats:', err);
      }
    });
  }
  
  private groupSeatsByHall(seats: SeatsDto[]): void {
    this.seatsByHall = {};
    this.seatMap = {};
    
    // First, group by hall
    seats.forEach(seat => {
      if (!this.seatsByHall[seat.hallName]) {
        this.seatsByHall[seat.hallName] = [];
        this.seatMap[seat.hallName] = {};
      }
      this.seatsByHall[seat.hallName].push(seat);
      
      // Assume each seat has row and column properties or extract them from seatNumber
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
  
  // Add method to calculate total price
  calculateTotalPrice(): number {
    if (!this.showtime) return 0;
    return this.selectedSeats.length * this.showtime.ticketPrice;
  }
  
  toggleSeatSelection(seat: SeatsDto): void {
    const index = this.selectedSeats.indexOf(seat.id);
    
    if (index === -1) {
      // If not selected, let's handle the selection
      this.selectSeatAndAdjacent(seat);
    } else {
      // If already selected, unselect the seat and any adjacent ones from the same group
      this.unselectSeatAndAdjacent(seat);
    }
    
    // Update total price
    this.totalPrice = this.calculateTotalPrice();
  }
  
  selectSeatAndAdjacent(seat: SeatsDto): void {
    // If we haven't reached the maximum yet
    if (this.selectedSeats.length < this.maxSeats) {
      const remainingSeats = this.maxSeats - this.selectedSeats.length;
      const row = Math.floor((seat.id - 1) / 15) + 1;
      const col = (seat.id - 1) % 15 + 1;
      
      // Create a group for tracking seats selected together
      const groupSeats: number[] = [seat.id];
      
      // First, add the selected seat
      this.selectedSeats.push(seat.id);
      
      // Then, try to find adjacent seats to select (to the right)
      if (remainingSeats > 1 && this.selectedHall) {
        for (let i = 1; i < remainingSeats; i++) {
          const adjacentCol = col + i;
          
          // Check if adjacent seat exists and is available
          if (
            this.seatMap[this.selectedHall][row] && 
            this.seatMap[this.selectedHall][row][adjacentCol] &&
            this.seatMap[this.selectedHall][row][adjacentCol].seatType !== 'Unavailable' &&
            !this.selectedSeats.includes(this.seatMap[this.selectedHall][row][adjacentCol].id)
          ) {
            const adjacentSeatId = this.seatMap[this.selectedHall][row][adjacentCol].id;
            this.selectedSeats.push(adjacentSeatId);
            groupSeats.push(adjacentSeatId);
          } else {
            // If we can't select to the right, try to the left
            const leftCol = col - i;
            if (
              this.seatMap[this.selectedHall][row] && 
              this.seatMap[this.selectedHall][row][leftCol] &&
              this.seatMap[this.selectedHall][row][leftCol].seatType !== 'Unavailable' &&
              !this.selectedSeats.includes(this.seatMap[this.selectedHall][row][leftCol].id)
            ) {
              const leftSeatId = this.seatMap[this.selectedHall][row][leftCol].id;
              this.selectedSeats.push(leftSeatId);
              groupSeats.push(leftSeatId);
            } else {
              // If we can't select in either direction, stop
              break;
            }
          }
        }
      }
      
      // Store the group selection information
      groupSeats.forEach(seatId => {
        this.selectedGroups[seatId] = groupSeats;
      });
    } else {
      alert(`You can only select up to ${this.maxSeats} seats.`);
    }
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
    
    const message = `You have selected ${this.getSelectedSeatsCount()} seats.
      Total price: $${this.totalPrice.toFixed(2)}
      Seats: ${this.selectedSeats.join(', ')}
      Movie: ${this.showtime.movieTitle}
      Showtime: ${new Date(this.showtime.showDateTime).toLocaleString()}`;
      
    alert(message);
    //checkout or reservation
  }
}
