import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { TaskService } from '../../../../core/services/task/task.service';
import { ResultFilterModel } from '../../../../core/models/result.model';
import { Task } from '../../../../core/models/task/task.model';
import { DatePipe, NgClass, NgFor, NgIf } from '@angular/common';
import { TaskFormComponent } from '../../../task/components/task-form/task-form.component';
import { Router } from '@angular/router';
import { TaskDetailComponent } from '../../../task/components/task-detail/task-detail.component';
import { ModalNameEnum } from '../../../../core/enums/modal-name.enum';
import { ModalService } from '../../../../shared/services/modal/modal.service';
import { TaskChangeStatusComponent } from '../../../task/components/task-change-status/task-change-status.component';

@Component({
  selector: 'app-user-task-table',
  standalone: true,
  imports: [
    TaskFormComponent, 
    TaskDetailComponent, 
    TaskChangeStatusComponent,
    NgClass, NgIf, NgFor, DatePipe
  ],
  templateUrl: './user-task-table.component.html',
  styleUrls: ['./user-task-table.component.css']
})
export class UserTaskTableComponent implements OnInit {

  isModalFormTaskOpen: boolean = false;
  isModalDetailTaskOpen: boolean = false;
  tasks: Task[] = [];
  totalTasks: number = 0;

  constructor(
    private taskService: TaskService,
    private modalService: ModalService,
    private router: Router
  ) {
  }

  ngOnInit(): void {
    this.loadTasks(1, 10); // Cambia los números según tus necesidades
  }

  loadTasks(pageNumber: number, rowsOfPage: number) {
    this.taskService.getTaskByUserRole({ pageNumber, rowsOfPage }).subscribe(
      (response: ResultFilterModel<Task[]> | null) => {
        if (response && response.data) {
          this.tasks = response.data.data; // Asegúrate de que esto sea un array de Task
          this.totalTasks = response.metadata.totalCount; // Almacena el total de tareas
        } else {
          console.log('No tasks found');
        }
      },
      (error) => {
        console.error('Error fetching tasks:', error);
      }
    );
  }

  truncateText(text: string, maxLength: number): string {
    if (text.length > maxLength) {
      const lastSpaceIndex = text.lastIndexOf(' ', maxLength);
      return text.substring(0, lastSpaceIndex !== -1 ? lastSpaceIndex : maxLength) + '...';
    }
    return text;
  }

  goToTasks() {
    this.router.navigate(['/tasks']);
  }

  openFormTaskModal(idTask: string): void {
    this.modalService.openModal(ModalNameEnum.TaskForm, idTask);
  }

  closeFormTaskModal(): void {
    this.modalService.closeModal(ModalNameEnum.TaskForm);
  }

  openDetailTaskModal(idTask: string): void {
    this.modalService.openModal(ModalNameEnum.TaskDetail, idTask);
  }

  closeDetailTaskModal(): void {
    this.modalService.closeModal(ModalNameEnum.TaskDetail);
  }

  openChangeStatusTaskModal(idTask: string): void {
    this.modalService.openModal(ModalNameEnum.TaskChangeStatus, idTask);
  }

  closeChangeStatusTaskModal(): void {
    this.modalService.closeModal(ModalNameEnum.TaskChangeStatus);
  }
}
