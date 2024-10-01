import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Role } from '../../../../core/models/role/role.model';
import { NgFor } from '@angular/common';
import { UserService } from '../../../../core/services/user/user.service';
import { ResultModel } from '../../../../core/models/result.model';

@Component({
  selector: 'app-role',
  standalone: true,
  imports: [NgFor],
  templateUrl: './role.component.html',
  styleUrl: './role.component.css'
})
export class RoleComponent implements OnInit {

  roles: Role[] = [];

  constructor(
    private userService: UserService
  ){

  }

  @Output() close = new EventEmitter<void>();

  ngOnInit(): void {
    this.loadTasks();    
  }

 loadTasks() {
    this.userService.getRoles({}).subscribe(
      (response: ResultModel<Role[]> | null) => {
        if (response && response.data) {
          this.roles = response.data; 
        } else {
          console.log('No roles found');
        }
      },
      (error) => {
        console.error('Error fetching roles:', error);
      }
    );
  }

  closeComponent(): void {
    this.close.emit();
  }
}
