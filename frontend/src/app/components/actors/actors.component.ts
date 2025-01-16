import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MyConfig } from '../../my-config';

@Component({
  selector: 'app-actors',
  templateUrl: './actors.component.html',
  styleUrl: './actors.component.css'
})

export class ActorsComponent {
  constructor(private http:HttpClient){}

  public actors: any;
  public movies: any[] = [];
  public actorToAdd: any = {
    firstName: '',
    lastName: ''
  };
  public actorToEdit: any = {
    id: null,
    firstName: '',
    lastName: ''
  };
  public search: string = '';
  public filteredActors: any;
  private debounceTimer: any;

  ngOnInit(): void {
    this.loadActors();
    this.loadMovies();
  }

  loadActors(): void {
    this.http.get(MyConfig.APIurl + '/api/Actors/GetAllActors').subscribe(response => {
      this.actors = response;
      this.filteredActors = response;
      console.log(this.actors);
    });
  }

  loadMovies(): void {
    this.http.get(MyConfig.APIurl + '/api/Movies/GetAllMovies').subscribe(response => {
      this.movies = response as any[];
      console.log('Movies loaded:', this.movies);
    });
  }

  deleteActor(actor: any): void {
    this.http.post(MyConfig.APIurl + '/api/Actors/DeleteActorById?id=' + actor.id, [{}]).subscribe(
      (response) => {
        console.log('Actor delete response: ' + response);
        this.loadActors();
      }
    );
  }

  getMoviesForActor(actorId: number): any[] {
    return this.movies.filter(movie => 
      movie.moviesActorsIds && movie.moviesActorsIds.includes(actorId)
    );
  }
  
  clearModalTextBox(): void {
    this.actorToAdd = {
      firstName: '',
      lastName: ''
    };
  }
  
  addActor(actorData: any): void {
    const body = {
      firstName: actorData.firstName,
      lastName: actorData.lastName
    };

    this.http.post<any>(`${MyConfig.APIurl}/api/Actors/AddActor`, body).subscribe({
      next: (response) => {
        this.actorToAdd = {};
        console.log('Actor add response: ' + response);
        this.loadActors();
      },
      error: (error) => {
        console.error('Error adding actor:', error);
      }
    });
  }

  prepareEditActor(actor: any): void {
    this.actorToEdit = {
      id: actor.id,
      firstName: actor.firstName,
      lastName: actor.lastName
    };
  }

  updateActor(actorData: any): void {
    const body = {
      firstName: actorData.firstName,
      lastName: actorData.lastName
    };

    const url = `${MyConfig.APIurl}/api/Actors/UpdateActor?id=${actorData.id}`;

    this.http.post<any>(url, body).subscribe({
      next: (response) => {
        console.log('Actor updated:', response);
        this.loadActors();
      },
      error: (error) => {
        console.error('Error updating actor:', error);
      }
    });
  }
  
  filterActors(): void {
    clearTimeout(this.debounceTimer);
    this.debounceTimer = setTimeout(() => {
      if (!this.search) {
        this.filteredActors = [...this.actors];
      } else {
        this.filteredActors = this.actors.filter((actor: any) =>
          (actor.firstName + ' ' + actor.lastName).toLowerCase().includes(this.search.toLowerCase())
        );
      }
    }, 300);
  }

}
