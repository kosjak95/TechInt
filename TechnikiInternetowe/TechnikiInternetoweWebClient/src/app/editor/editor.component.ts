import { Component, OnInit, Inject, OnDestroy } from '@angular/core'
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material';
import { FilesService } from '../files/files.service';
import { FileContent } from '../files/files.models';

@Component({
  selector: 'app-editor',
  templateUrl: './editor.component.html'
})


export class EditorComponent implements OnInit{

  ngOnInit(): void {
    this.name = this.data.Name;
    this.disableModify = this.data.IsEdited;
    this.fileTextArea = this.data.FileContent1;
  }

  name: string;
  disableModify: boolean;
  fileTextArea: string;
  public dialogRef: MatDialogRef<EditorComponent,any>;

  constructor(@Inject(MAT_DIALOG_DATA) public data: FileContent,
              private filesService: FilesService) { }

  SaveFileOnClick() {
    this.filesService.updateFileContent(this.data.Name, this.fileTextArea)
      .subscribe(
      resp => {
        this.dialogRef.close();
        },
      (error) => console.log(error));
  }

  ngOnDestroy(): void {
    console.log("window closed");

    if (!this.disableModify)
      this.releaseFile(name)
  }

  releaseFile(fileName: string) {
    this.filesService.releaseFile(fileName)
      .subscribe(
        (res: boolean) => { },
        (error) => console.log(error));
  }
}
