using System;
using System.IO;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TechnikiInternetowe.DBEntity;
using System.Linq;
using Newtonsoft.Json;
using System.Data.Entity;
using TechnikiInternetowe.WebSockets;

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
            var dane = db.Files.Select(s => new { s.FileId, s.LastUpdateTs, s.Name, s.Version, s.IsEdited });

            return new JavaScriptSerializer().Serialize(dane);
        }

        /// <summary>
        /// Return contend of given by argument file
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //[Route("OpenFile/{fileName}")]
        public static string GetFileContent(string project_path, string fileName)
        {
            string content = "";
            Files file = null;
            DB_TechIntEntities db = new DB_TechIntEntities();
            FileContent file_content = null;
            try
            {
                file = db.Files.Where(w => w.Name == fileName).First();
                using (StreamReader sr = new StreamReader(project_path + @file.FileSrc + file.Name + ".txt"))
                {
                    content = sr.ReadToEnd();
                }
                file_content = new FileContent() { FileId = file.FileId, Name = file.Name, IsEdited = file.IsEdited, FileContent1 = content };
                file.IsEdited = true;
                db.Entry(file).State = EntityState.Modified;
                db.SaveChanges();

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Write(e.Message);
                content = "We have some problem with open this file";
            }

            return JsonConvert.SerializeObject(file_content);
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
            if (isCreationOfFilePossible(file_name))
                if (insertDataToDb(project_path, file_name))
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

            ServerWebSocket serverSocket = ServerWebSocket.Instance;
            serverSocket.sendToAll("1");

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

        #region private methods

        //check if file with this name not exist in db
        private static bool isCreationOfFilePossible(string file_name)
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
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Write(e.Message);
                return false;
            }
            return true;
        }

        //try insert given file into db
        private static bool insertDataToDb(string project_path, string file_name)
        {
            try
            {
                DB_TechIntEntities db = new DB_TechIntEntities();
                Files newFile = new Files();

                newFile.Name = file_name;
                newFile.CreatedTs = DateTime.Now;
                newFile.LastUpdateTs = DateTime.Now;
                newFile.FileSrc = @"App_Data\Files\";
                newFile.Version = "0";
                newFile.IsEdited = false;
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
                file.Version = (Int32.Parse(file.Version) + 1).ToString();
                file.IsEdited = false;
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