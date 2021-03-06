import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MaterialModule } from './angular-material.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { FilesComponent, DialogOverviewExampleDialog } from './files/files.component';
import { FilesService } from './files/files.service';
import { HttpClientModule } from '@angular/common/http';
import { EditorComponent } from './editor/editor.component'

@NgModule({
  declarations: [
    AppComponent,
    FilesComponent,
    DialogOverviewExampleDialog,       
    EditorComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    MaterialModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    FormsModule,
    HttpClientModule
  ],
  providers: [FilesService],
  bootstrap: [AppComponent],
    entryComponents: [
        DialogOverviewExampleDialog,
        EditorComponent
    ]

})
export class AppModule { }
