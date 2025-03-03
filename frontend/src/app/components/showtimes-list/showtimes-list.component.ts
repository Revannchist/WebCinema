import { Component, OnInit } from '@angular/core';
import { ShowtimeService } from '../../services/showtime-service';
import { GetShowTimeDto } from '../../models/dto/showtime.dto';
import { MovieService } from '../../services/movie-service';
import { MoviePosterService } from '../../services/movie-poster-service';
import { MovieGetDto } from '../../models/dto/movie.dto';
import { MoviePosterResponseDto } from '../../models/dto/move-poster.dto';
import { GenreService } from '../../services/genre-service';

@Component({
  selector: 'app-showtimes-list',
  templateUrl: './showtimes-list.component.html',
  styleUrl: './showtimes-list.component.css'
})
export class ShowtimesListComponent implements OnInit {

  constructor(
    private showtimeService: ShowtimeService,
    private movieService: MovieService,
    private moviePosterService: MoviePosterService,
    private genreService: GenreService,
  ) { }

  showtimes: GetShowTimeDto[] = [];
  filteredShowtimes: GetShowTimeDto[] = [];
  moviePosters: { [key: number]: MoviePosterResponseDto | null } = {};
  loadingPosters: { [key: number]: boolean } = {};
  movieDetails: { [key: number]: MovieGetDto } = {};

  currentPage = 1;
  pageSize = 4; // Show fewer movies per page to fit the new card layout
  totalItems = 0;
  Math = Math;

  filterParams = {
    date: '',
    movieTitle: '',
    hallName: '',
    minPrice: null as number | null,
    maxPrice: null as number | null
  };

  uniqueMovieTitles: string[] = [];
  uniqueHallNames: string[] = [];
  uniqueMovieIds: number[] = [];

  ngOnInit(): void {
    this.loadShowtimes();
    this.loadGenres();

    //danasnji datum kao default filter
    this.filterParams.date = new Date().toISOString().split('T')[0];
  }

  loadShowtimes(): void {
    this.showtimeService.getAllShowTimes().subscribe({
      next: (showtimes) => {
        this.showtimes = showtimes;
        this.extractFilterOptions();
        this.applyFilters();
        this.loadMovieDetails();
      },
      error: (error) => console.error('Error loading showtimes:', error)
    });
  }

  loadMovieDetails(): void {
    const movieIds = [...new Set(this.showtimes.map(showtime => showtime.moviesId))];
    this.uniqueMovieIds = movieIds;

    movieIds.forEach(movieId => {
      this.loadMoviePoster(movieId);


      this.movieService.getMovieById(movieId).subscribe({
        next: (movie) => {
          this.movieDetails[movieId] = movie;
        },
        error: (error) => console.error(`Error loading details for movie ID ${movieId}:`, error)
      });

    });
  }

  loadMoviePoster(movieId: number): void {
    if (this.loadingPosters[movieId]) return;

    this.loadingPosters[movieId] = true;
    this.moviePosterService.getPosterByMovieId(movieId).subscribe({
      next: (poster) => {
        if (poster) {
          this.moviePosters[movieId] = poster;
        }
        this.loadingPosters[movieId] = false;
      },
      error: () => {
        this.moviePosters[movieId] = null;
        this.loadingPosters[movieId] = false;
      }
    });
  }

  extractFilterOptions(): void {
    this.uniqueMovieTitles = [...new Set(this.showtimes.map(s => s.movieTitle))];
    this.uniqueHallNames = [...new Set(this.showtimes.map(s => s.hallName))];
  }

