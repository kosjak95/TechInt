import { Component, OnInit, Inject } from '@angular/core'
import { MAT_DIALOG_DATA, MatDialog } from '@angular/material';
import { FilesService } from '../files/files.service';
import { concat } from 'rxjs';

@Component({
  selector: 'app-editor',
  templateUrl: './editor.component.html'
})


export class EditorComponent implements OnInit {

  ngOnInit(): void {
    this.disableModify = !this.data.isItEdit;
    this.fileTextArea = this.data.content;
    console.log("editor:" + this.data.content)
  }

  disableModify: boolean;
  fileTextArea: string;

  constructor(@Inject(MAT_DIALOG_DATA) public data: any, private filesService: FilesService) { }

  SaveFileOnClick() {
    console.log(this.fileTextArea);
    this.filesService.updateFileContent(this.data.fileName, this.fileTextArea)
      .subscribe(
      resp => {
        console.log(resp);
        },
        (error) => console.log(error));
  }
}
