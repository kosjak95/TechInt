import { Component, OnInit, Inject } from '@angular/core'
import { MAT_DIALOG_DATA, MatDialog } from '@angular/material';
import { FilesService } from '../files/files.service';
import { concat } from 'rxjs';
import { FileContent } from '../files/files.models';

@Component({
  selector: 'app-editor',
  templateUrl: './editor.component.html'
})


export class EditorComponent implements OnInit {

  ngOnInit(): void {
    this.name = this.data.Name;
    this.disableModify = this.data.IsEdited;
    this.fileTextArea = this.data.FileContent1;
  }

  name: string;
  disableModify: boolean;
  fileTextArea: string;

  constructor(@Inject(MAT_DIALOG_DATA) public data: FileContent, private filesService: FilesService) { }

  SaveFileOnClick() {
    console.log(this.data);
    this.filesService.updateFileContent(this.data.Name, this.fileTextArea)
      .subscribe(
      resp => {
        console.log(resp);
        },
        (error) => console.log(error));
  }
}
