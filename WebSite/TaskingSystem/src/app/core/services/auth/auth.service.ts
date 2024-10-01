import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { User } from '../../models/user/user.model';
import { ApiService } from '../api-service/api.service';
import { environment } from '../../../../environments/environment';
import { UrlApis } from '../../enums/api-url.enum';
import { ResultModel } from '../../models/result.model';
import { LocalStorageService } from '../local-storage/local-storage.service';
import { RoleEnum } from '../../enums/roles.enum';

@Injectable({
  providedIn: 'root'
})
export class AuthService {


  constructor(
    private lsService: LocalStorageService,
    private apiService: ApiService,
    private router: Router) {
     }

  login(user: string, password: string): Observable<any> {
    return this.apiService.post<ResultModel<User>>(environment.apiBaseUrl, UrlApis.Auth, {"userName": user, "password": password })
    .pipe(
      tap(response => {
        if(response.body.success){
          console.log(response.body);
          this.lsService.setToken(response.body.data.token);
          this.lsService.setUser(response.body.data);
        }
      })
    );
  }

  isAuthenticated() : boolean {
    const token = this.lsService.getToken();
    if (!token) {
      return false;      
    }

    const payload = JSON.parse(atob(token.split('.')[1]));
    const exp = payload.exp * 1000;
    return Date.now() < exp;
  }

  userAuthenticated(): User | null {
    return this.lsService.getUser();
  }

  logout(): void {
    this.lsService.cleanUser();
    this.router.navigate(['/login']);
  }

  hasRoles(roles: string[]): boolean {
    let userRole = this.lsService.getUser().role;
    return roles.some(role => userRole.includes(role));
  }
}
