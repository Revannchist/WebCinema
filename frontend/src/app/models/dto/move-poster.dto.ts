export interface CreateMoviePosterDto {
    id: number;
    movieId: number;
    image: string | null;
}

export interface MoviePosterResponseDto {
    id: number;
    image: string;
    imageFormat: string;
}