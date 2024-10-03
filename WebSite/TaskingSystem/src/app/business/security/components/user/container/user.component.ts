import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { CommonModule, DatePipe, NgClass, NgFor } from '@angular/common';
import { UserService } from '../../../../../core/services/user/user.service';
import { User } from '../../../../../core/models/user/user.model';
import { ResultModel } from '../../../../../core/models/result.model';
import { ModalService } from '../../../../../shared/services/modal/modal.service';
import { ModalNameEnum } from '../../../../../core/enums/modal-name.enum';
import { UserFormComponent } from '../components/user-form/user-form.component';
import { FormsModule } from '@angular/forms';
import { UserChangePasswordComponent } from '../components/user-chage-password/user-change-password.component';
import { NotificationService } from '../../../../../core/services/notification/notification.service';
import { LocalStorageService } from '../../../../../core/services/local-storage/local-storage.service';
import { ApiResponseHandlerService } from '../../../../../core/services/api-service/api-response-handler.service';

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [
    UserFormComponent,
    UserChangePasswordComponent,
    CommonModule, FormsModule,
    NgClass, NgFor, DatePipe
  ],
  templateUrl: './user.component.html',
  styleUrl: './user.component.css'
})
export class UserComponent implements OnInit {

  @Output() close = new EventEmitter<void>();

  users: User[] = [];
  searchByName: string = '';
  idUserLogin: string = '';

  constructor(
    private userService: UserService,
    private modalService: ModalService,
    private lsService: LocalStorageService,
    private apiResponseHandler: ApiResponseHandlerService,
    private notificationService: NotificationService
  ) {

  }

  ngOnInit(): void {
    this.idUserLogin = this.lsService.getUser().id;
    this.loadUsers();
  }

  loadUsers() {
    this.userService.getUsers({ allRows: true }).subscribe(
      (response: ResultModel<User[]> | null) => {
        if (response && response.data) {
          this.users = response.data;
        } else {
          console.log('No users found');
        }
      },
      (error) => {
        console.error('Error fetching users:', error);
      }
    );
  }

  closeComponent(): void {
    this.close.emit();
  }

  openFormUserModal(idUser: string): void {
    this.modalService.openModal(ModalNameEnum.UserForm, idUser);
  }

  deleteUser(name: string, idUser: string) {
    this.notificationService.showConfirm(`¿Estás seguro de que quieres desactivar a ${name}?'`).then((confirmed) => {
      if (confirmed) {
        this.userService.deleteUser({ idUser: idUser, idUserUpdated: this.idUserLogin }).subscribe({
          next: (response) => {
            if (response.success) {
              this.loadUsers();
              this.notificationService.showSuccess(`El usuario ${name} ha sido desactivado`);
            }
            else {
            this.apiResponseHandler.handleApiResponse(response.errorMessage);
            }
          },
          error: (error) => {
            this.apiResponseHandler.handleApiResponse(error);
          }
        });
      } else {
        console.log('Desactivación cancelada');
      }
    });
  }

  activeUser(name: string, idUser: string) {
    this.notificationService.showConfirm(`¿Estás seguro de que quieres activar a ${name}?'`).then((confirmed) => {
      if (confirmed) {
        this.userService.activeUser({ idUser: idUser, idUserUpdated: this.idUserLogin }).subscribe({
          next: (response) => {
            if (response.success) {
              this.loadUsers();
              this.notificationService.showSuccess(`El usuario ${name} ha sido activado`);
            }
            else {
            this.apiResponseHandler.handleApiResponse(response.errorMessage);
            }
          },
          error: (error) => {
            this.apiResponseHandler.handleApiResponse(error);
          }
        });
      } else {
        console.log('Activación cancelada');
      }
    });
  }


  changePassword(idUser: string) {
    this.modalService.openModal(ModalNameEnum.UserChangePasswordForm, idUser);
  }

  search() {
    if (this.searchByName && this.searchByName.length >= 3) {
      this.userService.getUsers({ allRows: true, fullName: this.searchByName }).subscribe(
        (response: ResultModel<User[]> | null) => {
          if (response && response.data) {
            this.users = response.data;
          } else {
            console.log('No users found');
          }
        },
        (error) => {
          console.error('Error fetching users:', error);
        }
      );
    }
    else if (this.searchByName == '') {
      this.loadUsers();
    }
  }
}
