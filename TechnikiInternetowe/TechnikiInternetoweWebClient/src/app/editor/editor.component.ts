import { Component, OnInit, Inject } from '@angular/core'
import { MAT_DIALOG_DATA, MatDialog } from '@angular/material';


@Component({
  selector: 'app-editor',
  templateUrl: './editor.component.html'
})


export class EditorComponent implements OnInit {

  ngOnInit(): void {
    //TODO: I dont know why it is "object Object"
    console.log("editor:" + this.fileContent)
  }

  constructor(@Inject(MAT_DIALOG_DATA) public fileContent: string) { }

}
