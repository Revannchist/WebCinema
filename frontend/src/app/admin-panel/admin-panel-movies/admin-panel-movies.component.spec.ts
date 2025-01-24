import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminPanelMoviesComponent } from './admin-panel-movies.component';

describe('AdminPanelMoviesComponent', () => {
  let component: AdminPanelMoviesComponent;
  let fixture: ComponentFixture<AdminPanelMoviesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdminPanelMoviesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminPanelMoviesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
