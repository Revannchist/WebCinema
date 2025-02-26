export interface HallDisplayDto {
  id: number;
  theatersID: number;
  theaterName: string;
  hallName: string;
  capacity: number;
  hallType: string;
}

export interface AddHallDto {
  id: number;
  theatersID: number;
  hallName: string;
  capacity: number;
  hallType: string;
}