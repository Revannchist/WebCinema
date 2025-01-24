import { CreateMovieDto } from "./createmovie.dto.js";

export interface UpdateMovieDto extends CreateMovieDto {
    id: number;
}