import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { User } from '../../../../core/models/user/user.model';
import { NgClass, NgFor } from '@angular/common';
import { UserService } from '../../../../core/services/user/user.service';
import { ResultModel } from '../../../../core/models/result.model';

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [NgClass, NgFor],
  templateUrl: './user.component.html',
  styleUrl: './user.component.css'
})
export class UserComponent implements OnInit {

  @Output() close = new EventEmitter<void>();

  users: User[] = [];

  constructor(
    private userService: UserService
  ) {
    
  }


  ngOnInit(): void {
    this.loadUsers();
  }
  
  loadUsers() {
    this.userService.getUsers({}).subscribe(
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

  updateUser(userName: string) {
    console.log(`Actualizar usuario: ${userName}`);
  }

  deleteUser(userName: string) {
    console.log(`Eliminar usuario: ${userName}`);
  }

  changePassword(userName: string) {
    console.log(`Cambiar contraseña de: ${userName}`);
  }

}
