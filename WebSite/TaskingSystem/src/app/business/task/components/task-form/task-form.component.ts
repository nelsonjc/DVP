import { CommonModule, NgIf } from '@angular/common';
import { Component, Output, EventEmitter, OnInit, OnDestroy } from '@angular/core';
import { TaskService } from '../../../../core/services/task/task.service';
import { Task } from '../../../../core/models/task/task.model';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { LocalStorageService } from '../../../../core/services/local-storage/local-storage.service';
import { ModalNameEnum } from '../../../../core/enums/modal-name.enum';
import { Subscription } from 'rxjs';
import { ModalService } from '../../../../shared/services/modal/modal.service';
import { Status } from '../../../../core/models/status/status.model';
import { MasterService } from '../../../../core/services/master/master.service';
import { EntityNameEnum } from '../../../../core/enums/entity-name.enum';
import { User } from '../../../../core/models/user/user.model';
import { UserService } from '../../../../core/services/user/user.service';
import { RoleEnum } from '../../../../core/enums/roles.enum';
import { ApiResponseHandlerService } from '../../../../core/services/api-service/api-response-handler.service';

@Component({
  selector: 'app-task-form',
  standalone: true,
  imports: [
    CommonModule, 
    NgIf, 
    ReactiveFormsModule,
    FormsModule
  ],
  templateUrl: './task-form.component.html',
  styleUrl: './task-form.component.css'
})
export class TaskFormComponent implements OnInit, OnDestroy {

  @Output() updateListTaskHome = new EventEmitter<void>();

  task: Task | null = null;
  isModalOpen: boolean = false;
  isUpdateMode = false;
  taskForm!: FormGroup;
  idUserLogin: string;
  statuses: Status[] = [];
  employees: User[] = [];

  private taskSubscription: Subscription | null = null;
  private modalSubscription: Subscription | null = null;

  constructor(
    private taskService: TaskService,
    private lsService: LocalStorageService,
    private modalService: ModalService,
    private masterService: MasterService,
    private userService: UserService,
    private apiResponseHandler: ApiResponseHandlerService,
    private fb: FormBuilder
  ) { }

  ngOnInit(): void {

    this.masterService.getByEntityName(EntityNameEnum.Task).subscribe(x => {
      this.statuses = x.data;
    });

    this.userService.getByRolName(RoleEnum.Empleado).subscribe(x => {
      this.employees = x.data;
    });

    this.idUserLogin = this.lsService.getUser().id;
    this.initForm();

    // Suscribirse al estado de apertura del modal
    this.modalSubscription = this.modalService.isModalOpen(ModalNameEnum.TaskForm).subscribe((isOpen) => {
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
        this.task = task.data; // Almacena la tarea y llena los campos del modal
        this.isUpdateMode = true; // Indica que estamos en modo de actualización
        this.initForm(); // Inicializa el formulario con la tarea
      },
      (error) => {
        console.error('Error fetching task details:', error);
      }
    );
  }

  initForm() {
    const formattedDueDate = this.isUpdateMode && this.task.dueDate
    ? new Date(this.task.dueDate).toISOString().split('T')[0] // Formato "yyyy-MM-dd"
    : '';

    if (!this.isUpdateMode) {
      this.taskForm = this.fb.group({
        title: [this.isUpdateMode ? this.task.title : '', Validators.required],
        description: [this.isUpdateMode ? this.task.description : '', Validators.required],
        dueDate: [this.isUpdateMode ? formattedDueDate : '', Validators.required],
        idUser: [this.isUpdateMode ? this.task.idUser : '', Validators.required], // Simula un usuario autenticado
      });      
    }
    else{
      this.taskForm = this.fb.group({
        title: [this.isUpdateMode ? this.task.title : '', Validators.required],
        description: [this.isUpdateMode ? this.task.description : '', Validators.required],
        dueDate: [this.isUpdateMode ? formattedDueDate : '', Validators.required],
        idUser: [this.isUpdateMode ? this.task.idUser : '', Validators.required], // Simula un usuario autenticado
        idStatus: [this.isUpdateMode ? this.task.idStatus : null, this.isUpdateMode ? Validators.required : null],
        observation: ['', this.isUpdateMode ? Validators.required : null]
      });
    }
  }

  closeModal() {
    this.task = null;
    this.isUpdateMode = false;
    
    if (this.taskForm) {
      this.taskForm.reset();
    }
    
    this.modalService.closeModal(ModalNameEnum.TaskForm);
  }

  onSubmit() {
    if (this.taskForm.valid) {
      if (this.isUpdateMode) {
        // Lógica para actualizar un WorkItem
        const updateCommand = {
          ...this.taskForm.value,
          id: this.task.id,
          idUserUpdated: this.idUserLogin
        };

        this.taskService.update(updateCommand).subscribe({
          next: (message) => {
            this.apiResponseHandler.handleApiResponse(message);
            this.updateListTaskHome.emit();
            this.closeModal();            
          },
          error: (err) => {
            this.apiResponseHandler.handleApiResponse(err);
          }
        });
      } else {
        const createCommand = {
          ...this.taskForm.value,
          idUserCreated: this.idUserLogin
        };


        this.taskService.create(createCommand).subscribe({
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
