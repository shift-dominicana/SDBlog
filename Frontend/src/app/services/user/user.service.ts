import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Category } from 'src/app/models/Category';
import { User } from 'src/app/models/User';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private appUrl = environment.baseUrl
  private apiUrl = 'api/User'
  
  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  }
  

  selectedCategory: User = new User(); 

  constructor(private http: HttpClient) { }

  getUsers(): Observable<any>{
    return this.http.get(this.appUrl+this.apiUrl)
  }

  update(data: any, id: any): Observable<any> {
    return this.http.put(this.appUrl+this.apiUrl+"/"+id, data);
  }

  create(data: any): Observable<any> {
    return this.http.post(this.appUrl+this.apiUrl, data, this.httpOptions);
  }

  delete(id: any): Observable<any> {
    return  this.http.delete(this.appUrl+this.apiUrl+'/'+id);
  }
}
