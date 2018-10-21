using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TechnikiInternetowe.Models;
using TechnikiInternetowe.NewFolder1;

namespace TechnikiInternetowe.Controllers
{
    public class FileController : Controller
    {
        private List<string> filesList { get; set; }
        private string project_path { get; set; }

        public FileController()
        {
            
            filesList = new List<string>();
            project_path = AppDomain.CurrentDomain.BaseDirectory + @"App_Data\Files";

            DirectoryInfo di = new DirectoryInfo(project_path);
            FileInfo[] files = di.GetFiles("*.txt");
            
            foreach ( var file in files)
            {
                filesList.Add(file.Name);
            }
            
        }

        [Route("Json")]
        public string Index()
        {
            DB_TechIntEntities db = new DB_TechIntEntities();
            var dane = db.Files;

            return new JavaScriptSerializer().Serialize(dane);


        }
        [HttpGet]
        [Route("Files")]
        public string GetListOfFilesOnServer()
        {
            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(filesList);

            return new JavaScriptSerializer().Serialize(filesList);
        }
    }
}
