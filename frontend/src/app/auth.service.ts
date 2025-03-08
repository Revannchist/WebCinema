import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MyConfig } from './my-config';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = `${MyConfig.APIurl}/api/Auth`;

  constructor(private http: HttpClient) { }

  login(credentials: { username: string; password: string }): Observable<any> {
    return this.http.post(`${this.apiUrl}/Login`, credentials);
  }

  logout() {
    localStorage.removeItem('token');
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
 
}


/*
login(credentials: { username: string; password: string }): Observable<{ token: string }> {
  return this.http.post<{ token: string }>(`${this.apiUrl}/Login`, credentials).pipe(
    tap(response => localStorage.setItem('token', response.token))
  );
}
*/
