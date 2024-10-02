import { Component, EventEmitter, Output } from '@angular/core';
import { RoleComponent } from '../components/role/role.component';
import { NgIf } from '@angular/common';
import { UserComponent } from '../components/user/container/user.component';

@Component({
  selector: 'app-security',
  standalone: true,
  imports: [UserComponent, RoleComponent, NgIf],
  templateUrl: './security.component.html',
  styleUrl: './security.component.css'
})
export default class SecurityComponent {

  showUser: boolean = true;
  showRole: boolean = false;

  openRole(): void {
    this.showRole = true;
    this.showUser = false; // Cerrar User si está abierto
  }

  openUser(): void {
    this.showUser = true;
    this.showRole = false; // Cerrar Role si está abierto
  }

  closeComponents(): void {
    this.showRole = false;
    this.showUser = false;
  }
}
