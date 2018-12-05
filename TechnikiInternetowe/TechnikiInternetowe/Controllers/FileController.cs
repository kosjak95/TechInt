using System;
using System.IO;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TechnikiInternetowe.DBEntity;
using System.Linq;
using Newtonsoft.Json;
using System.Data.Entity;
using TechnikiInternetowe.WebSockets;
using TechnikiInterentoweCommon;
using System.Collections.Generic;

namespace TechnikiInternetowe.Controllers
{
    public class FileController : Controller
    {
        #region public methods

        /// <summary>
        /// Return Json formated string with Names of each file on server
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //[Route("Files")]
        public static string GetListOfFilesOnServer()
        {
            DB_TechIntEntities db = new DB_TechIntEntities();
            var dane = db.Files.Select(s => new { s.FileId, s.LastUpdateTs, s.Name, s.Version, s.IsEdited, s.EditorName });

            List<FileData> baseData = new JavaScriptSerializer().Deserialize<List<FileData>>(
                                          new JavaScriptSerializer().Serialize(dane));
            List<FullFileData> fullFileDatas = new List<FullFileData>();
            foreach(FileData fileData in baseData)
            {
                FullFileData tmp = new FullFileData();
                tmp.setAll(fileData, OnlyGetFileContent(AppDomain.CurrentDomain.BaseDirectory, fileData.Name));
                fullFileDatas.Add(tmp);
            }
            return new JavaScriptSerializer().Serialize(fullFileDatas);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="project_path"></param>
        /// <param name="file_name"></param>
        /// <returns></returns>
        /// [HttpPost]
        /// [Route("SynchronizeAfterConnectionEstablished")]
        internal static bool SynchronizeAfterConnectionEstablished(string project_path, 
                                                                   string synchCodedMsg)
        {
            SynchronizeAfterConnectionEstablishedMsg synchMsg = 
                new JavaScriptSerializer().Deserialize<SynchronizeAfterConnectionEstablishedMsg>(synchCodedMsg);
            DB_TechIntEntities db = new DB_TechIntEntities();
            List<string> failedSyncFileNames = new List<string>();
            foreach (FullFileData fileData in synchMsg.filesList)
            {
                var queryFile = db.Files.Where(w => w.Name == fileData.Name);
                Files dbFile = null;
                if (queryFile.Any())
                    dbFile = queryFile.First();

                if (dbFile == null)
                {
                    if (PermissionOnCreateFile(project_path, fileData.Name))
                    {
                        UpdateFileContent(project_path, fileData.Name, fileData.FileContent);
                    }
                    else
                    {
                        failedSyncFileNames.Add(fileData.Name);
                    }
                }
                else
                {
                    if(fileData.Version >  int.Parse(dbFile.Version) && !dbFile.IsEdited)
                    {
                        UpdateFileContent(project_path, fileData.Name, fileData.FileContent);
                    }
                }
            }
            SendFailedSyncFilesMsg(synchMsg.sender, failedSyncFileNames);
            SendUpdateFileListToAll();
            return true;
        }

        /// <summary>
        /// Return contend of given by argument file
        /// </summary>
        /// <returns></returns>
        //[HttpPost]
        //[Route("OpenFile")]
        public static string GetFileContent(string project_path, string fileName, string editorName)
        {
            string content = "";
            Files file = null;
            DB_TechIntEntities db = new DB_TechIntEntities();
            DBEntity.FileContent file_content = null;
            try
            {
                file = db.Files.Where(w => w.Name == fileName).First();
                using (StreamReader sr = new StreamReader(project_path + @file.FileSrc + file.Name + ".txt"))
                {
                    content = sr.ReadToEnd();
                }
                file_content = new DBEntity.FileContent() { FileId = file.FileId,
                                                            Name = file.Name,
                                                            IsEdited = file.IsEdited,
                                                            FileContent1 = content,
                                                            EditorName = editorName };

                if (file.IsEdited == false)
                {
                    file.IsEdited = true;
                    file.EditorName = editorName;
                    db.Entry(file).State = EntityState.Modified;
                    db.SaveChanges();
                }
                SendUpdateFileListToAll();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Write(e.Message);
                content = "We have some problem with open this file";
            }

            return JsonConvert.SerializeObject(file_content);
        }

        /// <summary>
        /// Function only return file content, no locking, no other data
        /// </summary>
        /// <param name="project_path"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        internal static string OnlyGetFileContent(string project_path, string fileName)
        {
            DB_TechIntEntities db = new DB_TechIntEntities();
            try
            {
                Files file = db.Files.Where(w => w.Name == fileName).First();
                using (StreamReader sr = new StreamReader(project_path + @file.FileSrc + file.Name + ".txt"))
                {
                    return sr.ReadToEnd();
                }
            } catch (Exception e)
            {
                System.Diagnostics.Debug.Write(e.Message);
                return null;
            }
        }

        private static void SendUpdateFileListToAll()
        {
            ServerWebSocket serverSocket = ServerWebSocket.Instance;
            serverSocket.SendToAll(new Message()
            {
                Key = MsgType.REFRESH_FILES_LIST_MSG,
                Value = "Update"
            });
        }

        private static void SendFailedSyncFilesMsg(string receiver, List<string> filesNameslist)
        {
            if(filesNameslist.Count.Equals(0))
            {
                return;
            }
            ServerWebSocket serverSocket = ServerWebSocket.Instance;
            serverSocket.SendToOne(receiver, filesNameslist);
        }

        /// <summary>
        /// Fucntion gives information about possibility of creation new note
        /// if it is possible insert row to Db, and lock file on edit automaticly
        /// </summary>
        /// <param name="newFile">Inserted by user name of file</param>
        /// <returns>true if creation is possible and done, other way false</returns>
        //[HttpPost]
        //[Route("TryCreate")]
        public static bool PermissionOnCreateFile(string project_path, string file_name)
        {
            if (IsCreationOfFilePossible(file_name))
                if (InsertDataToDb(project_path, file_name))
                    return CreateFile(project_path, file_name);

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="file_data"></param>
        /// <returns></returns>
        //[HttpPost]
        //[Route("UpdateContent")]
        public static bool UpdateFileContent(string project_path, string file_name, string file_data)
        {
            if (file_name == null || file_data == null)
                return false;
            DB_TechIntEntities db = new DB_TechIntEntities();
            Files file = db.Files.Where(w => w.Name == file_name).Single();
            if (file == null)
                return false;

            string file_path = project_path + file.FileSrc + file.Name + ".txt";

            try
            {
                System.IO.File.WriteAllText(file_path, file_data);
            }
            catch (IOException e)
            {
                System.Diagnostics.Debug.Write(e.Message);
                return false;
            }

            UpdateDataAtDb(file);
            SendUpdateFileListToAll();

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// //[HttpOptions]
        //[Route("ReleaseFileCludge")]
        public static bool ReleaseFile(string fileName)
        {
            try
            {
                DB_TechIntEntities db = new DB_TechIntEntities();
                Files file = db.Files.Where(w => w.Name == fileName).Single();
                if (file == null)
                    return false;
                file.IsEdited = false;
                file.EditorName = "";
                db.Entry(file).State = EntityState.Modified;
                db.SaveChanges();
                SendUpdateFileListToAll();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Write(e.Message);
                return false;
            }

            return true;
        }

        #endregion

        #region private methods

        //check if file with this name not exist in db
        private static bool IsCreationOfFilePossible(string file_name)
        {
            DB_TechIntEntities db = new DB_TechIntEntities();
            Files file = null;
            try
            {
                file = db.Files.Where(w => w.Name == file_name).Single();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Write(e.Message);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Create File on serwer
        /// </summary>
        /// <param name="project_path"></param>
        /// <param name="file_name"></param>
        /// <returns></returns>
        private static bool CreateFile(string project_path, string file_name)
        {
            string path = project_path + @"App_Data\Files\" + file_name + ".txt";
            try
            {
                System.IO.File.Create(path).Dispose();
                SendUpdateFileListToAll();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Write(e.Message);
                return false;
            }
            return true;
        }

        //try insert given file into db
        private static bool InsertDataToDb(string project_path, string file_name)
        {
            try
            {
                DB_TechIntEntities db = new DB_TechIntEntities();
                Files newFile = new Files
                {
                    Name = file_name,
                    CreatedTs = DateTime.Now,
                    LastUpdateTs = DateTime.Now,
                    FileSrc = @"App_Data\Files\",
                    Version = "0",
                    IsEdited = false,
                    EditorName = ""
                };
                db.Files.Add(newFile);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Write(e.Message);
                return false;
            }

            return true;
        }

        private static bool UpdateDataAtDb(Files file)
        {
            try
            {
                DB_TechIntEntities db = new DB_TechIntEntities();
                file.LastUpdateTs = DateTime.Now;
                file.Version = (int.Parse(file.Version) + 1).ToString();
                file.IsEdited = false;
                file.EditorName = "";
                db.Entry(file).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Write(e.Message);
                return false;
            }

            return true;
        }

        #endregion
    }

}