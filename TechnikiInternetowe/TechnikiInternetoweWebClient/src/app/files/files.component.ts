import { Component, OnInit, Inject } from '@angular/core'
import { FilesService } from './files.service';
import { File, FileContent } from './files.models';
import { formatDate, KeyValue, KeyValuePipe } from '@angular/common';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { EditorComponent } from '../editor/editor.component';
import { concat } from 'rxjs';


@Component({
  selector: 'app-files',
  templateUrl: './files.component.html',
  styleUrls: ['./files.component.css']
})


export class FilesComponent implements OnInit {

  ngOnInit(): void {

    this.InitFilesList();
  }

  title = 'Notes Editor';
  dataSource;
  filesData = [];
  fileName: string;

  displayedColumns: string[] = ['Id', 'name', 'lastUpdate', 'version', 'isEdited'];
  files = [];


  constructor(private filesService: FilesService, private dialog: MatDialog) { }

  InitFilesList(): void {
    this.files = [];
    this.filesService.getFiles()
      .subscribe(
        (res: File[]) => {
          this.filesData = res;
          this.filesData.forEach(x => {
            x.LastUpdateTs = formatDate(x.LastUpdateTs.substring(6, 19), 'dd-MM-yyyy hh:mm:ss a', 'en-US');
          })
          this.dataSource = this.filesData;
        },
        (error) => console.log(error));
  }

  OnNewItemClick() {
    {
      const dialogRef = this.dialog.open(DialogOverviewExampleDialog, {
        width: '250px',
        data: { name: "test", vesion: "next" }
      });

      dialogRef.afterClosed().subscribe(result => {
        this.fileName = result;

        this.filesService.tryCreate({ file_name: this.fileName })
          .subscribe(
            resp => {
              this.InitFilesList();
            },
            (error) => {
              this.InitFilesList();
            });
      });
    }
  }

  editFileOnClick(fileName: string) {

    this.filesService.getFileContent(fileName)
      .subscribe(
        (res: FileContent) => {
          console.log(res);

          let dialogRef = this.dialog.open(EditorComponent, {
            height: '450px',
            width: '700px',
            data: res,
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
    @Inject(MAT_DIALOG_DATA) public data: string) { }

  public name: string;

  onNoClick(): void {
    this.dialogRef.close();
  }
}
