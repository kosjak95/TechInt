import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable()
export class FilesService {

  private baseURL = 'http://localhost:8080';
  constructor(private http: HttpClient) { }

  getFiles() {
    return this.http.get(`${this.baseURL}/Files/`);
  }

  getFilesData() {
    return this.http.get(`${this.baseURL}/Json/`);
  }

  getFileContent(fileName: string) {
    return this.http.get(`${this.baseURL}/OpenFile/${fileName}`);
  }

  tryCreate(fileName: string) {
    return this.http.post(`${this.baseURL}/TryCreate`, fileName);
  }

  updateContent() { }
}

