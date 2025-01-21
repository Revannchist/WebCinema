import { Component } from '@angular/core';

@Component({
  selector: 'app-landing-page',
  templateUrl: './landing-page.component.html',
  styleUrl: './landing-page.component.css'
})

export class LandingPageComponent {

  onEnterClick(): void {
    console.log('Enter clicked');
  }

  onLoginClick(): void {
    console.log('Login clicked');
  }

  /*
  onDemoClick(version: 'dark' | 'light'): void {
    console.log(`${version} version demo clicked`);
  }
  */
}
