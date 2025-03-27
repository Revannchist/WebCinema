import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router) { }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    const isAdmin = this.authService.getUserRole() === 'Admin' || this.authService.getUserRole() === 'admin';

    if (isAdmin) {
      return true;
    }

    // Redirect to home page if not an admin
    this.router.navigate(['/home']);
    return false;
  }
}

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router) { }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    const isAuthenticated = this.authService.isAuthenticated();

    if (isAuthenticated) {
      return true;
    }

    // Redirect to login page if not authenticated
    this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
    return false;
  }
}