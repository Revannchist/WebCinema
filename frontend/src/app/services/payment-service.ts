import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MyConfig } from '../my-config';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class PaymentService {
  constructor(private http: HttpClient) {}

  addPayment(paymentData: any): Observable<any> {
    return this.http.post(`${MyConfig.APIurl}/api/Payments/AddPayment`, paymentData);
  }
} 