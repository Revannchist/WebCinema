import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Injectable({
  providedIn: 'root'
})
export class LanguageService {
  constructor(private translate: TranslateService) {
    translate.setDefaultLang('en');
    translate.use(this.getCurrentLang() || 'en');
  }

  getCurrentLang(): string {
    return localStorage.getItem('language') || 'en';
  }

  setLanguage(lang: string) {
    localStorage.setItem('language', lang);
    this.translate.use(lang);
  }
} 