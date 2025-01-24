import { DirectorDto } from './director.dto.js';
import { ActorDto } from './actor.dto.js';
import { GenreDto } from './genre.dto.js';
import { CountryDto } from './country.dto.js';
//import { MovieImageDto } from './movieimage.dto.js';

export interface MovieDto {
    id: number;
    title: string;
    description: string;
    releaseDate: string;
    duration: number;
    language: string;
    ageRating: string;
    directorId: {
        id: number;
        firstName: string;
        lastName: string;
    };
    countryId: {
        id: number;
        name: string;
    };
    moviesGenresIds: number[];
    moviesActorsIds: number[];
    //poster?: MovieImageDto | undefined;
    //galleryImages?: MovieImageDto[];
}