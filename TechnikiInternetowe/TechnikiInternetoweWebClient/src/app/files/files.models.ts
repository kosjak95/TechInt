export interface File {
  FileContent: string;
  lastUpdateTs: string;
  version: string;
  FileId: number;
  isEdited: boolean;
  name: string;
  editorName: string;
}

export interface FileContent {
  Name: string;
  FileContent1: string;
  IsEdited: boolean;
  FileId: number;
  EditorName: string;
}
