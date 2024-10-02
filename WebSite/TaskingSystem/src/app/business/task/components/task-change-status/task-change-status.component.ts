import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { ModalService } from '../../../../shared/services/modal/modal.service';
import { ApiResponseHandlerService } from '../../../../core/services/api-service/api-response-handler.service';
import { ModalNameEnum } from '../../../../core/enums/modal-name.enum';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule, NgIf } from '@angular/common';
import { Status } from '../../../../core/models/status/status.model';
import { MasterService } from '../../../../core/services/master/master.service';
import { EntityNameEnum } from '../../../../core/enums/entity-name.enum';
import { TaskService } from '../../../../core/services/task/task.service';
import { Subscription } from 'rxjs';
import { LocalStorageService } from '../../../../core/services/local-storage/local-storage.service';
import { Task } from '../../../../core/models/task/task.model';

@Component({
  selector: 'app-task-change-status',
  standalone: true,
  imports: [
    CommonModule,
    NgIf,
    ReactiveFormsModule,
    FormsModule
  ],
  templateUrl: './task-change-status.component.html',
  styleUrl: './task-change-status.component.css'
})
export class TaskChangeStatusComponent implements OnInit, OnDestroy {
  @Output() updateListTaskHome = new EventEmitter<void>();

  task: Task | null = null;
  isModalOpen: boolean = false;
  taskForm!: FormGroup;
  statuses: Status[] = [];
  statusesAllowed: Status[] = [];
  idUserLogin: string;

  private taskSubscription: Subscription | null = null;
  private modalSubscription: Subscription | null = null;

  constructor(
    private lsService: LocalStorageService,
    private modalService: ModalService,
    private masterService: MasterService,
    private taskService: TaskService,
    private apiResponseHandler: ApiResponseHandlerService,
    private fb: FormBuilder
  ) {

  }
  ngOnInit(): void {
    this.masterService.getByEntityName(EntityNameEnum.Task).subscribe(x => {
      this.statuses = x.data;
      this.statusesAllowed = x.data;
    });

    this.idUserLogin = this.lsService.getUser().id;
    this.initForm();

     // Suscribirse al estado de apertura del modal
     this.modalSubscription = this.modalService.isModalOpen(ModalNameEnum.TaskChangeStatus).subscribe((isOpen) => {
      this.isModalOpen = isOpen;
      if (isOpen) {
        // Obtener el id de la tarea cuando el modal se abra
        this.taskSubscription = this.modalService.getIdFromModal().subscribe((taskId) => {
          if (taskId) {
            this.fetchTask(taskId); 
          }
        });
      }else {
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
        if (task ) {
          this.task = task.data; // Almacena la tarea y llena los campos del modal
          this.initForm(); // Inicializa el formulario con la tarea
          this.statusesAllowed = this.statuses.filter(x => x.id != this.task.idStatus)
        }
      },
      (error) => {
        console.error('Error fetching task details:', error);
      }
    );
  }

  initForm(){
    this.taskForm = this.fb.group({
      idStatus: [null, Validators.required],
      observation: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.taskForm.valid) {

      // Lógica para actualizar un WorkItem
      const changeStatusCommand = {
        ...this.taskForm.value,
        idUserUpdated: this.idUserLogin,
        id: this.task.id,
      };

      this.taskService.changeStatus(changeStatusCommand).subscribe({
        next: (message) => {
          this.apiResponseHandler.handleApiResponse(message);
          this.updateListTaskHome.emit();
          this.closeModal();
        },
        error: (err) => {
          this.apiResponseHandler.handleApiResponse(err);
        }
      });
    }
  }

  closeModal() {
    this.task = null;
    
    if (this.taskForm) {
      this.taskForm.reset();
    }
    
    this.modalService.closeModal(ModalNameEnum.TaskChangeStatus);
  }

  ngOnDestroy() {
    // Desuscribirse de todas las suscripciones
    if (this.modalSubscription) {
      this.modalSubscription.unsubscribe();
    }
    if (this.taskSubscription) {
      this.taskSubscription.unsubscribe();
    }

    this.statusesAllowed = this.statuses;
  }
}
