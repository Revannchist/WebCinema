export interface Movie {
  id?: number;
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
  //image?: string;
}

export interface PagedResponse<T> {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
}

export interface FilterParams {
  pageNumber: number;
  pageSize: number;
  searchTerm?: string;
  directorId?: number | null;
  genreIds?: number[];
  actorIds?: number[];
  fromDate?: Date | null;
  toDate?: Date | null;
  language?: string | null;
  ageRating?: string | null;
  countryId?: number | null;
}