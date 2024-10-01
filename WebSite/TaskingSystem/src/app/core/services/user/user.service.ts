import { Injectable } from '@angular/core';
import { map, Observable, of, tap } from 'rxjs';
import { ResultModel } from '../../models/result.model';
import { User } from '../../models/user/user.model';
import { ApiService } from '../api-service/api.service';
import { RoleEnum } from '../../enums/roles.enum';
import { environment } from '../../../../environments/environment';
import { UrlApis } from '../../enums/api-url.enum';
import { RoleFilterRequest } from '../../models/role/role-filter.request';
import { Role } from '../../models/role/role.model';
import { UserFilterRequest } from '../../models/user/user-filter.request';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private apiService: ApiService) { }

  getByRolName(rolName: RoleEnum): Observable<ResultModel<User[]> | null> {
    if (rolName != null) {
      // Retorna la llamada a la API como un observable
      return this.apiService.getById<ResultModel<User[]>>(rolName, environment.apiBaseUrl, UrlApis.UserByRolName)
        .pipe(
          tap(response => {
            // Verifica si la respuesta fue exitosa
            if (response?.body?.success) {
              console.log('Request successful');
            } else {
              console.error('Request failed');
            }
          }),
          map(response => response?.body ?? null) // Retorna el body o null si no existe
        );
    }
    else {
      return of(null);
    }
  }

  getRoles(request: RoleFilterRequest): Observable<ResultModel<Role[]> | null> {

    return this.apiService.getByParamsToUrl<ResultModel<Role[]>>(request, environment.apiBaseUrl, UrlApis.RoleGetAll)
      .pipe(
        tap(response => {
          // Verifica si la respuesta fue exitosa
          if (response?.body?.success) {
            console.log('Request successful');
          } else {
            console.error('Request failed');
          }
        }),
        map(response => response?.body ?? null) 
      );
  }

  getUsers(request: UserFilterRequest): Observable<ResultModel<User[]> | null> {
    return this.apiService.getByParamsToUrl<ResultModel<User[]>>(request, environment.apiBaseUrl, UrlApis.User)
    .pipe(
      tap(response => {
        // Verifica si la respuesta fue exitosa
        if (response?.body?.success) {
          console.log('Request successful');
        } else {
          console.error('Request failed');
        }
      }),
      map(response => response?.body ?? null) 
    );
  }

  // createUser(user: User): Observable<User> {
  //   return this.http.post<User>(this.apiUrl, user);
  // }

  // updateUser(id: string, user: User): Observable<User> {
  //   return this.http.put<User>(`${this.apiUrl}/${id}`, user);
  // }

  // deleteUser(id: string): Observable<void> {
  //   return this.http.delete<void>(`${this.apiUrl}/${id}`);
  // }

  // changePassword(id: string, newPassword: string): Observable<void> {
  //   return this.http.put<void>(`${this.apiUrl}/${id}/change-password`, { newPassword });
  // }
}
