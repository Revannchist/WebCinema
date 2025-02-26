export interface HallDisplayDto {
    id: number;
    theatersID: number;
    theaterName: string;
    hallName: string;
    capacity: number;
    hallType: string;
}

export interface AddShowTimeDto {
    id: number;
    moviesId: number;
    hallsId: number;
    showDateTime: string;
    ticketPrice: number;
    isActive: boolean;
}

export interface GetShowTimeDto {
    id: number;
    moviesId: number;
    movieTitle: string;
    hallsId: number;
    hallName: string;
    showDateTime: string;
    ticketPrice: number;
    isActive: boolean;
}
