import { Component, HostListener, ElementRef, ViewChild } from '@angular/core';
import { Modal } from 'bootstrap'; //npm install bootstrap
import { MovieService } from '../../services/movie-service';
import { GenreService } from '../../services/genre-service';
import { ActorService } from '../../services/actor-service';
import { DirectorService } from '../../services/director-service';
import { CountryService } from '../../services/country-service';
import { MovieCreateDto, MovieUpdateDto, MovieGetDto, MovieParameters, MoviePagedResponse } from '../../models/dto/movie.dto';
import { MoviePosterService } from '../../services/movie-poster-service';
import { CreateMoviePosterDto } from '../../models/dto/move-poster.dto';
import { DirectorDto } from '../../models/dto/director.dto';
import { CountryDto } from '../../models/dto/country.dto';
import { trigger, transition, style, animate } from '@angular/animations';


@Component({
  selector: 'app-admin-panel-movies',
  templateUrl: './admin-panel-movies.component.html',
  styleUrl: './admin-panel-movies.component.css',

  animations: [
    trigger('fadeIn', [
      transition(':enter', [
        style({ opacity: 0 }),
        animate('300ms', style({ opacity: 1 }))
      ])
    ])
  ]
  
})

export class AdminPanelMoviesComponent {

  @ViewChild('fileInput') fileInput!: ElementRef;

  constructor(
    private movieService: MovieService,
    private genreService: GenreService,
    private actorService: ActorService,
    private directorService: DirectorService,
    private countryService: CountryService,
    private moviePosterService: MoviePosterService
  
  ) { }

  movies: MovieGetDto[] = [];
  filteredMovies: MovieGetDto[] = [];

  public movieToAdd: MovieCreateDto = {
    title: '',
    description: '',
    releaseDate: '',
    duration: 0,
    language: '',
    ageRating: '',
    directorId: 0,
    countryId: 0,
    genreIds: [],
    actorIds: []
  };

  public selectedImage: string | null = null;
  public imagePreview: string | null = null;

  private movieToEditId: number | null = null;
  public movieToEdit: MovieUpdateDto = {
    title: '',
    description: '',
    releaseDate: '',
    duration: 0,
    language: '',
    ageRating: '',
    directorId: 0,
    countryId: 0,
    genreIds: [],
    actorIds: []
  };

  protected readonly Math = Math;

  currentPage: number = 1;
  pageSize: number = 10;
  totalPages: number = 0;
  totalItems: number = 0;

