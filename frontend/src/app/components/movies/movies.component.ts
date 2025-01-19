import { Component, HostListener } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MyConfig } from '../../my-config';
import { Modal } from 'bootstrap'; //npm install bootstrap
import { Observable, take } from 'rxjs';

interface PagedResponse<T> {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
}

@Component({
  selector: 'app-movies',
  templateUrl: './movies.component.html',
  styleUrl: './movies.component.css'
})

export class MoviesComponent {
  constructor(private http:HttpClient){}

  public movies:any
  movieToAdd: any = {
    title: '',
    description: '',
    releaseDate: '',
    duration: null,
    language: '',
    ageRating: '',
    directorId: null,
    countryId: null,
    moviesGenresIds: [],
    moviesActorsIds: []
  };

  public movieToEdit: any = {
    id: null,
    title: '',
    description: '',
    releaseDate: '',
    duration: null,
    language: '',
    ageRating: '',
    directorId: null,
    countryId: null,
    moviesGenresIds: [],
    moviesActorsIds: []  
  };

  protected readonly Math = Math;

  currentPage: number = 1;
  pageSize: number = 10;
  totalPages: number = 0;
  totalItems: number = 0;

  filterParams = {
    pageNumber: 1,
    pageSize: 10,
    searchTerm: '',
    fromDate: null as Date | null,
    toDate: null as Date | null,
    language: '',
    ageRating: '',
    directorId: null as number | null,
    countryId: null as number | null,
    genreIds: [] as number[],
    actorIds: [] as number[] 
  };

  public search:any
  public filteredMovies:any
  private debounceTimer:any;
  public director: { id: number; firstName: string, lastName: string }[] = [];
  public country: { id: number; name: string }[] = [];
  public genres: any[] = [];
  public actors: any[] = [];

  isGenreDropdownOpen = false;
  isActorDropdownOpen = false;

  isModalGenreDropdownOpen = false;
  isModalActorDropdownOpen = false;
  
  private formatDate(date: string | null): string | null {
    if (!date) return null;
    const d = new Date(date);
    if (isNaN(d.getTime())) return null;
    return d.toISOString();
  }

  ngOnInit(): void {
    this.loadMovies();
    this.loadDirectors();
    this.loadCountries();
    this.loadGenres();
    this.loadActors();
  }

  loadMovies(): void {
    // Start with basic pagination params
    let params = new HttpParams()
      .set('pageNumber', this.filterParams.pageNumber.toString())
      .set('pageSize', this.filterParams.pageSize.toString());
  
    // Add search term
    if (this.filterParams.searchTerm && this.filterParams.searchTerm.trim() !== '') {
      params = params.set('searchTerm', this.filterParams.searchTerm.trim());
    }
  
    // Add director filter
    if (this.filterParams.directorId !== null) {
      params = params.set('directorId', this.filterParams.directorId.toString());
    }
  
    // Add genre filters
    if (this.filterParams.genreIds && this.filterParams.genreIds.length > 0) {
      this.filterParams.genreIds.forEach(genreId => {
        params = params.append('genreIds', genreId.toString());
      });
    }
  
    // Add actor filters
    if (this.filterParams.actorIds && this.filterParams.actorIds.length > 0) {
      this.filterParams.actorIds.forEach(actorId => {
        params = params.append('actorsIds', actorId.toString());
      });
    }
  
    if (this.filterParams.fromDate) {
      const formattedFromDate = this.formatDate(this.filterParams.fromDate.toString());
      if (formattedFromDate) {
        params = params.set('fromDate', formattedFromDate);
      }
    }
    if (this.filterParams.toDate) {
      const formattedToDate = this.formatDate(this.filterParams.toDate.toString());
      if (formattedToDate) {
        params = params.set('toDate', formattedToDate);
      }
    }

    if (this.filterParams.language) {
      params = params.set('language', this.filterParams.language);
    }
    if (this.filterParams.ageRating) {
      params = params.set('ageRating', this.filterParams.ageRating);
    }
    if (this.filterParams.countryId !== null) {
      params = params.set('countryId', this.filterParams.countryId.toString());
    }
  
    this.http.get<PagedResponse<any>>(MyConfig.APIurl + '/api/Movies/GetAllMovies', { params })
      .subscribe({
        next: (response) => {
          this.movies = response.items;
          this.filteredMovies = response.items;
          this.currentPage = response.pageNumber;
          this.pageSize = response.pageSize;
          this.totalPages = response.totalPages;
          this.totalItems = response.totalCount;
        },
        error: (error) => {
          console.error('Error loading movies:', error);
        }
      });
  }

  loadGenres(): void {
    this.http.get(MyConfig.APIurl + '/api/Genres/GetAllGenres').subscribe((response : any) => 
      this.genres = response);
  }

  getGenreName(genreId: number): string {
    const genre = this.genres.find((g: any) => g.id === genreId);  
    return genre ? genre.name : 'N/A'; 
  }

  loadActors(): void {
    this.http.get(MyConfig.APIurl + '/api/Actors/GetAllActors').subscribe((response : any) => 
      this.actors = response);
  }

  getActorName(actorId: number): string {
    const actor = this.actors.find((a: any) => a.id === actorId);  
    return actor ? `${actor.firstName} ${actor.lastName}` : 'N/A'; 
  }

