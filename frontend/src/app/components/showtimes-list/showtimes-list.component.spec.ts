import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowtimesListComponent } from './showtimes-list.component';

describe('ShowtimesListComponent', () => {
  let component: ShowtimesListComponent;
  let fixture: ComponentFixture<ShowtimesListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ShowtimesListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ShowtimesListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
