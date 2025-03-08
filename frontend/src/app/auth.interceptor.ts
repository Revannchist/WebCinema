import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = localStorage.getItem('token');

    //console.log('Current Token:', token);
    //console.log('Request URL:', req.url);
    //console.log('Outgoing Request:', req);

    if (token) {
      req = req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
          
        }
      });
    }

    return next.handle(req);
  }
}
