import { Component, OnInit } from '@angular/core';
import { User } from '../../../core/models/user/user.model';
import { HomeStatisticDashboard } from '../../../core/models/home/home-statistic-dashboard.model';
import { UserTaskTableComponent } from '../components/user-task-table/user-task-table.component';
import { LocalStorageService } from '../../../core/services/local-storage/local-storage.service';
import { TaskFormComponent } from '../../task/components/task-form/task-form.component';
import { TaskService } from '../../../core/services/task/task.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    TaskFormComponent,
    UserTaskTableComponent
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})

export default class DashboardComponent implements OnInit {

  isModalOpen: boolean = false;
  userAuth: User;
  statistic: HomeStatisticDashboard;

  constructor(
    private lsService : LocalStorageService,
    private taskService: TaskService
  ){
  }

  ngOnInit(): void {
    this.userAuth = this.lsService.getUser();

    if (this.userAuth) {
      this.statistic = { 
        taskPending: this.userAuth.taskPending, 
        taskInProcess: this.userAuth.taskInProcess, 
        taskCompleted: this.userAuth.taskCompleted,
        tasksDue: this.userAuth.taskDue
      };      
    }
  }
}
