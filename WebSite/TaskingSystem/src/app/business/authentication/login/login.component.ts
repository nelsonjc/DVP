import { Component } from '@angular/core';
import { AuthService } from '../../../core/services/auth/auth.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ApiResponseHandlerService } from '../../../core/services/api-service/api-response-handler.service';
import { LoadingComponent } from '../../../shared/components/loading/loading.component';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule, LoadingComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export default class LoginComponent {
  user: string = '';
  password: string = '';
  loading: boolean = false;

  constructor(
    private authService: AuthService, 
    private apiResponseHandler: ApiResponseHandlerService, 
    private router: Router
  ){

  }

  login(): void{
    this.loading = true;
    this.authService.login(this.user, this.password).subscribe({
      next: () => {
        this.loading = false;
        this.router.navigate(['/dashboard'])
      },
      error: (err) => {
        console.error('Login failed', err);
        this.apiResponseHandler.handleApiResponse(err);
        this.loading = false;
      }
    })
  }
}
