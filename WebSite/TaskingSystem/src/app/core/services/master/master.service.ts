import { Injectable } from '@angular/core';
import { map, Observable, of, tap } from 'rxjs';
import { ResultModel } from '../../models/result.model';
import { Status } from '../../models/status/status.model';
import { ApiService } from '../api-service/api.service';
import { environment } from '../../../../environments/environment';
import { UrlApis } from '../../enums/api-url.enum';

@Injectable({
  providedIn: 'root'
})
export class MasterService {

  constructor(private apiService: ApiService) { }

  
  getByEntityName(entityName: string): Observable<ResultModel<Status[]> | null> {
    if (entityName != null && entityName != '') {

      let params: { key: string, value: any }[] = [
        { key: "Entity", value: entityName },
        { key: "Active", value: true },
        { key: "All", value: false }
      ];

      // Retorna la llamada a la API como un observable
      return this.apiService.getByParams<ResultModel<Status[]>>(params, environment.apiBaseUrl, UrlApis.MasterStatus)
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
}
