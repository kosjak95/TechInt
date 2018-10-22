using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TechnikiInternetowe.DBEntity;
using System.Linq;
using Newtonsoft.Json;

namespace TechnikiInternetowe.Controllers
{
    public class FileController : Controller
    {
        private string project_path { get; set; }
        private DB_TechIntEntities db { get; set; }

        /// <summary>
        /// Controller
        /// </summary>
        public FileController()
        {
            project_path = AppDomain.CurrentDomain.BaseDirectory;
            db = new DB_TechIntEntities();
        }

        #region public methods

        /// <summary>
        /// Test db json
        /// </summary>
        /// <returns></returns>
        [Route("Json")]
        public string Index()
        {
            var dane = db.Files;
            KeyValuePair<int, string> test = new KeyValuePair<int, string>(2, "asdasdasd");

            return new JavaScriptSerializer().Serialize(dane);
        }

        /// <summary>
        /// Return Json formated string with Names of each file on server
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Files")]
        public string GetListOfFilesOnServer()
        {
            return new JavaScriptSerializer().Serialize(db.Files.Select(s => s.Name).ToList());
        }

        /// <summary>
        /// Return contend of given by argument file
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("OpenFile/{fileName}")]
        public string GetFileContent(string fileName)
        {
            string contents = "";
            Files file = null;
            List<KeyValuePair<int, string>> file_content = new List<KeyValuePair<int, string>>();
            try
            {
                file = db.Files.Where(w => w.Name == fileName).First();
                string file_path = project_path + @file.FileSrc + file.Name + ".txt";
                using (StreamReader sr = new StreamReader(file_path))
                {
                    contents = sr.ReadToEnd();
                }

                KeyValuePair<int, string> test = new KeyValuePair<int, string>(file.FileId, contents);
                file_content.Add(test);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Write(e.Message);
                contents = "We have some problem with open this file";
            }


            return JsonConvert.SerializeObject(file_content[0]);;
        }

        /// <summary>
        /// Fucntion gives information about possibility of creation new note
        /// if it is possible insert row to Db, and lock file on edit automaticly
        /// </summary>
        /// <param name="newFile">Inserted by user name of file</param>
        /// <returns>true if creation is possible and done, other way false</returns>
        [HttpPost]
        [Route("TryCreate")]
        public bool PermissionOnCreateFile(string file_name)
        {

            if (isCreationOfFilePossible(file_name))
                return insertDataToDb(file_name);

            return false;
        }

        /// <summary>
        ///  @TODO: IN PROGRESS
        /// </summary>
        /// <param name="file_data"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateContent")]
        public bool UpdateFileContent(int file_id, string file_data)
        {
            Files file = db.Files.Where(w => w.FileId == file_id).Single();
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

            return true;
        }

        #endregion

        #region private methods

        //check if file with this name not exist in db
        private bool isCreationOfFilePossible(string file_name)
        {
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

        //try insert given file into db
        private bool insertDataToDb(string file_name)
        {
            try
            {
                Files newFile = new Files();

                newFile.Name = file_name;
                newFile.CreatedTs = DateTime.Now;
                newFile.FileSrc = project_path + @"App_Dat\Files\" + file_name + ".txt";
                newFile.Version = "1";
                newFile.IsEdited = true;
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

        #endregion
    }

}
