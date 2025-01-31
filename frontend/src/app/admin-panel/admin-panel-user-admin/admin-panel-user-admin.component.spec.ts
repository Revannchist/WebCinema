import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminPanelUserAdminComponent } from './admin-panel-user-admin.component';

describe('AdminPanelUserAdminComponent', () => {
  let component: AdminPanelUserAdminComponent;
  let fixture: ComponentFixture<AdminPanelUserAdminComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdminPanelUserAdminComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminPanelUserAdminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
