import { Component, Input, Output, EventEmitter, HostListener } from '@angular/core';

interface MenuItem {
  icon: string;
  label: string;
  route: string;
  badge?: number;
}

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent {
  @Input() collapsed = false;
  @Output() collapsedChange = new EventEmitter<boolean>();
  @Input() rememberState = true;
  isAnimating = false;
  
  menuItems: MenuItem[] = [
    { icon: 'home', label: 'Dashboard', route: '/admin' },
    { icon: 'film', label: 'Movies', route: '/admin/movies', badge: 3 },
    { icon: 'calendar', label: 'Showtimes', route: '/admin/showtimes' },
    { icon: 'users', label: 'Users', route: '/admin/users' }
  ];

  @HostListener('window:keydown', ['$event'])
  handleKeyDown(event: KeyboardEvent): void {
    if ((event.ctrlKey || event.metaKey) && event.key === 'b') {
      event.preventDefault();
      this.toggleSidebar();
    }
  }

  ngOnInit(): void {
    if (this.rememberState) {
      const savedState = localStorage.getItem('sidebarCollapsed');
      if (savedState !== null) {
        this.collapsed = savedState === 'true';
        this.collapsedChange.emit(this.collapsed);
      }
    }
    this.checkScreenSize();
  }

  @HostListener('window:resize')
  checkScreenSize(): void {
    if (window.innerWidth < 768 && !this.collapsed) {
      this.collapsed = true;
      this.collapsedChange.emit(this.collapsed);
    }
  }

  toggleSidebar(): void {
    if (this.isAnimating) return;
    
    this.isAnimating = true;
    this.collapsed = !this.collapsed;
    this.collapsedChange.emit(this.collapsed);
    
    setTimeout(() => {
      this.isAnimating = false;
    }, 350);
    
    if (this.rememberState) {
      localStorage.setItem('sidebarCollapsed', this.collapsed.toString());
    }
  }
}