  filterParams: MovieParameters = {
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

  private validateMovie(movie: MovieCreateDto): boolean {
    return !!(movie.title &&
      movie.description &&
      movie.releaseDate &&
      movie.duration >= 1 &&
      movie.duration <= 1000 &&
      movie.language &&
      movie.ageRating &&
      movie.directorId &&
      movie.countryId &&
      movie.genreIds.length > 0 &&
      movie.actorIds.length > 0);
  }

  public search: string = '';
  private debounceTimer: any;
  public director: DirectorDto[] = [];
  public country: CountryDto[] = [];
  public genres: { id: number; name: string; }[] = [];
  public actors: { id: number; firstName: string; lastName: string; }[] = [];

  movieToDelete: MovieGetDto | null = null;

  isGenreDropdownOpen = false;
  isActorDropdownOpen = false;

  isModalGenreDropdownOpen = false;
  isModalActorDropdownOpen = false;

  ngOnInit(): void {
    this.loadMovies();
    this.loadDirectors();
    this.loadCountries();
    this.loadGenres();
    this.loadActors();
  }

  loadMovies(): void {
    this.movieService.getMovies(this.filterParams).subscribe({
      next: (response: MoviePagedResponse<MovieGetDto>) => {
        this.movies = response.items;
        this.filteredMovies = response.items;
        this.currentPage = response.pageNumber;
        this.pageSize = response.pageSize;
        this.totalPages = response.totalPages;
        this.totalItems = response.totalCount;
      },
      error: (error) => console.error('Error loading movies:', error)
    });
  }

  loadGenres(): void {
    this.genreService.getAllGenres().subscribe(genres => this.genres = genres);
  }

  getGenreName(genreId: number): string {
    const genre = this.genres.find((g: any) => g.id === genreId);
    return genre ? genre.name : 'N/A';
  }

  loadActors(): void {
    this.actorService.getAllActors().subscribe(actors => this.actors = actors);
  }

  getActorName(actorId: number): string {
    const actor = this.actors.find((a: any) => a.id === actorId);
    return actor ? `${actor.firstName} ${actor.lastName}` : 'N/A';
  }

  loadDirectors(): void {
    this.directorService.getAllDirectors().subscribe({
      next: (response: any) => {
        this.director = response;
      },
      error: (error) => {
        console.error('Error loading directors:', error);
      }
    });
  }

  loadCountries(): void {
    this.countryService.getAllCountries().subscribe({
      next: (response: any) => {
        this.country = response;
      },
      error: (error) => console.error('Error:', error)
    });
  }

  getCountryName(countryData: CountryDto | number | undefined): string {
    if (!countryData) return 'N/A';
    const countryId = typeof countryData === 'object' ? countryData.id : countryData;
    return this.country.find(c => c.id === countryId)?.name || 'N/A';
  }

  getDirectorName(directorData: DirectorDto | number | undefined): string {
    if (!directorData) return 'N/A';
    const directorId = typeof directorData === 'object' ? directorData.id : directorData;
    const director = this.director.find(d => d.id === directorId);
    return director ? `${director.firstName} ${director.lastName}` : 'N/A';
  }

  deleteMovie(movie: MovieGetDto): void {
    this.movieToDelete = movie;
    const modal = new Modal(document.getElementById('deleteConfirmModal')!);
    modal.show();
  }

  confirmDelete(): void {
    if (this.movieToDelete) {
      this.movieService.deleteMovie(this.movieToDelete.id).subscribe({
        next: (response) => {
          console.log('Movie delete response:', response);
          this.ngOnInit();
          this.movieToDelete = null;
        },
        error: (error) => console.error('Error:', error)
      });
    }
  }

  clearModalTextBox(): void {
    this.movieToAdd = {
      title: '',
      description: '',
      releaseDate: '',
      duration: 0,
      language: '',
      ageRating: '',
      directorId: 0,
      countryId: 0,
      genreIds: [],
      actorIds: []
    };
    this.isModalGenreDropdownOpen = false;
    this.isModalActorDropdownOpen = false;
  }

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {

      if (!file.type.match(/image\/(jpeg|png)/)) {
        alert('Only JPEG and PNG images are allowed');
        this.fileInput.nativeElement.value = '';
        this.selectedImage = null;
        this.imagePreview = null;
        return;
      }

      if (file.size > 5 * 1024 * 1024) {
        alert('File size should not exceed 5MB');
        this.fileInput.nativeElement.value = '';
        this.selectedImage = null;
        this.imagePreview = null;
        return;
      }

      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.selectedImage = e.target.result;
        this.imagePreview = e.target.result;
      };
      reader.readAsDataURL(file);
    }
  }

  addMovie(movieData: any): void {
    if (!this.validateMovie(movieData)) {
      return;
    }

    const movieToCreate: MovieCreateDto = {
      title: movieData.title,
      description: movieData.description,
      releaseDate: movieData.releaseDate,
      duration: movieData.duration,
      language: movieData.language,
      ageRating: movieData.ageRating,
      directorId: movieData.directorId,
      countryId: movieData.countryId,
      genreIds: movieData.genreIds,
      actorIds: movieData.actorIds
    };

    this.movieService.createMovie(movieToCreate).subscribe({
      next: (response: any) => {
        console.log('Movie created successfully:', response);

        if (this.selectedImage) {
          const posterDto: CreateMoviePosterDto = {
            id: 0,
            movieId: response.id,
            image: this.selectedImage
          };

          this.moviePosterService.addMoviePoster(posterDto).subscribe({
            next: (posterResponse) => {
              console.log('Poster added successfully:', posterResponse);
              this.clearModalTextBox();
              this.loadMovies();
              this.loadGenres();
              this.loadActors();
            },
            error: (error) => {
              console.error('Error adding poster:', error);
              this.clearModalTextBox();
              this.loadMovies();
              this.loadGenres();
              this.loadActors();
            }
          });
        } else {
          this.clearModalTextBox();
          this.loadMovies();
          this.loadGenres();
          this.loadActors();
        }
      },
      error: (error) => console.error('Error adding movie:', error)
    });
  }


  prepareEditMovie(movie: MovieGetDto): void {
    console.log('Movie being prepared for edit:', movie);
    this.movieToEditId = movie.id;
    console.log('Set movieToEditId to:', this.movieToEditId);
    this.movieToEdit = {
      title: movie.title,
      description: movie.description,
      releaseDate: movie.releaseDate.split('T')[0],
      duration: movie.duration,
      language: movie.language,
      ageRating: movie.ageRating,
      directorId: typeof movie.directorId === 'object' ? movie.directorId.id : movie.directorId,
      countryId: typeof movie.countryId === 'object' ? movie.countryId.id : movie.countryId,
      genreIds: movie.moviesGenresIds,
      actorIds: movie.moviesActorsIds
    };

    this.selectedImage = null;
    this.imagePreview = null;

    if (this.movieToEditId) {
      this.moviePosterService.getPosterByMovieId(this.movieToEditId).subscribe({
        next: (poster) => {
          if (poster && poster.image) {
            this.imagePreview = poster.image;
          }
        },
        error: (error) => {
          console.log('No existing poster or error fetching poster:', error);
        }
      });
    }

    this.isModalGenreDropdownOpen = false;
    this.isModalActorDropdownOpen = false;
  }

  updateMovie(movieData: any): void {
    if (!this.validateMovie(movieData)) {
      return;
    }

    if (!this.movieToEditId) {
      console.error('No movie ID for update');
      return;
    }

    this.movieService.updateMovie(this.movieToEditId, movieData).subscribe({
      next: () => {
        this.moviePosterService.updateMoviePoster(this.movieToEditId!, this.selectedImage)
          .subscribe({
            next: () => this.ngOnInit(),
            error: error => {
              console.error('Error with poster:', error);
              this.ngOnInit();
            }
          });
      },
      error: error => console.error('Error updating movie:', error)
    });
  }

  clearPosterImage(): void {
    this.imagePreview = null;
    this.selectedImage = "DELETE_POSTER";
    if (this.fileInput && this.fileInput.nativeElement) {
      this.fileInput.nativeElement.value = '';
    }
  }

  onPageChange(page: number): void {
    this.filterParams.pageNumber = page;
    this.loadMovies();
  }

  filterMovies(): void {
    clearTimeout(this.debounceTimer);
    this.debounceTimer = setTimeout(() => {
      if (this.search !== undefined) {
        this.filterParams.searchTerm = this.search;
      }
      this.filterParams.pageNumber = 1;
      this.loadMovies();
    }, 300);
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
    this.search = '';
    this.loadMovies();
  }

  toggleGenre(array: any[], genreId: any, event?: any) {
    if (event) {
      event.stopPropagation();
    }
    const index = array.indexOf(genreId);
    if (index === -1) {
      array.push(genreId);
    } else {
      array.splice(index, 1);
    }
  }

  removeGenre(array: any[], genreId: any) {
    const index = array.indexOf(genreId);
    if (index !== -1) {
      array.splice(index, 1);
    }
  }

  toggleActor(array: any[], actorId: any, event?: any) {
    if (event) {
      event.stopPropagation();
    }
    const index = array.indexOf(actorId);
    if (index === -1) {
      array.push(actorId);
    } else {
      array.splice(index, 1);
    }
    this.filterMovies();
  }

  removeActor(array: any[], actorId: any) {
    const index = array.indexOf(actorId);
    if (index !== -1) {
      array.splice(index, 1);
    }
    this.filterMovies();
  }

  toggleGenreDropdown(event: Event): void {
    event.stopPropagation();
    this.isGenreDropdownOpen = !this.isGenreDropdownOpen;
    
    // Close actor dropdown when opening genre dropdown
    if (this.isGenreDropdownOpen) {
      this.isActorDropdownOpen = false;
    }
  }
  
  toggleActorDropdown(event: Event): void {
    event.stopPropagation();
    this.isActorDropdownOpen = !this.isActorDropdownOpen;
    
    // Close genre dropdown when opening actor dropdown
    if (this.isActorDropdownOpen) {
      this.isGenreDropdownOpen = false;
    }
  }

  //mozda ovo kasnije uradim
  getGenresString(movie: MovieGetDto): string {
    return movie?.moviesGenresIds?.map(genreId => this.getGenreName(genreId)).join(', ') || 'N/A';
  }
  
  getActorsString(movie: MovieGetDto): string {
    return movie?.moviesActorsIds?.map(actorId => this.getActorName(actorId)).join(', ') || 'N/A';
  }
  

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: any) {
    if (!event.target.closest('.dropdown-container')) {
      this.isGenreDropdownOpen = false;
      this.isActorDropdownOpen = false;
      this.isModalGenreDropdownOpen = false;
      this.isModalActorDropdownOpen = false;
    }
  }



}
