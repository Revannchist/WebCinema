import { Component, OnInit } from '@angular/core';
import { MovieService, Movie } from '../../services/movie.service';
import { ShowtimeService, Showtime } from '../../services/showtime.service';
import { MoviePosterService } from '../../services/movie-poster-service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  featuredMovies: Movie[] = [];
  upcomingShowtimes: Showtime[] = [];

  heroMovie: Movie | null = null;
  heroMoviePosterUrl: string | null = null;
  showTrailerModal = false;

  allMovies: Movie[] = [];
  allMoviePosters: { [id: number]: string } = {};

  constructor(
    private movieService: MovieService,
    private showtimeService: ShowtimeService,
    private moviePosterService: MoviePosterService
  ) {}

  ngOnInit(): void {
    this.loadHeroMovie();
    this.loadFeaturedMovies();
    this.loadUpcomingShowtimes();
    this.loadAllMovies();
  }

  loadHeroMovie(): void {
    this.movieService.getAllMovies().subscribe({
      next: (movies: any) => {
        const found = (movies.items || []).find((m: Movie) => m.title.trim().toUpperCase() === 'MINECRAFT FILM') || null;
        this.heroMovie = found;
        if (found) {
          this.moviePosterService.getPosterByMovieId(found.id).subscribe({
            next: (poster: any) => {
              this.heroMoviePosterUrl = poster?.image || null;
            },
            error: () => {
              this.heroMoviePosterUrl = null;
            }
          });
        }
      },
      error: (error: Error) => {
        console.error('Greška pri učitavanju filmova:', error);
      }
    });
  }

  private loadFeaturedMovies(): void {
    this.movieService.getFeaturedMovies?.().subscribe?.({
      next: (movies: any) => {
        this.featuredMovies = movies.items || movies;
      },
      error: (error: Error) => {
        console.error('Greška pri učitavanju filmova:', error);
      }
    });
  }

  private loadUpcomingShowtimes(): void {
    this.showtimeService.getUpcomingShowtimes().subscribe({
      next: (showtimes: Showtime[]) => {
        this.upcomingShowtimes = showtimes;
      },
      error: (error: Error) => {
        console.error('Greška pri učitavanju projekcija:', error);
      }
    });
  }

  loadAllMovies(): void {
    this.movieService.getAllMovies().subscribe({
      next: (movies: any) => {
        this.allMovies = movies.items || [];
        this.allMovies.forEach(movie => {
          this.moviePosterService.getPosterByMovieId(movie.id).subscribe({
            next: (poster: any) => {
              this.allMoviePosters[movie.id] = poster?.image || '';
            },
            error: () => {
              this.allMoviePosters[movie.id] = '';
            }
          });
        });
      },
      error: (error: Error) => {
        console.error('Greška pri učitavanju svih filmova:', error);
      }
    });
  }

  openTrailerModal() {
    this.showTrailerModal = true;
  }

  closeTrailerModal() {
    this.showTrailerModal = false;
  }
}
