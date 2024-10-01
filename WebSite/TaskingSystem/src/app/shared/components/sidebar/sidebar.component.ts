import { Component, OnInit } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { LocalStorageService } from '../../../core/services/local-storage/local-storage.service';
import { Permission } from '../../../core/models/user/user.model';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, NgIf],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent implements OnInit{

  hasSecurity: boolean = false;
  permisions: Permission[] = [];

  constructor(private lsService: LocalStorageService){

  }

  ngOnInit(): void {
    this.permisions =  this.lsService.getUser().permissions;
    
    if(this.permisions.find(x => x.entity == "USER")){
      this.hasSecurity = true;
    }
  }
}
