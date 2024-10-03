import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { UserService } from '../../../../../../core/services/user/user.service';
import { CommonModule } from '@angular/common';
import { Subscription } from 'rxjs';
import { ModalService } from '../../../../../../shared/services/modal/modal.service';
import { ModalNameEnum } from '../../../../../../core/enums/modal-name.enum';
import { LocalStorageService } from '../../../../../../core/services/local-storage/local-storage.service';
import { NotificationService } from '../../../../../../core/services/notification/notification.service';

@Component({
  selector: 'app-user-change-password',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './user-change-password.component.html',
  styleUrl: './user-change-password.component.css'
})
export class UserChangePasswordComponent implements OnInit, OnDestroy {
  isModalOpen: boolean = false;
  changePasswordForm: FormGroup;
  errorMessage: string = '';
  successMessage: string = '';
  idUserLogin: string;
  idUser: string;


  private taskSubscription: Subscription | null = null;
  private modalSubscription: Subscription | null = null;

  constructor(
    private modalService: ModalService,
    private userService: UserService,
    private lsService: LocalStorageService,
    private notificationService: NotificationService,
    private fb: FormBuilder,
  ) {

  }
  ngOnInit(): void {

    this.changePasswordForm = this.fb.group({
      newPassword: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required]
    }, {
      validators: this.passwordMatchValidator
    });

    this.idUserLogin = this.lsService.getUser().id;

    // Suscribirse al estado de apertura del modal
    this.modalSubscription = this.modalService.isModalOpen(ModalNameEnum.UserChangePasswordForm).subscribe((isOpen) => {
      this.isModalOpen = isOpen;
      if (isOpen) {

        document.body.classList.add('overflow-hidden');

        this.taskSubscription = this.modalService.getIdFromModal().subscribe((userId) => {
          if (userId) {
            this.idUser = userId;
          }
        });
      } else {
        if (this.taskSubscription) {
          this.taskSubscription.unsubscribe(); // Desuscribirse para evitar múltiples consultas
        }
      }
    });
  }


  passwordMatchValidator(formGroup: FormGroup) {
    return formGroup.get('newPassword')?.value === formGroup.get('confirmPassword')?.value
      ? null : { mismatch: true };
  }

  changePassword() {
    if (this.changePasswordForm.valid) {
      const { newPassword } = this.changePasswordForm.value;

      this.userService.changePassword({ idUser: this.idUser, password: newPassword, idUserUpdated: this.idUserLogin }).subscribe({
        next: (response) => {
          this.successMessage = 'Contraseña cambiada con éxito.';
          this.errorMessage = '';
          this.notificationService.showSuccess('La contraseña ha sido cambiado con exito!');
          this.closeModal();
        },
        error: (error) => {
          this.errorMessage = 'Error al cambiar la contraseña: ' + error.message;
          this.successMessage = '';
        }
      });
    }
  }

  closeModal() {

    if (this.changePasswordForm) {
      this.changePasswordForm.reset();
    }

    document.body.classList.remove('overflow-hidden');
    this.modalService.closeModal(ModalNameEnum.UserChangePasswordForm);
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
