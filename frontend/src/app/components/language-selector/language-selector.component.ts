import { Component } from '@angular/core';
import { LanguageService } from '../../services/language.service';

@Component({
  selector: 'app-language-selector',
  template: `
    <div class="language-selector">
      <button (click)="changeLanguage('bs')" [class.active]="currentLang === 'bs'">BS</button>
      <button (click)="changeLanguage('en')" [class.active]="currentLang === 'en'">EN</button>
      <button (click)="changeLanguage('hr')" [class.active]="currentLang === 'hr'">HR</button>
    </div>
  `,
  styles: [`
    .language-selector {
      display: flex;
      gap: 10px;
    }
    button {
      padding: 5px 10px;
      border: none;
      border-radius: 4px;
      cursor: pointer;
    }
    .active {
      background-color: #007bff;
      color: white;
    }
  `]
})
export class LanguageSelectorComponent {
  currentLang: string;

  constructor(private languageService: LanguageService) {
    this.currentLang = this.languageService.getCurrentLang();
  }

  changeLanguage(lang: string) {
    this.currentLang = lang;
    this.languageService.setLanguage(lang);
  }
} 