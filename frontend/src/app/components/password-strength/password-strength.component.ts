import { Component, Input, OnChanges } from '@angular/core';
import { PasswordStrengthService } from '../../services/password-strength.service';
import { TranslateService } from '@ngx-translate/core';

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
      {{ getStrengthText() | translate }}
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

  constructor(
    private passwordStrengthService: PasswordStrengthService,
    private translate: TranslateService
  ) {}

  ngOnChanges() {
    this.strength = this.passwordStrengthService.checkStrength(this.password);
    this.color = this.passwordStrengthService.getColor(this.strength);
  }

  getStrengthText() {
    if (this.strength <= 25) return 'USER_REGISTRATION.PASSWORD_STRENGTH.WEAK';
    if (this.strength <= 50) return 'USER_REGISTRATION.PASSWORD_STRENGTH.FAIR';
    if (this.strength <= 75) return 'USER_REGISTRATION.PASSWORD_STRENGTH.GOOD';
    return 'USER_REGISTRATION.PASSWORD_STRENGTH.STRONG';
  }
} 