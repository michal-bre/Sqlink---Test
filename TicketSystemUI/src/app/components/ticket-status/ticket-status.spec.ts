import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TicketStatus } from './ticket-status';

describe('TicketStatus', () => {
  let component: TicketStatus;
  let fixture: ComponentFixture<TicketStatus>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TicketStatus]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TicketStatus);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
