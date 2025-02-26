import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminPanelShowtimesComponent } from './admin-panel-showtimes.component';

describe('AdminPanelShowtimesComponent', () => {
  let component: AdminPanelShowtimesComponent;
  let fixture: ComponentFixture<AdminPanelShowtimesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdminPanelShowtimesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminPanelShowtimesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
