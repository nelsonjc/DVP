import { Injectable } from '@angular/core';
import { LocalStorageService } from '../local-storage/local-storage.service';
import { ApiService } from '../api-service/api.service';
import { environment } from '../../../../environments/environment';
import { UrlApis } from '../../enums/api-url.enum';
import { ResultFilterModel, ResultModel } from '../../models/result.model';
import { Task } from '../../models/task/task.model';
import { map, Observable, of, tap } from 'rxjs';
import { RoleEnum } from '../../enums/roles.enum';
import { TaskCreateRequest } from '../../models/task/task.create.request';
import { ApiResponseHandlerService } from '../api-service/api-response-handler.service';
import { TaskUpdateRequest } from '../../models/task/task.update.request';
import { TaskChageStatusRequest } from '../../models/task/task.change-status';
import { TaskFiltersRequest } from '../../models/task/task.filters.request';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  constructor(
    private lsService: LocalStorageService,
    private apiService: ApiService,
    private apiResponseHandler: ApiResponseHandlerService
  ) { }

  getTaskByUserRole(filters: TaskFiltersRequest): Observable<ResultFilterModel<Task[]> | null> {
    // let params: { key: string, value: any }[] = [
    //   { key: "pageNumber", value: pageNumber },
    //   { key: "rowsOfPage", value: rowsOfPage }
    // ];

    let user = this.lsService.getUser();

    // Verifica si hay un usuario
    if (user) {
      // Filtra según el rol del usuario
      switch (user.role) {
        case RoleEnum.Administrador:
          // Administrador no necesita agregar idUser
          break;
        case RoleEnum.Supervisor:
          // Supervisor no necesita agregar idUser
          break;
        case RoleEnum.Empleado:
        default:
          // Agrega idUser para empleados y casos por defecto
          // params.push({ key: "idUser", value: user.id });
          filters.idUser = user.id;
          break;
      }

      // Retorna la llamada a la API como un observable
      return this.apiService.getByParamsToUrl<ResultFilterModel<Task[]>>(
        filters,
        environment.apiBaseUrl,
        UrlApis.Task
      ).pipe(
        tap(response => {
          // Verifica si la respuesta fue exitosa
          if (response?.body?.data?.success) {
            console.log('Request successful');
          } else {
            console.error('Request failed');
          }
        }),
        map(response => response?.body ?? null)
      );
    } else {
      return of(null);
    }
  }

  getById(idTask: string): Observable<ResultModel<Task> | null> {
    if (idTask != null && idTask != '') {
      // Retorna la llamada a la API como un observable
      return this.apiService.getById<ResultModel<Task>>(idTask, environment.apiBaseUrl, UrlApis.Task)
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

  create(request: TaskCreateRequest): Observable<ResultModel<boolean>> {
    return this.apiService.post<ResultModel<boolean>>(environment.apiBaseUrl, UrlApis.Task, request)
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

  update(request: TaskUpdateRequest): Observable<ResultModel<boolean>> {
    return this.apiService.update<ResultModel<boolean>>(
      environment.apiBaseUrl,
      UrlApis.Task,
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

  changeStatus(request: TaskChageStatusRequest): Observable<ResultModel<boolean>> {
    return this.apiService.update<ResultModel<boolean>>(
      environment.apiBaseUrl,
      UrlApis.TaskChangeStatus,
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
}
