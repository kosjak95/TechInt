import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable()
export class FilesService {

  private baseURL = 'http://localhost:8080';
  constructor(private http: HttpClient) { }

  getFiles() {
    return this.http.get(`${this.baseURL}/Files/`);
  }
}
