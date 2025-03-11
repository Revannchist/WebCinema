import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of, throwError } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { MyConfig } from '../my-config';


@Injectable({
  providedIn: 'root'
})

export class CartService {
  private cartItemCountSubject = new BehaviorSubject<number>(0);
  cartItemCount$ = this.cartItemCountSubject.asObservable();

  updateCartCount(count: number): void {
    this.cartItemCountSubject.next(count);
  }
}