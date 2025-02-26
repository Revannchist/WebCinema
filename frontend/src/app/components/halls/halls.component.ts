import { Component } from '@angular/core';
import { HallService } from '../../services/hall-service';
import { HallDisplayDto } from '../../models/dto/showtime.dto';

@Component({
  selector: 'app-halls',
  templateUrl: './halls.component.html',
  styleUrl: './halls.component.css'
})
export class HallsComponent {

  constructor(
    private hallService: HallService
  ) { }

  halls: HallDisplayDto[] = [];

  loadHalls(): void {
    this.hallService.getAllHalls().subscribe({
      next: (data) => {
        this.halls = data;
      },
      error: (error) => {
        console.error('Error fetching halls:', error);
      }
    });
  }
}
