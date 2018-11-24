export interface File {
  name: string;
  FileId: number;
  version: string;
  lastUpdateTs: string;
  isEdited: boolean;
  editorName: string;
}


export interface FileContent {
  Name: string;
  FileContent1: string;
  IsEdited: boolean;
  FileId: number;
  EditorName: string;
}
