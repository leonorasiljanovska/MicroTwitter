import { Injectable } from '@angular/core';
import { AuthenticationService } from '../services/authentication.service';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class AuthInterceptorService implements HttpInterceptor {
  
  constructor(private authService: AuthenticationService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>>{
    const token=this.authService.getToken();

    if(token){
      const cloned=request.clone({
        headers: request.headers.set("Authorization", "Bearer "+ token)
      });
      return next.handle(cloned);
    }
    return next.handle(request);
  }
}
