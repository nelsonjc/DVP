import { Injectable } from '@angular/core';
import { User } from '../../models/user/user.model';

@Injectable({
  providedIn: 'root'
})
export class LocalStorageService {

  private tokenKey = 'authToken';
  private userKey = 'user';

  constructor() { }


  public setToken(token: string) : void {
    localStorage.setItem(this.tokenKey, token);
  }

  public setUser(user: User) : void {
    localStorage.setItem(this.userKey, JSON.stringify(user));
  }

  public getUser(): User | null {
    const user = localStorage.getItem(this.userKey);
    return user ? JSON.parse(user) : null;
  }

  public getToken() : string | null {
    if (typeof window !== 'undefined') {
      return localStorage.getItem(this.tokenKey);      
    }
    else{
      return null;
    }
  }

  public cleanUser() : void {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.userKey);
  }
}
