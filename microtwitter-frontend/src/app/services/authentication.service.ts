import { HttpBackend, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserRegisterDTO } from '../models/UserRegisterDTO';
import { Observable } from 'rxjs';
import { LoginResponse } from '../models/LoginResponse';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  private apiUrl="https://localhost:7246/api/authentication"

  constructor(private http: HttpClient) { }

  register(dto: UserRegisterDTO): Observable<string>{
    return this.http.post(
      `${this.apiUrl}/register`, 
      dto, 
      { responseType: 'text' } 
    );
  }
  
  login(dto: any): Observable<LoginResponse>{
    return this.http.post<any>(`${this.apiUrl}/login`, dto);
  }
  saveUsername(username: string): void {
    localStorage.setItem('username', username);
    console.log('Saved username:', username);
  }

  getCurrentUsername(): string | null {
    return localStorage.getItem('username');
  }
  saveToken(token: string){
    localStorage.setItem('authToken', token);
  }
  getToken(): string | null{
    return localStorage.getItem('authToken');
  }
  clearUsername(): void {
  localStorage.removeItem('username');
}
  logout(){
    localStorage.removeItem('authToken');
    this.clearUsername();
  }
  isLoggedIn(): boolean{
    return this.getToken() !== null;
  }
}
