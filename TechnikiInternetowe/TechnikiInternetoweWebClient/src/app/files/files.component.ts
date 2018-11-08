import { Component, OnInit } from '@angular/core'
import { FilesService } from './files.service';
import { MatSnackBar, MatTableDataSource } from '@angular/material';
import { concat } from 'rxjs';
import { File } from './files.models';
import { formatDate } from '@angular/common';


@Component({
  selector: 'app-files',
  templateUrl: './files.component.html',
  styleUrls: ['./files.component.css']
})


export class FilesComponent implements OnInit {

  ngOnInit(): void {

    this.filesService.getFiles()
      .subscribe(
        (res: File[]) => {
          this.filesData = res;
          this.filesData.forEach(x => {
            x.LastUpdateTs = formatDate(x.LastUpdateTs.substring(6,19), 'dd-MM-yyyy hh:mm:ss a', 'en-US');})
          this.dataSource = this.filesData;
          console.log(this.filesData);
        },
        (error) => console.log(error));
  }

  title = 'Notes Editor';
  dataSource;
  filesData = [];

  displayedColumns: string[] = ['Id', 'name', 'lastUpdate', 'version', 'isEdited'];
  files = [];

  constructor(private filesService: FilesService, private matSnackBar: MatSnackBar) {}

  editFileOnClick(fileName: string) {

    var content;
    this.filesService.getFileContent(fileName)
      .subscribe(
        (res: string) => {
          content = res;
          console.log(content);
        },
      (error) => console.log(error));
  }
}
