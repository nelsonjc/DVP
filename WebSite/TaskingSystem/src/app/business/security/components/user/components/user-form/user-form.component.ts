import { Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
import { ModalService } from '../../../../../../shared/services/modal/modal.service';
import { ModalNameEnum } from '../../../../../../core/enums/modal-name.enum';
import { Subscription } from 'rxjs';
import { User } from '../../../../../../core/models/user/user.model';
import { UserService } from '../../../../../../core/services/user/user.service';
import { CommonModule, NgIf } from '@angular/common';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { LocalStorageService } from '../../../../../../core/services/local-storage/local-storage.service';
import { ApiResponseHandlerService } from '../../../../../../core/services/api-service/api-response-handler.service';
import { Role } from '../../../../../../core/models/role/role.model';

@Component({
  selector: 'app-user-form',
  standalone: true,
  imports: [    
    CommonModule, 
    NgIf, 
    ReactiveFormsModule,
    FormsModule
  ],
  templateUrl: './user-form.component.html',
  styleUrl: './user-form.component.css'
})
export class UserFormComponent implements OnInit, OnDestroy {


  @Output() updateListUser = new EventEmitter<void>();

  isModalOpen: boolean = false;
  userForm!: FormGroup;
  isUpdateMode = false;
  user: User | null = null;
  idUserLogin: string;
  roles: Role[] = [];

  private taskSubscription: Subscription | null = null;
  private modalSubscription: Subscription | null = null;

  constructor(
    private modalService: ModalService,
    private userService: UserService,
    private lsService: LocalStorageService,
    private apiResponseHandler: ApiResponseHandlerService,
    private fb: FormBuilder

  ){

  }

  ngOnInit(): void {

    this.userService.getRoles({}).subscribe(x => {
      this.roles = x.data;
    });

    this.idUserLogin = this.lsService.getUser().id;
    this.initForm();

    // Suscribirse al estado de apertura del modal
    this.modalSubscription = this.modalService.isModalOpen(ModalNameEnum.UserForm).subscribe((isOpen) => {
      this.isModalOpen = isOpen;
      if (isOpen) {
        
        document.body.classList.add('overflow-hidden');

        this.taskSubscription = this.modalService.getIdFromModal().subscribe((userId) => {
          if (userId) {          
            this.isUpdateMode = true;  
            this.fetchUser(userId); 
          }
          else{
            this.isUpdateMode = false;  
          }
        });
      }else {
        this.user = null; // Limpiar tarea cuando se cierra el modal
        if (this.taskSubscription) {
          this.taskSubscription.unsubscribe(); // Desuscribirse para evitar múltiples consultas
        }
      }
    });
  }

  fetchUser(idUser: string) {
    this.userService.getById(idUser).subscribe(
      (user) => {
        if (user) {
          this.user = user.data; 
          this.initForm();
        }
      },
      (error) => {
        console.error('Error fetching task details:', error);
      }
    );
  }

  initForm() {

    if (this.isUpdateMode) {
      this.userForm = this.fb.group({
        fullName: [this.user.fullName, Validators.required],
        userName: [this.user.userName, Validators.required],
        active: [this.user.active, Validators.required],
        idRole: [this.user.idRole, Validators.required],
        password: ['', null],
      });      
    }
    else{
      this.userForm = this.fb.group({
        fullName: ['', Validators.required],
        userName: ['', Validators.required],
        active: ['', Validators.required],
        idRole: ['', Validators.required],
        password: ['', Validators.required],
      });
    }
  }

  onSubmit() {
    if (this.userForm.valid) {
      if (this.isUpdateMode) {
        const updateCommand = {
          ...this.userForm.value,
          id: this.user.id,
          idUserUpdated: this.idUserLogin
        };

        console.log(updateCommand);

        this.userService.update(updateCommand).subscribe({
          next: (message) => {
            this.apiResponseHandler.handleApiResponse(message);
            this.updateListUser.emit();
            this.closeModal();            
          },
          error: (err) => {
            this.apiResponseHandler.handleApiResponse(err);
          }
        });
      } else {
        const createCommand = {
          ...this.userForm.value,
          idUserCreated: this.idUserLogin
        };

        console.log(createCommand);



        this.userService.create(createCommand).subscribe({
          next: (message) => {
            this.apiResponseHandler.handleApiResponse(message);
            this.updateListUser.emit();
            this.closeModal();            
          },
          error: (err) => {
            this.apiResponseHandler.handleApiResponse(err);
          }
        });
      }
    }
  }

  closeModal() {
    this.user = null;
    this.isUpdateMode = false;
    
    if (this.userForm) {
      this.userForm.reset();
    }
    
    document.body.classList.remove('overflow-hidden');
    this.modalService.closeModal(ModalNameEnum.UserForm);
  }

  ngOnDestroy() {
    // Desuscribirse de todas las suscripciones
    if (this.modalSubscription) {
      this.modalSubscription.unsubscribe();
    }
    if (this.taskSubscription) {
      this.taskSubscription.unsubscribe();
    }

    document.body.classList.remove('overflow-hidden');
  }
}
