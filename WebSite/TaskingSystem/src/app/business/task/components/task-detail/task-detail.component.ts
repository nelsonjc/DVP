import { DatePipe, NgClass, NgFor, NgIf } from '@angular/common';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { TaskService } from '../../../../core/services/task/task.service';
import { Task } from '../../../../core/models/task/task.model';
import { ModalNameEnum } from '../../../../core/enums/modal-name.enum';
import { ModalService } from '../../../../shared/services/modal/modal.service';
import { Subscription } from 'rxjs';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-task-detail',
  standalone: true,
  imports: [
    FormsModule, 
    ReactiveFormsModule,
    NgIf, NgFor, NgClass, DatePipe
  ],
  templateUrl: './task-detail.component.html',
  styleUrl: './task-detail.component.css'
})
export class TaskDetailComponent implements OnInit, OnDestroy {

  task: Task | null = null;
  isModalOpen: boolean = false;
  formattedDateCreated: string;
  formattedDueDate: string;
  formattedDateUpdated: string;


  private taskSubscription: Subscription | null = null;
  private modalSubscription: Subscription | null = null;

  // Variable to hold the active tab index
  activeTab: number = 0;

  // Array to hold the tab names
  tabs: string[] = ["Información Principal", "Flujo de Estados", "Histórico"];

  constructor(
    private taskService: TaskService,
    private modalService: ModalService,
    private fb: FormBuilder
  ) {

  }

  ngOnInit(): void {
    // Suscribirse al estado de apertura del modal
    this.modalSubscription = this.modalService.isModalOpen(ModalNameEnum.TaskDetail).subscribe((isOpen) => {
      this.isModalOpen = isOpen;
      if (isOpen) {
        // Obtener el id de la tarea cuando el modal se abra
        this.taskSubscription = this.modalService.getIdFromModal().subscribe((taskId) => {
          if (taskId) {
            this.fetchTask(taskId);
          }
        });
      } else {
        this.task = null; // Limpiar tarea cuando se cierra el modal
        if (this.taskSubscription) {
          this.taskSubscription.unsubscribe(); // Desuscribirse para evitar múltiples consultas
        }
      }
    });
  }

  fetchTask(idTask: string) {
    this.taskService.getById(idTask).subscribe(
      (task) => {
        this.task = task.data; // Almacena la tarea y llena los campos del modal
        this.initForm(); // Inicializa el formulario con la tarea
      },
      (error) => {
        console.error('Error fetching task details:', error);
      }
    );
  }

  initForm() {
    this.formattedDateCreated = new Date(this.task.dateCreated).toISOString().split('T')[0];
    this.formattedDueDate = new Date(this.task.dueDate).toISOString().split('T')[0];
    this.formattedDateUpdated = this.task.dateUpdated ? new Date(this.task.dateUpdated).toISOString().split('T')[0] : '';
  }

  getLogClass(type: string){
    switch (type) {
      case 'Creacioó':
        return 'border-green-600'; 
      case 'Actualización':
          return 'border-yellow-600'; 
      case 'Cambio de Estado':
        return 'border-purple-600'; 
      case 'Eliminado':
        return 'border-red-600'; 
      default:
        return 'border-gray-600'; 
    }
  }

  setActiveTab(index: number): void {
    this.activeTab = index;
  }

  prevTab(): void {
    if (this.activeTab > 0) {
      this.activeTab--;
    }
  }

  nextTab(): void {
    if (this.activeTab < this.tabs.length - 1) {
      this.activeTab++;
    }
  }

  closeModal() {
    this.task = null;
    this.activeTab = 0;

    this.modalService.closeModal(ModalNameEnum.TaskDetail);
  }

  ngOnDestroy() {
    // Desuscribirse de todas las suscripciones
    if (this.modalSubscription) {
      this.modalSubscription.unsubscribe();
    }
    if (this.taskSubscription) {
      this.taskSubscription.unsubscribe();
    }
  }
}
