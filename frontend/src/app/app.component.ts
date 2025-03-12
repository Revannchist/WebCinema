import { Component, OnInit } from '@angular/core';
import { fadeAnimation } from './services/animation-service';
import { Router, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';
import { AuthService } from './auth.service';
import { CartService } from './services/cart-service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  animations: [fadeAnimation]
})
export class AppComponent implements OnInit {
  title = 'kino';
  sidebarCollapsed = false;
  isAdminRoute = false;
  showNavbar = false;
  isLoggedIn = false;
  cartItemCount = 0;
  
  navbarRoutes = [
    '/home',
    '/movie-list',
    '/showtimes-list',
    '/bookings',
    '/booking-details',
    '/checkout',
    '/login',
    '/register'
  ];
  
  constructor(
    private router: Router,
    private authService: AuthService,
    private cartService: CartService
  ) {
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe((event: any) => {
      this.isAdminRoute = event.url.startsWith('/admin');
      const currentRoute = event.url;
      const isSeatRoute = currentRoute.startsWith('/seats/');
      const isBookingRoute = currentRoute.startsWith('/bookings') ||
        currentRoute.startsWith('/booking-details/') ||
        currentRoute.startsWith('/checkout/');
      this.showNavbar = this.navbarRoutes.some(route =>
        currentRoute === route || currentRoute.startsWith(route + '/')) ||
        isSeatRoute || isBookingRoute;
    });
    
    this.cartService.cartItemCount$.subscribe(count => {
      console.log('Cart count in navbar:', count);
      this.cartItemCount = count;
    });
  }
  
  ngOnInit(): void {
    this.checkAuthStatus();
    this.authService.authStatus$.subscribe((isLoggedIn: boolean) => {
      this.isLoggedIn = isLoggedIn;
      if (isLoggedIn) {
        this.cartService.refreshCartCount();
      } else {
        this.cartItemCount = 0;
        this.cartService.updateCartCount(0);
      }
    });
  }
  
  checkAuthStatus(): void {
    this.isLoggedIn = this.authService.isAuthenticated();
    if (this.isLoggedIn) {
      this.cartService.refreshCartCount();
    }
  }
  
  // Remove the loadCartItemCount method from before since we're now using refreshCartCount
  
  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
  
  onSidebarCollapsedChange(collapsed: boolean): void {
    this.sidebarCollapsed = collapsed;
  }
}