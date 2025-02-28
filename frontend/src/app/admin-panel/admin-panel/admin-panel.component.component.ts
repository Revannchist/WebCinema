import { Component } from '@angular/core';
import { trigger, transition, style, animate } from '@angular/animations';

@Component({
  selector: 'app-admin-panel',// Fixed selector
  templateUrl: './admin-panel.component.component.html',
  styleUrl: './admin-panel.component.component.css',
  animations: [
    trigger('fadeIn', [
      transition(':enter', [
        style({ opacity: 0 }),
        animate('300ms', style({ opacity: 1 }))
      ])
    ])
  ]
  
})
export class AdminPanelComponentComponent {
  sidebarCollapsed = false;
}
