import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { MyConfig } from './my-config';

interface AuthResponse {
  token: string;
}

@Injectable({
  providedIn: 'root'
})


export class AuthService {

  private apiUrl = `${MyConfig.APIurl}/api/Auth`;
  private authStatusSubject = new BehaviorSubject<boolean>(this.isAuthenticated());
  public authStatus$ = this.authStatusSubject.asObservable();


  constructor(private http: HttpClient) { }

  login(credentials: { username: string; password: string }): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/Login`, credentials).pipe(
      tap((response: AuthResponse) => {
        if (response?.token) {
          localStorage.setItem('token', response.token);
          this.authStatusSubject.next(true);
        }
      })
    );
  }

  logout() {
    localStorage.removeItem('token');
    this.authStatusSubject.next(false);
  }

  isAuthenticated(): boolean {
    return !!localStorage.getItem('token');
  }

  getUserRole(): string | null {
    const token = localStorage.getItem('token');
    if (!token) return null;
    try {
      const payload = token.split('.')[1];
      const decodedPayload = JSON.parse(atob(payload));
      return decodedPayload.role || decodedPayload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
    } catch (e) {
      console.error('Error decoding token', e);
      return null;
    }
  }

  getCurrentUserId(): number | null {
    const token = localStorage.getItem('token');
    if (!token) return null;

    try {
      const payload = token.split('.')[1];
      const decodedPayload = JSON.parse(atob(payload));

      const userId = decodedPayload.userId ||
        decodedPayload.sub ||
        decodedPayload.id ||
        decodedPayload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];

      return userId ? +userId : null; // Convert to number with +
    } catch (e) {
      console.error('Error extracting user ID from token', e);
      return null;
    }
  }

  getCurrentUserName(): string | null {
    const token = localStorage.getItem('token');
    if (!token) return null;

    try {
      const payload = token.split('.')[1];
      const decodedPayload = JSON.parse(atob(payload));

      return decodedPayload.name ||
        decodedPayload.unique_name ||
        decodedPayload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] ||
        null;
    } catch (e) {
      console.error('Error extracting username from token', e);
      return null;
    }
  }

  /*
  getCurrentUserName(): string | null {
    const token = localStorage.getItem('token');
    if (!token) return null;
  
    try {
      const payload = token.split('.')[1];
      const decodedPayload = JSON.parse(atob(payload));
  
      return decodedPayload.username || 
             // Keep these as fallbacks during transition
             decodedPayload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] ||
             null;
    } catch (e) {
      console.error('Error extracting username from token', e);
      return null;
    }
  }
  */

  // Method to decode and get the entire token payload
  getDecodedToken(): any {
    const token = localStorage.getItem('token');
    if (!token) return null;

    try {
      const payload = token.split('.')[1];
      return JSON.parse(atob(payload));
    } catch (e) {
      console.error('Error decoding token', e);
      return null;
    }
  }
}