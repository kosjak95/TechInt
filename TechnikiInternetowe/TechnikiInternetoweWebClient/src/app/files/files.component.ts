import { Component, OnInit, Inject } from '@angular/core'
import { FilesService } from './files.service';
import { File } from './files.models';
import { formatDate } from '@angular/common';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { EditorComponent } from '../editor/editor.component';


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
            x.LastUpdateTs = formatDate(x.LastUpdateTs.substring(6, 19), 'dd-MM-yyyy hh:mm:ss a', 'en-US');
          })
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


  constructor(private filesService: FilesService, private dialog: MatDialog) { }

  OnNewItemClick() {
    {
      const dialogRef = this.dialog.open(DialogOverviewExampleDialog, {
        width: '250px',
        data: { name: "test", vesion: "next" }
      });

      dialogRef.afterClosed().subscribe(result => {
        console.log('The dialog was closed');
      });
    }
  }

  openFileOnClick(fileName: string) {

    this.filesService.getFileContent(fileName)
      .subscribe(
        (res: string) => {
          var content = res;
          console.log(content);

          let dialogRef = this.dialog.open(EditorComponent, {
            height: '400px',
            width: '600px',
            data: {
              content: content
            },
          });
        },
        (error) => console.log(error));
  }

  editFileOnClick(fileName: string) {

    this.filesService.getFileContent(fileName)
      .subscribe(
        (res: string) => {
          var content = res;
          console.log(content);

          let dialogRef = this.dialog.open(EditorComponent, {
            height: '400px',
            width: '600px',
            data: {
              content: content
            },
          });
        },
        (error) => console.log(error));
  }
}

@Component({
  selector: 'dialog-overview-example-dialog',
  templateUrl: 'add-new-partial.html',
})
export class DialogOverviewExampleDialog {

  constructor(
    public dialogRef: MatDialogRef<DialogOverviewExampleDialog>,
    @Inject(MAT_DIALOG_DATA) public data: File) { }

  onNoClick(): void {
    this.dialogRef.close();
  }
}
