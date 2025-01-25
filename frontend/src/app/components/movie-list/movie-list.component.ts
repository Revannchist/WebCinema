import { Component, OnInit, HostListener } from '@angular/core';
import { MovieService } from '../../services/movie-service';
import { MoviePosterService } from '../../services/movie-poster-service';
import { MovieGetDto, MovieParameters, MoviePagedResponse } from '../../models/dto/movie.dto';
import { MoviePosterResponseDto } from '../../models/dto/move-poster.dto';
import { HttpErrorResponse } from '@angular/common/http';
import { GenreService } from '../../services/genre-service';
import { GenreDto } from '../../models/dto/genre.dto';


@Component({
  selector: 'app-movie-list',
  templateUrl: './movie-list.component.html',
  styleUrls: ['./movie-list.component.css']
})
export class MovieListComponent implements OnInit {

  constructor(
    private movieService: MovieService,
    private moviePosterService: MoviePosterService,
    private genreService: GenreService
  ) { }

  movies: MovieGetDto[] = [];
  moviePosters: { [key: number]: MoviePosterResponseDto | null } = {};
  loadingPosters: { [key: number]: boolean } = {};

  currentPage = 1;
  pageSize = 10;
  totalItems = 0;
  Math = Math;

  genres: GenreDto[] = [];
  isGenreDropdownOpen = false;


  filterParams: MovieParameters = {
    pageNumber: this.currentPage,
    pageSize: this.pageSize,
    searchTerm: '',
    directorId: undefined,
    genreIds: [],
    actorIds: [],
    fromDate: undefined,
    toDate: undefined,
    language: undefined,
    ageRating: undefined,
    countryId: undefined
  };

  ngOnInit(): void {
    this.loadMovies();
    this.loadGenres();
  }

  loadGenres(): void {
    this.genreService.getAllGenres().subscribe(genres => this.genres = genres);
  }

  getGenreName(genreId: number): string {
    const genre = this.genres.find((g: any) => g.id === genreId);
    return genre ? genre.name : 'N/A';
  }


  toggleGenre(genreIds: number[], genreId: number, event: Event): void {
    event.stopPropagation();
    const index = genreIds.indexOf(genreId);
    if (index > -1) {
      genreIds.splice(index, 1);
    } else {
      genreIds.push(genreId);
    }
  }

  removeGenre(genreIds: number[], genreId: number): void {
    const index = genreIds.indexOf(genreId);
    if (index > -1) {
      genreIds.splice(index, 1);
    }
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

  loadMovies(): void {
    this.movieService.getMovies(this.filterParams).subscribe({
      next: (response: MoviePagedResponse<MovieGetDto>) => {
        this.movies = response.items;
        this.totalItems = response.totalCount;
        // Stagger poster loading zato sto lazy loading ima problema sa base64 formatom
        this.movies.forEach((movie, index) => {
          setTimeout(() => {
            this.loadMoviePoster(movie.id);
          }, index * 100);
        });
      },
      error: (error) => console.error('Error loading movies:', error)
    });
  }

  onPageChange(page: number): void {
    this.filterParams.pageNumber = page;
    this.loadMovies();
  }

  filterMovies(): void {
    this.filterParams.pageNumber = 1;
    this.loadMovies();
  }

  resetFilters(): void {
    this.filterParams = {
      pageNumber: 1,
      pageSize: 10,
      searchTerm: '',
      fromDate: undefined,
      toDate: undefined,
      language: undefined,
      ageRating: undefined,
      directorId: undefined,
      countryId: undefined,
      genreIds: [],
      actorIds: []
    };
    this.loadMovies();
  }

  @HostListener('document:click', ['$event'])
  clickOutside(event: MouseEvent): void {
    const clickedInside = event.target instanceof HTMLElement &&
      document.querySelector('.dropdown-container')?.contains(event.target);

    if (!clickedInside) {
      this.isGenreDropdownOpen = false;
    }
  }
}