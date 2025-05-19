import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.css']
})
export class AdminDashboardComponent implements OnInit {
  chartData: any = null;
  chartLabels: string[] = [];
  chartBooked: number[] = [];
  chartSold: number[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.loadChartData();
  }

  loadChartData() {
    this.http.get<any[]>('https://localhost:7057/api/Movies/GetMoviesStatistics').subscribe({
      next: data => {
        if (!data || data.length === 0) {
          this.chartLabels = ['Test Film 1', 'Test Film 2'];
          this.chartBooked = [5, 8];
          this.chartSold = [3, 6];
        } else {
          this.chartLabels = data.map(x => x.title);
          this.chartBooked = data.map(x => x.booked);
          this.chartSold = data.map(x => x.sold);
        }
        this.chartData = {
          labels: this.chartLabels,
          datasets: [
            {
              label: 'Booked',
              data: this.chartBooked,
              backgroundColor: 'rgba(54, 162, 235, 0.7)'
            },
            {
              label: 'Sold',
              data: this.chartSold,
              backgroundColor: 'rgba(255, 99, 132, 0.7)'
            }
          ]
        };
      },
      error: err => {
        this.chartLabels = ['Test Film 1', 'Test Film 2'];
        this.chartBooked = [5, 8];
        this.chartSold = [3, 6];
        this.chartData = {
          labels: this.chartLabels,
          datasets: [
            {
              label: 'Booked',
              data: this.chartBooked,
              backgroundColor: 'rgba(54, 162, 235, 0.7)'
            },
            {
              label: 'Sold',
              data: this.chartSold,
              backgroundColor: 'rgba(255, 99, 132, 0.7)'
            }
          ]
        };
        console.error('Greška kod dohvata statistike:', err);
      }
    });
  }
}
