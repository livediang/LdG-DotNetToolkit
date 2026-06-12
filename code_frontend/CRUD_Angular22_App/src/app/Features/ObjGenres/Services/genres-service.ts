import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment.development';
import { Observable } from 'rxjs';
import { GetGenreResponse } from '../../../Shared/Interfaces/get-genre-response';

@Injectable({
  providedIn: 'root',
})
export class GenresService {
  private http = inject(HttpClient)
  private endPoint = `${environment}/books`

  getAll():Observable<GetGenreResponse[]>{
    return this.http.get<GetGenreResponse[]>(this.endPoint);
  }
}
