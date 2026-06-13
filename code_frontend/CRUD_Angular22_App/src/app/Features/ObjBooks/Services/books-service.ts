import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment.development';
import { Observable } from 'rxjs';
import { GetBookResponse } from '../../../Shared/Interfaces/get-book-response';
import { CreateBookRequest } from '../../../Shared/Interfaces/create-book-request';
import { UpdateBookRequest } from '../../../Shared/Interfaces/update-book-request';

@Injectable({
  providedIn: 'root',
})
export class BooksService {
  private http = inject(HttpClient)
  private endPoint = `${environment.apiUrl}/books`

  getAll():Observable<GetBookResponse[]>{
    return this.http.get<GetBookResponse[]>(this.endPoint);
  }

  getById(id:number):Observable<GetBookResponse>{
    return this.http.get<GetBookResponse>(`${this.endPoint}/${id}`);
  }

  create(request:CreateBookRequest):Observable<void>{
    return this.http.post<void>(this.endPoint,request);
  }

  update(request:UpdateBookRequest):Observable<void>{
    return this.http.put<void>(this.endPoint,request);
  }

  delete(id:number):Observable<void>{
    return this.http.delete<void>(`${this.endPoint}/${id}`);
  }
}
