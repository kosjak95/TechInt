import { Component, OnInit, Inject } from '@angular/core'
import { MAT_DIALOG_DATA, MatDialog } from '@angular/material';

@Component({
  selector: 'app-editor',
  templateUrl: './editor.component.html'
})


export class EditorComponent implements OnInit {

  ngOnInit(): void {
    this.disableModify = !this.data.isItEdit;
    console.log("editor:" + this.data.content)
  }

  disableModify: boolean;

  constructor(@Inject(MAT_DIALOG_DATA) public data: any) { }

  SaveFileOnClick() {
    //TODO: handle save file
  }
}
