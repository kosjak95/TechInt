import { HttpClient, HttpHeaders, HttpParams, HttpErrorResponse} from '@angular/common/http';
import { Injectable } from '@angular/core';

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

  tryCreate(data: { file_name: string }) {
    console.log(data);
    //return this.http.post(`${this.baseURL}/Route`, data);

    const body = JSON.stringify(data);
    const headerOptions = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post<boolean>(`${this.baseURL}/Cludge`, body, {
      headers: headerOptions
    });


  }

  updateFileContent(fileName: string, content: string) {

    return this.http.post(`${this.baseURL}/UpdateContentReqStruct`, { 'FileName': fileName, 'FileContent': content });
  }

}
