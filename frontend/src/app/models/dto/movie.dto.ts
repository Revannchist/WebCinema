import { DirectorDto } from "./director.dto";
import { CountryDto } from "./country.dto";

export interface MovieCreateDto {
  title: string;
  description: string;
  releaseDate: string;
  duration: number;
  language: string;
  ageRating: string;
  directorId: number;
  countryId: number;
  genreIds: number[];
  actorIds: number[];
}

export interface MovieUpdateDto {
  title: string;
  description: string;
  releaseDate: string;
  duration: number;
  language: string;
  ageRating: string;
  directorId: number;
  countryId: number;
  genreIds: number[];
  actorIds: number[];
}


export interface MovieGetDto {
  id: number;
  title: string;
  description: string;
  releaseDate: string;
  duration: number;
  language: string;
  ageRating: string;
  directorId: DirectorDto;
  countryId: CountryDto;
  moviesGenresIds: number[];
  moviesActorsIds: number[];
}

export interface MovieParameters {
  pageNumber: number;
  pageSize: number;
  searchTerm?: string;
  fromDate?: string;
  toDate?: string;
  language?: string;
  ageRating?: string;
  directorId?: number;
  countryId?: number;
  genreIds: number[];
  actorIds: number[];
}

export interface MoviePagedResponse<T> {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
}