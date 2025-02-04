import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class PasswordStrengthService {
  checkStrength(password: string): number {
    let strength = 0;
    
    if (password.length >= 5) strength += 25;
    if (password.match(/[a-z]+/)) strength += 25;
    if (password.match(/[A-Z]+/)) strength += 25;
    if (password.match(/[0-9]+/)) strength += 25;
    
    return strength;
  }

  getColor(strength: number): string {
    if (strength <= 25) return '#DD2C00';
    if (strength <= 50) return '#FF6D00';
    if (strength <= 75) return '#FFD600';
    return '#00C853';
  }
} 