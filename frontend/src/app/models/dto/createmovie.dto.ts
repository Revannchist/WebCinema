export interface CreateMovieDto {
    title: string;
    description: string;
    releaseDate: string;
    duration: number;
    language: string;
    ageRating: string;
    directorId: number;
    countryId: number;
    GenreIds: number[];
    ActorIds: number[];
}

export interface UpdateMovieDto extends CreateMovieDto {
    id: number;
}