  loadDirectors(): void {
    this.http.get(MyConfig.APIurl + '/api/Directors/GetAllDirectors').subscribe({
        next: (response: any) => {
            this.director = response;
        },
        error: (error) => {
            console.error('Error loading directors:', error);
        }
    });
  }

  loadCountries(): void{
    this.http.get(MyConfig.APIurl + '/api/Countries/GetAllCountries').subscribe((response: any) => {
      this.country = response;
    })
  }

  getCountryName(countryData: any) {
    if (!countryData) return 'N/A';
    // If countryData is already an object with id property
    const countryId = typeof countryData === 'object' ? countryData.id : countryData;
    return this.country.find(c => c.id === countryId)?.name || 'N/A';
}

  getDirectorName(directorData: any) {
    if (!directorData) return 'N/A';
    // If directorData is already an object with id property
    const directorId = typeof directorData === 'object' ? directorData.id : directorData;
    const director = this.director.find(d => d.id === directorId);
    return director ? `${director.firstName} ${director.lastName}` : 'N/A';  
  }

  movieToDelete: any = null;

  deleteMovie(movie: any): void {
    this.movieToDelete = movie;
    const modal = new Modal(document.getElementById('deleteConfirmModal')!);
    modal.show();
  }

  confirmDelete(): void {
    if (this.movieToDelete) {
      this.http.post(MyConfig.APIurl + '/api/Movies/DeleteMovieById?id=' + this.movieToDelete.id, [{}]).subscribe(
        (response) => {
          console.log('Movie delete response: ' + response);
          this.ngOnInit();
          this.movieToDelete = null;
        }
      );
    }
  }
  
  clearModalTextBox():void{
    this.movieToAdd.name="";
    this.isModalGenreDropdownOpen = false;
    this.isModalActorDropdownOpen = false;
  }

addMovie(movieData: any): void {
  const genreIds: number[] = Array.isArray(movieData.moviesGenresIds) 
    ? movieData.moviesGenresIds 
    : [];
  
  const actorIds: number[] = Array.isArray(movieData.moviesActorsIds) 
    ? movieData.moviesActorsIds 
    : [];

  const body = {
    title: movieData.title,
    description: movieData.description,
    releaseDate: movieData.releaseDate,  
    duration: movieData.duration,
    language: movieData.language,
    ageRating: movieData.ageRating,
    directorId: movieData.directorId,
    countryId: movieData.countryId,
    GenreIds: genreIds, 
    ActorIds: actorIds   
  };

  this.http.post<any>(`${MyConfig.APIurl}/api/Movies/CreateMovie`, body).subscribe({
    next: (response) => {
      this.movieToAdd = {
        title: '',
        description: '',
        releaseDate: '',
        duration: null,
        language: '',
        ageRating: '',
        directorId: null,
        countryId: null,
        moviesGenresIds: [], 
        moviesActorsIds: []  
      };
      
      this.isModalGenreDropdownOpen = false;
      this.isModalActorDropdownOpen = false;
      
      this.loadMovies();
      this.loadGenres();
      this.loadActors();
    },
    error: (error) => {
      console.error('Error adding movie:', error);
    }
  });    
}

  prepareEditMovie(movie: any): void {

    this.movieToEdit = {
        id: movie.id,
        title: movie.title,
        description: movie.description,
        releaseDate: movie.releaseDate.split('T')[0], // Format date for input
        duration: movie.duration,
        language: movie.language,
        ageRating: movie.ageRating,
        directorId: movie.directorId.id, // Get ID from director object
        countryId: movie.countryId.id,   // Get ID from country object
        moviesGenresIds: movie.moviesGenresIds.map((g: any) => 
            typeof g === 'object' ? g.id : g), // Handle both object and ID cases
        moviesActorsIds: movie.moviesActorsIds.map((a: any) => 
            typeof a === 'object' ? a.id : a)  // Handle both object and ID cases
        
    };

    this.isModalGenreDropdownOpen = false;
    this.isModalActorDropdownOpen = false;

    console.log('Prepared edit data:', this.movieToEdit);
  }

  updateMovie(movieData: any): void {
    const genreIds: number[] = Array.isArray(movieData.moviesGenresIds) 
    ? movieData.moviesGenresIds 
    : [];
  const actorIds: number[] = Array.isArray(movieData.moviesActorsIds) 
    ? movieData.moviesActorsIds 
    : [];

    const body = {
      title: movieData.title,
      description: movieData.description,
      releaseDate: movieData.releaseDate,
      duration: movieData.duration,
      language: movieData.language,
      ageRating: movieData.ageRating,
      directorId: movieData.directorId,
      countryId: movieData.countryId,
      GenreIds: genreIds, 
      ActorIds: actorIds 
    };
    const url = `${MyConfig.APIurl}/api/Movies/UpdateMovie?id=${movieData.id}`;
  
    this.http.post<any>(url, body).subscribe({
      next: (response) => {
        console.log('Movie updated:', response);
        this.ngOnInit(); 
      },
      error: (error) => {
        console.error('Error updating movie:', error);
      }
    });
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
      this.filterParams.pageNumber = 1; // Reset to first page when filtering
      this.loadMovies();
    }, 300);
  }

  resetFilters(): void {
    this.filterParams = {
      pageNumber: 1,
      pageSize: 10,
      searchTerm: '',
      fromDate: null,
      toDate: null,
      language: '',
      ageRating: '',
      directorId: null,
      countryId: null,
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