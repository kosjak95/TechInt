import { Component, OnInit } from '@angular/core'
import { FilesService } from './files.service';
import { MatSnackBar, MatTableDataSource, MatDialog } from '@angular/material';
import { concat } from 'rxjs';
import { File } from './files.models';
import { formatDate } from '@angular/common';
import { EditorComponent } from '../editor/editor.component'


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

  displayedColumns: string[] = ['Id', 'name', 'lastUpdate', 'version', 'isEdited', 'open'];
  files = [];

  constructor(private filesService: FilesService, private matSnackBar: MatSnackBar, private dialog: MatDialog) {}

  editFileOnClick(fileName: string) {

    var content;
    this.filesService.getFileContent(fileName)
      .subscribe(
        (res: string) => {
          content = res;
          console.log(content);

          let dialogRef = this.dialog.open(EditorComponent, {
            height: '400px',
            width: '600px',
            data: {
              dataKey: content
            },
          });
        },
      (error) => console.log(error));
  }

  openFileOnClick(fileName: string) {

    var content;
    this.filesService.getFileContent(fileName)
      .subscribe(
        (res: string) => {
          content = res;
          console.log(content);

          let dialogRef = this.dialog.open(EditorComponent, {
            height: '400px',
            width: '600px',
            data: {
              dataKey: content
            },
          });
        },
        (error) => console.log(error));
  }
}
