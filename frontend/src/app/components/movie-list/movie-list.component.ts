import { Component, OnInit } from '@angular/core';
import { MovieService } from '../../services/movie-service';
import { Movie, FilterParams } from '../../models/movie.model';
//import { MovieImageService } from '../../services/movie-image-service';

@Component({
  selector: 'app-movie-list',
  templateUrl: './movie-list.component.html',
  styleUrls: ['./movie-list.component.css']
})
export class MovieListComponent implements OnInit {

  movies: Movie[] = [];
  currentPage = 1;
  pageSize = 10;
  totalItems = 0;

  Math = Math;

  filterParams: FilterParams = {
    pageNumber: this.currentPage,
    pageSize: this.pageSize,
    searchTerm: '',
    directorId: null,
    genreIds: [],
    actorIds: [],
    fromDate: null,
    toDate: null,
    language: null,
    ageRating: null,
    countryId: null
  };

  constructor(private movieService: MovieService /*, private movieImageService: MovieImageService */) { }
  ngOnInit(): void {
    this.loadMovies();
  }

  loadMovies(): void {
    this.movieService.getMovies(this.filterParams).subscribe(response => {
      this.movies = response.items;
      this.totalItems = response.totalCount;
    });
  }

  /*
  getImageByMovieId(id: number) {
    this.movieImageService.loadImageByMovieId(id).subscribe(response => {
      const movie = this.movies.find(m => m.id === id);
      if (movie) {
        console.log(response);
        
        movie.image = response.toString();
      }
    });
  }
    */

  onPageChange(page: number): void {
    this.filterParams.pageNumber = page;
    this.loadMovies();
  }

}