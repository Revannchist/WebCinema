import { Component, OnInit } from '@angular/core';
import { ShowtimeService } from '../../services/showtime-service';
import { MovieService } from '../../services/movie-service';
import { MovieGetDto } from '../../models/dto/movie.dto';
import { HallService } from '../../services/hall-service';
import { HallDisplayDto } from '../../models/dto/showtime.dto';
import { AddShowTimeDto, GetShowTimeDto } from '../../models/dto/showtime.dto';
import { DatePipe } from '@angular/common';

declare var bootstrap: any; // For Bootstrap modal access

@Component({
  selector: 'app-admin-panel-showtimes',
  templateUrl: './admin-panel-showtimes.component.html',
  styleUrl: './admin-panel-showtimes.component.css',
  providers: [DatePipe]
})
export class AdminPanelShowtimesComponent implements OnInit {

  showtimes: GetShowTimeDto[] = [];
  movies: MovieGetDto[] = [];
  halls: HallDisplayDto[] = [];
 
  showtimeToAdd: AddShowTimeDto = {
    id: 0,
    moviesId: 0,
    hallsId: 0,
    showDateTime: '',
    ticketPrice: 0,
    isActive: true
  };
  
  showtimeToEdit: AddShowTimeDto = {
    id: 0,
    moviesId: 0,
    hallsId: 0,
    showDateTime: '',
    ticketPrice: 0,
    isActive: true
  };
 
  showtimeDate: string = '';
  showtimeTime: string = '';
  editShowtimeDate: string = '';
  editShowtimeTime: string = '';
  
  editModal: any; // Reference to the edit modal
 
  constructor(
    private showtimeService: ShowtimeService,
    private movieService: MovieService,
    private hallService: HallService,
    private datePipe: DatePipe
  ) { }

  ngOnInit(): void {
    this.loadShowtimes();
    this.loadMovies();
    this.loadHalls();
  }

  loadShowtimes(): void {
    this.showtimeService.getAllShowTimes().subscribe({
      next: (data) => {
        this.showtimes = data;
        this.calculateTotalPages();
      },
      error: (error) => {
        console.error('Error fetching showtimes:', error);
      }
    });
  }

  loadMovies(): void {
    this.movieService.getAllMoviesSimple().subscribe({
      next: (data) => {
        this.movies = data;
      },
      error: (error) => {
        console.error('Error fetching movies:', error);
      }
    });
  }

  loadHalls(): void {
    this.hallService.getAllHalls().subscribe({
      next: (data) => {
        this.halls = data;
      },
      error: (error) => {
        console.error('Error fetching halls:', error);
      }
    });
  }

  addShowtime(): void {
    this.showtimeToAdd.showDateTime = `${this.showtimeDate}T${this.showtimeTime}`;
     
    this.showtimeService.addShowTime(this.showtimeToAdd).subscribe({
      next: (result) => {
        console.log('Showtime added successfully');
        this.loadShowtimes();
        this.clearShowtimeModalTextBox();
      },
      error: (error) => {
        console.error('Error adding showtime:', error);
      }
    });
  }

  deleteShowtime(id: number): void {
    if (confirm('Are you sure you want to delete this showtime?')) {
      this.showtimeService.deleteShowTime(id).subscribe({
        next: () => {
          console.log('Showtime deleted successfully');
          this.loadShowtimes(); 
        },
        error: (error) => {
          console.error('Error deleting showtime:', error);
        }
      });
    }
  }

  openEditModal(showtime: GetShowTimeDto): void {
    // Clone the showtime to avoid directly modifying the list item
    this.showtimeToEdit = {
      id: showtime.id,
      moviesId: showtime.moviesId,
      hallsId: showtime.hallsId,
      showDateTime: showtime.showDateTime,
      ticketPrice: showtime.ticketPrice,
      isActive: showtime.isActive
    };
    
    // Parse the date and time from showDateTime
    const dateObj = new Date(showtime.showDateTime);
    
    // Format date as YYYY-MM-DD for input[type="date"]
    this.editShowtimeDate = this.datePipe.transform(dateObj, 'yyyy-MM-dd') || '';
    
    // Format time as HH:MM for input[type="time"]
    this.editShowtimeTime = this.datePipe.transform(dateObj, 'HH:mm') || '';
    
    // Open the modal
    this.editModal = new bootstrap.Modal(document.getElementById('editShowtime'));
    this.editModal.show();
  }

  updateShowtime(): void {
    // Combine date and time into ISO string
    this.showtimeToEdit.showDateTime = `${this.editShowtimeDate}T${this.editShowtimeTime}`;
    
    this.showtimeService.updateShowTime(this.showtimeToEdit).subscribe({
      next: () => {
        console.log('Showtime updated successfully');
        this.loadShowtimes();
        // Close the modal
        if (this.editModal) {
          this.editModal.hide();
        }
      },
      error: (error) => {
        console.error('Error updating showtime:', error);
      }
    });
  }

  clearShowtimeModalTextBox(): void {
    this.showtimeToAdd = {
      id: 0,
      moviesId: 0,
      hallsId: 0,
      showDateTime: '',
      ticketPrice: 0,
      isActive: true
    };
    this.showtimeDate = '';
    this.showtimeTime = '';
  }

  formatDateTime(dateTimeString: string): string {
    const date = new Date(dateTimeString);
    return date.toLocaleString();
  }

  getMovieTitle(movieId: number): string {
    const movie = this.movies.find(m => m.id === movieId);
    return movie ? movie.title : 'Unknown';
  }

  getHallName(hallId: number): string {
    const hall = this.halls.find(h => h.id === hallId);
    return hall ? hall.hallName : 'Unknown';
  }

  formatDate(dateTime: string): string {
    if (!dateTime) return '';
    const date = new Date(dateTime);
    return date.toLocaleDateString('en-GB', { 
      day: '2-digit', 
      month: '2-digit', 
      year: 'numeric' 
    });
  }
  
  formatTime(dateTime: string): string {
    if (!dateTime) return '';
    const date = new Date(dateTime);
    return date.toLocaleTimeString('en-GB', { 
      hour: '2-digit', 
      minute: '2-digit',
      hour12: false
    });
  }

  // Pagination properties
pageSize = 10; // Number of items per page
currentPage = 1; // Current active page
totalPages = 1; // Total number of pages

// Computed property to get paginated data
get paginatedShowtimes() {
  const startIndex = (this.currentPage - 1) * this.pageSize;
  const endIndex = startIndex + this.pageSize;
  return this.showtimes.slice(startIndex, endIndex);
}

// Method to generate array of page numbers
getPagesArray(): number[] {
  return Array.from({ length: this.totalPages }, (_, i) => i + 1);
}

// Method to handle page change
changePage(page: number): void {
  if (page >= 1 && page <= this.totalPages) {
    this.currentPage = page;
  }
}

// Method to calculate total pages when data changes
calculateTotalPages(): void {
  this.totalPages = Math.ceil(this.showtimes.length / this.pageSize);
  
  // Reset to page 1 if current page is beyond total pages
  if (this.currentPage > this.totalPages) {
    this.currentPage = 1;
  }
}
}