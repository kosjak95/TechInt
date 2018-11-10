import { Component, OnInit, Inject } from '@angular/core'
import { FilesService } from './files.service';
import { MatSnackBar, MatTableDataSource } from '@angular/material';
import { concat } from 'rxjs';
import { File } from './files.models';
import { formatDate } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material'



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

  displayedColumns: string[] = ['Id', 'name', 'lastUpdate', 'version', 'isEdited'];
  files = [];

  constructor(private filesService: FilesService, private matSnackBar: MatSnackBar, public dialog: MatDialog) {

  }

  onClick(file: any) {
    this.matSnackBar.open(file, null, {
      'duration': 2000
    })
  }

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
