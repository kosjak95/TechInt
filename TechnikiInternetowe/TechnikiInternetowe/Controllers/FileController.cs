using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TechnikiInternetowe.DBEntity;


namespace TechnikiInternetowe.Controllers
{
    public class FileController : Controller
    {
        private List<string> filesList { get; set; }
        private string project_path { get; set; }

        public FileController()
        {
            filesList = new List<string>();
            project_path = AppDomain.CurrentDomain.BaseDirectory + @"App_Data\Files\";

            DirectoryInfo di = new DirectoryInfo(project_path);
            FileInfo[] files = di.GetFiles("*.txt");
            
            foreach ( var file in files)
            {
                filesList.Add(file.Name);
            }
        }

        /// <summary>
        /// Test db json
        /// </summary>
        /// <returns></returns>
        [Route("Json")]
        public string Index()
        {
            DB_TechIntEntities db = new DB_TechIntEntities();
            var dane = db.Files;

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
            return new JavaScriptSerializer().Serialize(filesList);
        }

        /// <summary>
        /// Return contend of given by argument file
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("OpenFile/{fileName}")]
        public string GetFileContent(string fileName)
        {
            //string nameOfFile = RouteData.Values["fileName"] as string;
            string contents = "";
            try
            {
                string file_path = project_path + fileName + ".txt";
                using (StreamReader sr = new StreamReader(file_path))
                {
                    contents = sr.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Write(e.Message);
                contents = "We have some problem with open this file";
            }

            return new JavaScriptSerializer().Serialize(contents);
        }
    }

}