  applyFilters(): void {

    if (!this.filterParams.date) {
      this.filteredShowtimes = [...this.showtimes];
    } else {
      const filterDate = this.filterParams.date;
      this.filteredShowtimes = this.showtimes.filter(showtime => {
        const showtimeDate = new Date(showtime.showDateTime).toISOString().split('T')[0];
        return showtimeDate === filterDate;
      });
    }

    this.filteredShowtimes = this.filteredShowtimes.filter(showtime => {
      const titleMatches = !this.filterParams.movieTitle ||
        showtime.movieTitle.toLowerCase().includes(this.filterParams.movieTitle.toLowerCase());

      const hallMatches = !this.filterParams.hallName ||
        showtime.hallName.toLowerCase().includes(this.filterParams.hallName.toLowerCase());

      const priceMatches =
        (!this.filterParams.minPrice || showtime.ticketPrice >= this.filterParams.minPrice) &&
        (!this.filterParams.maxPrice || showtime.ticketPrice <= this.filterParams.maxPrice);

      return titleMatches && hallMatches && priceMatches;
    });

    const uniqueMovieIds = [...new Set(this.filteredShowtimes.map(s => s.moviesId))];
    this.totalItems = uniqueMovieIds.length;
    this.currentPage = 1;
  }

  resetFilters(): void {
    this.filterParams = {
      date: new Date().toISOString().split('T')[0], //trenutni datum
      movieTitle: '',
      hallName: '',
      minPrice: null,
      maxPrice: null
    };
    this.applyFilters();
  }

  onPageChange(page: number): void {
    this.currentPage = page;
  }

  formatTime(dateTimeStr: string): string {
    const date = new Date(dateTimeStr);
    return date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
  }

  formatDate(dateTimeStr: string): string {
    const date = new Date(dateTimeStr);
    return date.toLocaleDateString('en-US', { weekday: 'short', month: 'short', day: 'numeric' });
  }

  groupShowtimesByMovie(): { [key: number]: GetShowTimeDto[] } {
    const uniqueMovieIds = [...new Set(this.filteredShowtimes.map(s => s.moviesId))];
    const paginatedMovieIds = this.getPaginatedMovieIds(uniqueMovieIds);

    const groupedShowtimes: { [key: number]: GetShowTimeDto[] } = {};

    paginatedMovieIds.forEach(movieId => {
      groupedShowtimes[movieId] = this.filteredShowtimes.filter(s => s.moviesId === movieId);
    });

    return groupedShowtimes;
  }

  getPaginatedMovieIds(uniqueMovieIds: number[]): number[] {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    return uniqueMovieIds.slice(startIndex, startIndex + this.pageSize);
  }

  groupMovieShowtimesByDate(movieId: number): { [key: string]: GetShowTimeDto[] } {
    const movieShowtimes = this.filteredShowtimes.filter(s => s.moviesId === movieId);
    const groupedByDate: { [key: string]: GetShowTimeDto[] } = {};

    movieShowtimes.forEach(showtime => {
      const dateStr = this.formatDate(showtime.showDateTime);

      if (!groupedByDate[dateStr]) {
        groupedByDate[dateStr] = [];
      }

      groupedByDate[dateStr].push(showtime);
    });

    return groupedByDate;
  }


  genres: { [id: number]: string } = {};

  private loadGenres(): void {
    this.genreService.getAllGenres().subscribe(
      (genresList) => {
        // Create a mapping of id -> name
        genresList.forEach((genre: any) => {
          this.genres[genre.id] = genre.name;
        });
        console.log('Genres loaded successfully');
      },
      (error) => {
        console.error('Error loading genres:', error);
      }
    );
  }

  getMovieGenre(movieId: number): string {
    const movie = this.movieDetails[movieId];
    if (!movie || !movie.moviesGenresIds || movie.moviesGenresIds.length === 0) {
      return '';
    }

    return movie.moviesGenresIds
      .map(genreId => this.genres[genreId] || `Genre ${genreId}`)
      .join(', ');
  }


  getMovieDuration(movieId: number): string {
    const durationMinutes = this.movieDetails[movieId]?.duration;
    if (!durationMinutes) return '';

    const hours = Math.floor(durationMinutes / 60);
    const minutes = durationMinutes % 60;

    if (hours > 0) {
      return `${hours}h ${minutes}min`;
    }
    return `${minutes}min`;
  }
}