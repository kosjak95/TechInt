import { Component } from '@angular/core'
import { FilesService } from './files.service';
import { MatSnackBar } from '@angular/material';


@Component({
  selector: 'app-files',
  templateUrl: './files.component.html',
  styleUrls: ['./files.component.css']
})

export class FilesComponent {
  title = 'lista plikow';

  files = [];

  constructor(private filesService: FilesService, private matSnackBar: MatSnackBar) {
    this.filesService.getFiles()
      .subscribe((res: any) => this.files = res);
  }

  onClick(file: any) {
    this.matSnackBar.open(file, null, {
      'duration':2000})
  }

}
