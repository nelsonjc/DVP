import { Component, EventEmitter, Output } from '@angular/core';
import { UserComponent } from '../components/user/user.component';
import { RoleComponent } from '../components/role/role.component';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-security',
  standalone: true,
  imports: [UserComponent, RoleComponent, NgIf],
  templateUrl: './security.component.html',
  styleUrl: './security.component.css'
})
export default class SecurityComponent {

  showRole: boolean = false;
  showUser: boolean = true;

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
