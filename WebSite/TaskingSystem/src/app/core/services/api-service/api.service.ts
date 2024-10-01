import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { AuthService } from '../auth/auth.service';
import { UrlApis } from '../../enums/api-url.enum';
import { catchError, Observable, throwError } from 'rxjs';
import { LocalStorageService } from '../local-storage/local-storage.service';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(
    private httpClient: HttpClient,
    private lsService: LocalStorageService

  ) { }

  getAll<Entity>(baseUrl: string, urlApi: UrlApis): Observable<HttpResponse<Entity>> {
    return this.httpClient.get<Entity>(`${baseUrl}${urlApi}`, { 
      observe: 'response', 
      headers: { 'Authorization': `Bearer ${this.lsService.getToken()}` } 
    });
  }

  getById<Entity>(id: string, baseUrl: string ,urlApi: UrlApis): Observable<HttpResponse<Entity>> {
    return this.httpClient.get<Entity>(`${baseUrl}${urlApi}/${id}`, { 
      observe: 'response', 
      headers: { 'Authorization': `Bearer ${this.lsService.getToken()}` } 
    });
  }

  getByRouteParams<Entity>(params: any[], baseUrl: string ,urlApi: UrlApis): Observable<HttpResponse<Entity>> {
    let url: string = `${baseUrl}${urlApi}`;
    params.map(p => url = `${url}/${p}`);
    return this.httpClient.get<Entity>(url, { 
      observe: 'response', 
      headers: { 'Authorization': `Bearer ${this.lsService.getToken()}` } 
    });
  }

  getByParams<Entity>(params: { key: string, value: any }[], baseUrl: string ,urlApi: UrlApis): Observable<HttpResponse<Entity>> {
    let url: string = `${baseUrl}${urlApi}`;
    params.map((p, i) => i === 0 ? url = `${url}?${p.key}=${p.value}` : url = `${url}&${p.key}=${p.value}`);
    return this.httpClient.get<Entity>(url, { 
      observe: 'response', 
      headers: { 'Authorization': `Bearer ${this.lsService.getToken()}` } 
    });
  }

  getByParamsToUrl<Entity>(objParam: any, baseUrl: string ,urlApi: UrlApis): Observable<HttpResponse<Entity>> {
    let url: string = `${baseUrl}${urlApi}`;
    let params = this.convertToParams(objParam);
    params.map((p, i) => i === 0 ? url = `${url}?${p.key}=${p.value}` : url = `${url}&${p.key}=${p.value}`);
    return this.httpClient.get<Entity>(url, { 
      observe: 'response', 
      headers: { 'Authorization': `Bearer ${this.lsService.getToken()}` } 
    });
  }

  getByUrl<Entity>(url: string): Observable<HttpResponse<Entity>> {
    return this.httpClient.get<Entity>(url, { 
      observe: 'response', 
      headers: { 'Authorization': `Bearer ${this.lsService.getToken()}` } 
    });
  }

  post<Entity>(baseUrl: string ,urlApi: UrlApis, body: any): Observable<HttpResponse<Entity>> {
    return this.httpClient.post<Entity>(`${baseUrl}${urlApi}`, body, { 
      observe: 'response', 
      headers: { 'Authorization': `Bearer ${this.lsService.getToken()}` } 
    });
  }

  update<Entity>(baseUrl: string ,urlApi: UrlApis, id: string, body: any): Observable<HttpResponse<Entity>> {
    return this.httpClient.put<Entity>(`${baseUrl}${urlApi}/${id}`, body, { 
      observe: 'response', 
      headers: { 'Authorization': `Bearer ${this.lsService.getToken()}` } 
    });
  }

  private convertToParams<T>(obj: T): { key: string; value: any }[] {
    return Object.entries(obj).map(([key, value]) => ({
        key,
        value,
    }));
  }
}
