import { DatePipe, NgFor, NgIf } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Task } from '../../../core/models/task/task.model';
import { TaskFiltersRequest } from '../../../core/models/task/task.filters.request';
import { Metadata, ResultFilterModel } from '../../../core/models/result.model';
import { ModalService } from '../../../shared/services/modal/modal.service';
import { ModalNameEnum } from '../../../core/enums/modal-name.enum';
import { TaskChangeStatusComponent } from '../components/task-change-status/task-change-status.component';
import { TaskDetailComponent } from '../components/task-detail/task-detail.component';
import { TaskFormComponent } from '../components/task-form/task-form.component';
import { TaskService } from '../../../core/services/task/task.service';
import { Status } from '../../../core/models/status/status.model';
import { User } from '../../../core/models/user/user.model';
import { MasterService } from '../../../core/services/master/master.service';
import { EntityNameEnum } from '../../../core/enums/entity-name.enum';
import { RoleEnum } from '../../../core/enums/roles.enum';
import { UserService } from '../../../core/services/user/user.service';
import { AuthService } from '../../../core/services/auth/auth.service';
import { ScreenEnum } from '../../../core/enums/screen.enum';
import { PermissionEnum } from '../../../core/enums/permission.enum';

@Component({
  selector: 'app-task',
  standalone: true,
  imports: [
    TaskFormComponent,
    TaskDetailComponent,
    TaskChangeStatusComponent,
    FormsModule,
    NgFor, NgIf, DatePipe
  ],
  templateUrl: './task.component.html',
  styleUrl: './task.component.css'
})
export default class TaskComponent implements OnInit {

  tasks: Task[] = [];
  metadata: Metadata;
  statuses: Status[] = [];
  employees: User[] = [];
  errorMessage: string = '';
  errorMessageDueDate: string = '';
  activeCleanFilters: boolean = false;

  filters: TaskFiltersRequest = {
    title: '',
    creationDateStart: '',
    creationDateEnd: '',
    dueDateStart: '',
    dueDateEnd: '',
    idUser: '',
    idStatus: '',
    pageNumber: 1,
    rowsOfPage: 30
  };

  constructor(
    private taskService: TaskService,
    private modalService: ModalService,
    private masterService: MasterService,
    private userService: UserService,
    private authService: AuthService
  ) {
  }


  ngOnInit(): void {
    this.loadTasks(this.filters.pageNumber, this.filters.rowsOfPage);

    this.masterService.getByEntityName(EntityNameEnum.Task).subscribe(x => {
      this.statuses = x.data;
    });

    this.userService.getByRolName(RoleEnum.Empleado).subscribe(x => {
      this.employees = x.data;
    });
  }

  // Validar que la segunda fecha sea requerida y no menor que la primera
  validateDates() {
    this.errorMessage = ''; // Limpiar mensaje de error

    // Validación: si la primera fecha está seleccionada y la segunda está vacía
    if (this.filters.creationDateStart && !this.filters.creationDateEnd) {
      this.errorMessage = 'La segunda fecha es requerida';
      return;
    }

    if (!this.filters.creationDateStart && this.filters.creationDateEnd) {
      this.errorMessage = 'La primera fecha es requerida';
      return;
    }

    // Validación: si ambas fechas están seleccionadas, comparar
    if (this.filters.creationDateStart && this.filters.creationDateEnd) {
      const date1 = new Date(this.filters.creationDateStart);
      const date2 = new Date(this.filters.creationDateEnd);

      // Si la segunda fecha es menor que la primera, mostrar error
      if (date2 < date1) {
        this.errorMessage = 'La segunda fecha no puede ser menor que la primera.';
        this.filters.creationDateEnd = null; // Limpiar la segunda fecha si es inválida
      }
    }
  }

  validateDueDates() {
    this.errorMessageDueDate = ''; // Limpiar mensaje de error

    // Validación: si la primera fecha está seleccionada y la segunda está vacía
    if (this.filters.dueDateStart && !this.filters.dueDateEnd) {
      this.errorMessageDueDate = 'La segunda fecha es requerida';
      return;
    }

    if (!this.filters.dueDateStart && this.filters.dueDateEnd) {
      this.errorMessageDueDate = 'La primera fecha es requerida';
      return;
    }

    // Validación: si ambas fechas están seleccionadas, comparar
    if (this.filters.dueDateStart && this.filters.dueDateEnd) {
      const date1 = new Date(this.filters.dueDateStart);
      const date2 = new Date(this.filters.dueDateEnd);

      // Si la segunda fecha es menor que la primera, mostrar error
      if (date2 < date1) {
        this.errorMessageDueDate = 'La segunda fecha no puede ser menor que la primera.';
        this.filters.dueDateEnd = null; // Limpiar la segunda fecha si es inválida
      }
    }
  }

  openCalendar(event: Event) {
    const element = event.target as HTMLInputElement;
    element.showPicker();  // Forzar la apertura del calendario
  }

  loadTasks(pageNumber: number, rowsOfPage: number) {
    this.taskService.getTaskByUserRole({ pageNumber, rowsOfPage }).subscribe(
      (response: ResultFilterModel<Task[]> | null) => {
        if (response && response.data) {
          this.tasks = response.data.data; // Asegúrate de que esto sea un array de Task
          this.metadata = response.metadata; // Almacena el total de tareas
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

  applyFilters() {
    console.log(this.filters);

    if (this.isApplyFilters()) {
      this.activeCleanFilters = true;
      this.taskService.getTaskByUserRole(this.filters).subscribe(
        (response: ResultFilterModel<Task[]> | null) => {
          if (response && response.data) {
            this.tasks = response.data.data; // Asegúrate de que esto sea un array de Task
            this.metadata = response.metadata; // Almacena el total de tareas
          } else {
            console.log('No tasks found');
          }
        },
        (error) => {
          console.error('Error fetching tasks:', error);
        }
      );
    }
  }

  isApplyFilters(): boolean {
    // Comprobar si las propiedades tienen valores válidos
    return (
      this.filters.title !== '' ||
      this.filters.creationDateStart !== '' ||
      this.filters.creationDateEnd !== '' ||
      this.filters.dueDateStart !== '' ||
      this.filters.dueDateEnd !== '' ||
      this.filters.idUser !== '' ||
      this.filters.idStatus !== '' ||
      this.filters.pageNumber != 1 ||
      this.filters.rowsOfPage != 30);
  }

  cleanFilerts() {
    this.filters = {
      title: '',
      creationDateStart: '',
      creationDateEnd: '',
      dueDateStart: '',
      dueDateEnd: '',
      idUser: '',
      idStatus: '',
      pageNumber: 1,
      rowsOfPage: 30
    };
    this.activeCleanFilters = false;
    this.loadTasks(this.filters.pageNumber, this.filters.rowsOfPage);
  }

  previousPage() {
    if (this.metadata.hasPreviousPage) {
      this.filters.pageNumber = this.filters.pageNumber - 1;
      console.log(this.filters);
      this.applyFilters();      
    }
  }

  nextPage() {
    if (this.metadata.hasNextPage) {
      this.filters.pageNumber = this.filters.pageNumber + 1;
      console.log(this.filters);
      this.applyFilters();      
    }
  }

  isCreateTaskAllowed() : boolean {
    return this.authService.validatepermission(ScreenEnum.Task, PermissionEnum.Create);
  }
  
  isEditTaskAllowed() : boolean {
    return this.authService.validatepermission(ScreenEnum.Task, PermissionEnum.Update);
  }
}
