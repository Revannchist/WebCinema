import { Component, OnInit, AfterViewInit, NgZone } from '@angular/core';
import { NgForm } from '@angular/forms';
import jsPDF from 'jspdf';
import html2canvas from 'html2canvas';
import { Router } from '@angular/router';
import { loadStripe, Stripe, StripeElements, StripeCardElement } from '@stripe/stripe-js';
import JSBarcode from 'jsbarcode';
import { AuthService } from '../../auth.service';

@Component({
  selector: 'app-payment',
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.css']
})
export class PaymentComponent implements OnInit {
  paymentData: any;
  isProcessing: boolean = false;
  paymentSuccess: boolean = false;
  stripe: Stripe | null = null;
  elements: StripeElements | null = null;
  card: StripeCardElement | null = null;
  errorMessage: string = '';
  stripeReady: boolean = false;

  constructor(private router: Router, private ngZone: NgZone, private authService: AuthService) {}

  async ngOnInit() {
    const data = sessionStorage.getItem('paymentData');
    if (data) {
      this.paymentData = JSON.parse(data);
    }
    this.stripe = await loadStripe('pk_test_51RMaUIR5a4PC69xEBzUKYLKwBGUQBlAm0WeEl9aCqFTAYGyb5gkdLJlPkI0CLFhD9peSI0rHialNWVhFzKzoU0f600UUv4QHS3');
    console.log('Stripe:', this.stripe);
    if (this.stripe) {
      this.stripeReady = true;
      this.mountStripeCardElement();
    }
  }

  mountStripeCardElement() {
    const cardDiv = document.getElementById('card-element');
    if (this.stripe && cardDiv) {
      this.elements = this.stripe.elements();
      this.card = this.elements.create('card');
      try {
        this.card.mount('#card-element');
        console.log('Card mountan na #card-element');
      } catch (err) {
        console.error('Greška pri mountanju Stripe Card elementa:', err);
      }
    } else {
      console.error('Stripe ili card-element nije dostupan!');
    }
  }

  async pay() {
    if (!this.stripeReady) {
      this.errorMessage = 'Stripe nije spreman. Pričekaj trenutak i pokušaj ponovno.';
      return;
    }
    this.isProcessing = true;
    this.errorMessage = '';
    if (!this.stripe || !this.card) {
      this.isProcessing = false;
      this.errorMessage = 'Stripe nije inicijaliziran.';
      return;
    }
    const { paymentMethod, error } = await this.stripe.createPaymentMethod({
      type: 'card',
      card: this.card,
    });
    if (error) {
      this.ngZone.run(() => {
        this.errorMessage = error.message || 'Greška pri plaćanju.';
        this.isProcessing = false;
      });
      return;
    }
    // Ovdje bi inače išao backend poziv za stvarno plaćanje, ali za test je dovoljno da je validacija prošla
    this.ngZone.run(() => {
      this.isProcessing = false;
      this.paymentSuccess = true;
      this.downloadTicketPDF();
    });
  }

  async downloadTicketPDF(): Promise<void> {
    const doc = new jsPDF({ orientation: 'landscape', unit: 'px', format: [700, 220 * (this.paymentData.selectedSeats.length || 1)] });
    const ticketWidth = 650;
    const ticketHeight = 180;
    const margin = 25;
    const startY = 20;
    const gap = 20;
    const showtime = this.paymentData.showtime;
    const seats = this.paymentData.selectedSeats;
    const price = this.paymentData.totalPrice / seats.length;
    let user = this.authService.getCurrentUserName();
    if (!user) user = 'GOST';
    for (let i = 0; i < seats.length; i++) {
      const y = startY + i * (ticketHeight + gap);
      // Vanjski okvir
      doc.setDrawColor(60, 60, 60);
      doc.setLineWidth(1.2);
      doc.roundedRect(margin, y, ticketWidth, ticketHeight, 18, 18, 'S');
      // Pozadina
      doc.setFillColor(255, 245, 230);
      doc.roundedRect(margin, y, ticketWidth, ticketHeight, 18, 18, 'F');
      // Naslov: ime filma (centrirano)
      doc.setTextColor(40, 40, 40);
      doc.setFont('courier', 'bold');
      doc.setFontSize(32);
      doc.text(showtime.movieTitle, margin + ticketWidth/2, y + 45, { align: 'center' });
      // NAME
      doc.setFontSize(13);
      doc.setFont('courier', 'normal');
      doc.text('NAME :', margin + 20, y + 75);
      doc.setFont('courier', 'bold');
      doc.setFontSize(20);
      doc.text(user, margin + 20, y + 98);
      // SEAT
      doc.setFont('courier', 'normal');
      doc.setFontSize(13);
      doc.text('SEAT :', margin + 260, y + 75);
      doc.setFont('courier', 'bold');
      doc.setFontSize(20);
      doc.text(String(seats[i]), margin + 260, y + 98);
      // TICKET NO
      doc.setFont('courier', 'normal');
      doc.setFontSize(13);
      doc.text('TICKET NO :', margin + 420, y + 75);
      doc.setFont('courier', 'bold');
      doc.setFontSize(15);
      doc.text(`${Math.floor(Math.random()*90000000+10000000)}`, margin + 420, y + 98);
      // THEATER
      doc.setFont('courier', 'normal');
      doc.setFontSize(13);
      doc.text('THEATER :', margin + 20, y + 155);
      doc.setFont('courier', 'bold');
      doc.setFontSize(15);
      doc.text(showtime.hallName, margin + 20, y + 170);
      // DATE & TIME
      doc.setFont('courier', 'normal');
      doc.setFontSize(13);
      doc.text('DATE & TIME :', margin + 260, y + 120);
      doc.setFont('courier', 'bold');
      doc.setFontSize(15);
      const dateStr = new Date(showtime.showDateTime).toLocaleDateString();
      const timeStr = new Date(showtime.showDateTime).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
      doc.text(`${dateStr}  ${timeStr}`, margin + 260, y + 138);
      // PRICE
      doc.setFont('courier', 'normal');
      doc.setFontSize(13);
      doc.text('PRICE :', margin + 260, y + 155);
      doc.setFont('courier', 'bold');
      doc.setFontSize(15);
      doc.text(`€${price.toFixed(2)}`, margin + 260, y + 170);
      // Barkod (desna strana, centriran vertikalno)
      const barcodeCanvas = document.createElement('canvas');
      JSBarcode(barcodeCanvas, `SEAT${seats[i]}-${showtime.movieTitle}`, { format: 'CODE128', width: 2, height: 32, displayValue: false });
      const barcodeImg = barcodeCanvas.toDataURL('image/png');
      doc.addImage(barcodeImg, 'PNG', margin + ticketWidth - 110, y + ticketHeight/2 - 16, 80, 32);
    }
    doc.save('WebCinema-Tickets.pdf');
  }

  backToHome(): void {
    this.router.navigate(['/home']);
  }

  // Provjeri je li korisnik u dark modu
  isDarkMode(): boolean {
    return document.body.classList.contains('dark-theme');
  }
}
