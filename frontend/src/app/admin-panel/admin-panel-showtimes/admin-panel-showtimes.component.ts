import { Component, OnInit } from '@angular/core';
import { ShowtimeService } from '../../services/showtime-service';
import { MovieService } from '../../services/movie-service';
import { MovieGetDto } from '../../models/dto/movie.dto';
import { HallService } from '../../services/hall-service';
import { HallDisplayDto } from '../../models/dto/showtime.dto';
import { AddShowTimeDto, GetShowTimeDto, UpdateShowTimeDto } from '../../models/dto/showtime.dto';
import { DatePipe } from '@angular/common';
import { Modal } from 'bootstrap'; //npm install bootstrap


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

  editModal: any;

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

  showtimeToDelete: any | null = null;

  deleteShowtime(showtime: any): void {
    this.showtimeToDelete = showtime;
    const modal = new Modal(document.getElementById('deleteShowtimeModal')!);
    modal.show();
  }

  confirmDelete(): void {
    if (this.showtimeToDelete) {
      this.showtimeService.deleteShowTime(this.showtimeToDelete.id).subscribe({
        next: (response) => {
          console.log('Showtime delete response:', response);
          this.loadShowtimes();
          this.showtimeToDelete = null;
        },
        error: (error) => console.error('Error deleting showtime:', error)
      });
    }
  }

  openEditModal(showtime: GetShowTimeDto): void {

    this.showtimeToEdit = {
      id: showtime.id,
      moviesId: showtime.moviesId,
      hallsId: showtime.hallsId,
      showDateTime: showtime.showDateTime,
      ticketPrice: showtime.ticketPrice,
      isActive: showtime.isActive
    };

    const dateObj = new Date(showtime.showDateTime);

    this.editShowtimeDate = this.datePipe.transform(dateObj, 'yyyy-MM-dd') || '';

    this.editShowtimeTime = this.datePipe.transform(dateObj, 'HH:mm') || '';

    this.editModal = new bootstrap.Modal(document.getElementById('editShowtime'));
    this.editModal.show();
  }

  updateShowtime(): void {
    const id = this.showtimeToEdit.id;
    const updateDto: UpdateShowTimeDto = {
      moviesId: this.showtimeToEdit.moviesId,
      hallsId: this.showtimeToEdit.hallsId,
      showDateTime: `${this.editShowtimeDate}T${this.editShowtimeTime}`,
      ticketPrice: this.showtimeToEdit.ticketPrice,
      isActive: this.showtimeToEdit.isActive
    };

    this.showtimeService.updateShowTime(id, updateDto).subscribe({
      next: () => {
        console.log('Showtime updated successfully');
        this.loadShowtimes();
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

  pageSize = 10;
  currentPage = 1;
  totalPages = 1;

  get paginatedShowtimes() {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    return this.showtimes.slice(startIndex, endIndex);
  }

  getPagesArray(): number[] {
    return Array.from({ length: this.totalPages }, (_, i) => i + 1);
  }

  changePage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
    }
  }

  calculateTotalPages(): void {
    this.totalPages = Math.ceil(this.showtimes.length / this.pageSize);

    if (this.currentPage > this.totalPages) {
      this.currentPage = 1;
    }
  }

}