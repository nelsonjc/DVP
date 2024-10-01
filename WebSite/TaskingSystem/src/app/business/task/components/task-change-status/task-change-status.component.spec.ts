import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskChangeStatusComponent } from './task-change-status.component';

describe('TaskChangeStatusComponent', () => {
  let component: TaskChangeStatusComponent;
  let fixture: ComponentFixture<TaskChangeStatusComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TaskChangeStatusComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TaskChangeStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
