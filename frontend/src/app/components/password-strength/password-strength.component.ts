import { Component, Input, OnChanges } from '@angular/core';
import { PasswordStrengthService } from '../../services/password-strength.service';

@Component({
  selector: 'app-password-strength',
  template: `
    <div class="strength-meter">
      <div class="strength-meter-fill" 
           [style.width.%]="strength"
           [style.background-color]="color">
      </div>
    </div>
    <div class="strength-text" [style.color]="color">
      {{ getStrengthText() }}
    </div>
  `,
  styles: [`
    .strength-meter {
      height: 5px;
      background-color: #ddd;
      margin: 10px 0;
      border-radius: 3px;
    }
    .strength-meter-fill {
      height: 100%;
      border-radius: 3px;
      transition: all 0.3s;
    }
    .strength-text {
      font-size: 12px;
      text-align: left;
      margin-bottom: 10px;
    }
  `]
})
export class PasswordStrengthComponent implements OnChanges {
  @Input() password: string = '';
  strength: number = 0;
  color: string = '#DD2C00';

  constructor(private passwordStrengthService: PasswordStrengthService) {}

  ngOnChanges() {
    this.strength = this.passwordStrengthService.checkStrength(this.password);
    this.color = this.passwordStrengthService.getColor(this.strength);
  }

  getStrengthText() {
    if (this.strength <= 25) return 'Weak';
    if (this.strength <= 50) return 'Fair';
    if (this.strength <= 75) return 'Good';
    return 'Strong';
  }
} 