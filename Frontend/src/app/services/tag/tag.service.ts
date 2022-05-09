import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Tag } from 'src/app/models/Tag';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TagService {
  private appUrl = environment.baseUrl
  private apiUrl = 'api/Tag'
  
  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  }
  
  selectedTag: Tag = new Tag();
  constructor(private http: HttpClient) { }

  getTags(): Observable<any>{
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
