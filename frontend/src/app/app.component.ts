import { Component } from '@angular/core';
import { fadeAnimation } from './services/animation-service';
import { Router, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  animations: [fadeAnimation]
})
export class AppComponent {
  title = 'kino';
  sidebarCollapsed = false;
  isAdminRoute = false;
  showNavbar = false;

  navbarRoutes = [ 
    '/home',
    '/movie-list',
    '/showtimes-list'
  ];

  constructor(private router: Router) {
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe((event: any) => {
      this.isAdminRoute = event.url.startsWith('/admin');
      
      this.showNavbar = this.navbarRoutes.includes(event.url);
    });
  }

  onSidebarCollapsedChange(collapsed: boolean): void {
    this.sidebarCollapsed = collapsed;
  }
}
