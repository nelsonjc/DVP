import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../core/services/auth/auth.service';
import { Router } from '@angular/router';
import { LocalStorageService } from '../../../core/services/local-storage/local-storage.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})

export class HeaderComponent implements OnInit {

  nameUser: string = '';

  constructor(private authService: AuthService, private lsService: LocalStorageService){

  }
  ngOnInit(): void {
    this.nameUser = this.lsService.getUser().fullName;
  }

  logout(): void{
    this.authService.logout();
  }
}
