import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Post } from '../models/post';
import { AuthenticationService } from './authentication.service';
@Injectable({
  providedIn: 'root'
})
export class PostService {

  private apiUrl="https://localhost:7246/api/posts"

  constructor(private http: HttpClient, private authService: AuthenticationService){}


  private getAuthHeaders(): HttpHeaders{
    const token=this.authService.getToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }
  getAllPosts() : Observable<Post[]>{
    return this.http.get<Post[]>(this.apiUrl,{ headers: this.getAuthHeaders()});
  }
  createPost(post:Partial<Post>): Observable<Post>{
    return this.http.post<Post>(this.apiUrl, post,{ headers: this.getAuthHeaders() } );
}
  deletePost(id:string): Observable<void>{
    return this.http.delete<void>(`${this.apiUrl}/${id}`, { headers: this.getAuthHeaders() });
}
getMyPosts(): Observable<Post[]> {
  return this.http.get<Post[]>(`${this.apiUrl}/my-posts`);
}

}