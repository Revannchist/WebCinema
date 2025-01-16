import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MyConfig } from '../../my-config';

@Component({
  selector: 'app-directors',
  templateUrl: './directors.component.html',
  styleUrls: ['./directors.component.css']
})

export class DirectorsComponent {  
  constructor(private http: HttpClient) {}

  public directors: any;
  public movies: any[] = [];
  public directorToAdd: any = {
    firstName: '',
    lastName: ''
  };
  public directorToEdit: any = {
    id: null,
    firstName: '',
    lastName: ''
  };
  public search: string = '';
  public filteredDirectors: any;
  private debounceTimer: any;

  ngOnInit(): void {
    this.loadDirectors();
    this.loadMovies();
  }

  loadDirectors(): void {
    this.http.get(MyConfig.APIurl + '/api/Directors/GetAllDirectors').subscribe(response => {
      this.directors = response;
      this.filteredDirectors = response;
      console.log(this.directors);
    });
  }

  loadMovies(): void {
    this.http.get(MyConfig.APIurl + '/api/Movies/GetAllMovies').subscribe(response => {
      this.movies = response as any[];
      console.log('Movies loaded:', this.movies);
    });
  }

  deleteDirector(director: any): void {
    this.http.post(MyConfig.APIurl + '/api/Directors/DeleteDirectorById?id=' + director.id, [{}]).subscribe(
      (response) => {
        console.log('Director delete response: ' + response);
        this.loadDirectors();
      }
    );
  }

  getMoviesForDirector(directorId: number): any[] {
    return this.movies.filter(movie => 
        movie.directorId && movie.directorId.id === directorId
    );
}
  clearModalTextBox(): void {
    this.directorToAdd = {
      firstName: '',
      lastName: ''
    };
  }
  
  addDirector(directorData: any): void {
    const body = {
      firstName: directorData.firstName,
      lastName: directorData.lastName
    };

    this.http.post<any>(`${MyConfig.APIurl}/api/Directors/AddDirector`, body).subscribe({
      next: (response) => {
        this.directorToAdd = {};
        console.log('Director add response: ' + response);
        this.loadDirectors();
      },
      error: (error) => {
        console.error('Error adding director:', error);
      }
    });
  }

  prepareEditDirector(director: any): void {
    this.directorToEdit = {
      id: director.id,
      firstName: director.firstName,
      lastName: director.lastName
    };
  }

  updateDirector(directorData: any): void {
    const body = {
      firstName: directorData.firstName,
      lastName: directorData.lastName
    };

    const url = `${MyConfig.APIurl}/api/Directors/UpdateDirector?id=${directorData.id}`;

    this.http.post<any>(url, body).subscribe({
      next: (response) => {
        console.log('Director updated:', response);
        this.loadDirectors();
      },
      error: (error) => {
        console.error('Error updating director:', error);
      }
    });
  }
  
  filterDirectors(): void {
    clearTimeout(this.debounceTimer);
    this.debounceTimer = setTimeout(() => {
      if (!this.search) {
        this.filteredDirectors = [...this.directors];
      } else {
        this.filteredDirectors = this.directors.filter((director: any) =>
          (director.firstName + ' ' + director.lastName).toLowerCase().includes(this.search.toLowerCase())
        );
      }
    }, 300);
  }
}