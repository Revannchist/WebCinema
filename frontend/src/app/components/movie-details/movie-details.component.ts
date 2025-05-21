import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MovieService, Movie } from '../../services/movie.service';
import { MoviePosterService } from '../../services/movie-poster-service';
import { DirectorService } from '../../services/director-service';
import { CountryService } from '../../services/country-service';

@Component({
  selector: 'app-movie-details',
  templateUrl: './movie-details.component.html',
  styleUrls: ['./movie-details.component.css']
})
export class MovieDetailsComponent implements OnInit {
  movie: any = null;
  moviePosterUrl: string | null = null;
  directorName: string = '';
  countryName: string = '';

  constructor(
    private route: ActivatedRoute,
    private movieService: MovieService,
    private moviePosterService: MoviePosterService,
    private directorService: DirectorService,
    private countryService: CountryService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.movieService.getMovieById(+id).subscribe({
        next: (movie: any) => {
          this.movie = movie;
          this.loadPoster(movie.id);
          // Director
          if (movie.directorName) {
            this.directorName = movie.directorName;
          } else if (movie.directorId) {
            this.directorService.getDirectorById(movie.directorId).subscribe({
              next: (director: any) => {
                this.directorName = director ? `${director.firstName} ${director.lastName}` : 'Unknown';
              },
              error: () => {
                this.directorName = 'Unknown';
              }
            });
          } else {
            this.directorName = 'Unknown';
          }
          // Country
          if (movie.countryName) {
            this.countryName = movie.countryName;
          } else if (movie.countryId) {
            this.countryService.getCountryById(movie.countryId).subscribe({
              next: (country: any) => {
                this.countryName = country?.name || 'Unknown';
              },
              error: () => {
                this.countryName = 'Unknown';
              }
            });
          } else {
            this.countryName = 'Unknown';
          }
        },
        error: () => {
          this.movie = null;
        }
      });
    }
  }

  loadPoster(movieId: number) {
    this.moviePosterService.getPosterByMovieId(movieId).subscribe({
      next: (poster: any) => {
        this.moviePosterUrl = poster?.image || null;
      },
      error: () => {
        this.moviePosterUrl = null;
      }
    });
  }
}
