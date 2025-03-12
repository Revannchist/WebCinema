import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-landing-page',
  templateUrl: './landing-page.component.html',
  styleUrls: ['./landing-page.component.css']
})

export class LandingPageComponent {

  constructor(private router: Router) {}

  onEnterClick(): void {
    console.log('Enter clicked');
    this.router.navigate(['/home']);

  }

  onLoginClick(): void {
    console.log('Login clicked');
    this.router.navigate(['/login']);
  }



  /*
  onDemoClick(version: 'dark' | 'light'): void {
    console.log(`${version} version demo clicked`);
  }
  */
}
