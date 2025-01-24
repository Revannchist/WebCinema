export interface MovieFilterParamsDto {
    pageNumber: number;
    pageSize: number;
    searchTerm: string;
    fromDate: Date | null;
    toDate: Date | null;
    language: string;
    ageRating: string;
    directorId: number | null;
    countryId: number | null;
    genreIds: number[];
    actorIds: number[];
}