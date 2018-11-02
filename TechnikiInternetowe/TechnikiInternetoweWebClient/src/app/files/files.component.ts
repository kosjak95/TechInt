import { Component } from '@angular/core'
import { FilesService } from './files.service';


@Component({
  selector: 'app-files',
  template: '<h2>{{title}}</h2>',
  styleUrls: ['./files.component.css']
})

export class FilesComponent {
  title = 'lista plikow';

  files = [];

  constructor(private filesService: FilesService) {
    this.filesService.getFiles()
      .subscribe((res: any) => this.files = res.body);
  }

}
