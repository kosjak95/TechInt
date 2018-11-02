import { Http } from '@angular/http';
import { Injectable } from '@angular/core';

@Injectable()
export class FilesService {

  private baseURL = 'http://localhost:8080';
  constructor(private http: Http) { }

  getFiles() {
    return this.http.get(`${this.baseURL}/Files/`);
  }
}
