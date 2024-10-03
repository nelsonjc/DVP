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
import { UserCreateRequest } from '../../models/user/user-create.request';
import { UserUpdateRequest } from '../../models/user/user-update.request';
import { UserChangePasswordRequest } from '../../models/user/user-change-password.request';
import { UserDeleteRequest } from '../../models/user/user-delete.request';
import { UserActiveRequest } from '../../models/user/user-active.request';

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

  getById(idUser: string): Observable<ResultModel<User> | null> {
    if (idUser != null && idUser != '') {
      // Retorna la llamada a la API como un observable
      return this.apiService.getById<ResultModel<User>>(idUser, environment.apiBaseUrl, UrlApis.User)
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

  create(request: UserCreateRequest): Observable<ResultModel<boolean>> {
    return this.apiService.post<ResultModel<boolean>>(environment.apiBaseUrl, UrlApis.User, request)
      .pipe(
        tap(response => {
          // Verifica si la respuesta fue exitosa
          if (response?.body?.success) {
            console.log('Request successful');
          } else {
            console.log('Request successful');
          }
        }),
        map(response => response?.body ?? null) // Retorna el body o null si no existe
      );
  }

  update(request: UserUpdateRequest): Observable<ResultModel<boolean>> {
    return this.apiService.update<ResultModel<boolean>>(
      environment.apiBaseUrl,
      UrlApis.User,
      request.id,
      request
    )
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

  changePassword(request: UserChangePasswordRequest): Observable<ResultModel<boolean>> {
    return this.apiService.update<ResultModel<boolean>>(environment.apiBaseUrl, UrlApis.UserChangePassword, request.idUser, request)
      .pipe(
        tap(response => {
          // Verifica si la respuesta fue exitosa
          if (response?.body?.success) {
            console.log('Request successful');
          } else {
            console.log('Request successful');
          }
        }),
        map(response => response?.body ?? null) // Retorna el body o null si no existe
      );
  }

  deleteUser(request: UserDeleteRequest): Observable<ResultModel<boolean>> {
    return this.apiService.update<ResultModel<boolean>>(environment.apiBaseUrl, UrlApis.DeleteActive, request.idUser, request)
      .pipe(
        tap(response => {
          // Verifica si la respuesta fue exitosa
          if (response?.body?.success) {
            console.log('Request successful');
          } else {
            console.log('Request successful');
          }
        }),
        map(response => response?.body ?? null) // Retorna el body o null si no existe
      );
  }

  activeUser(request: UserActiveRequest): Observable<ResultModel<boolean>> {
    return this.apiService.update<ResultModel<boolean>>(environment.apiBaseUrl, UrlApis.UserActive, request.idUser, request)
      .pipe(
        tap(response => {
          // Verifica si la respuesta fue exitosa
          if (response?.body?.success) {
            console.log('Request successful');
          } else {
            console.log('Request successful');
          }
        }),
        map(response => response?.body ?? null) // Retorna el body o null si no existe
      );
  }
}
