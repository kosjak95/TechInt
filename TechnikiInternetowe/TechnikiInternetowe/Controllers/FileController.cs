using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Web.Script.Serialization;

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

        [HttpGet]
        [Route("Files")]
        public string GetListOfFilesOnServer()
        {
            return new JavaScriptSerializer().Serialize(filesList);
        }
    }

    public class OpenFileController : Controller
    {
        private string project_path { get; set; }

        public OpenFileController()
        {
            project_path = AppDomain.CurrentDomain.BaseDirectory + @"App_Data\Files\";
        }

        [HttpGet]
        [Route("OpenFile/{fileName}")]
        public string GetFileContent()
        {
            string nameOfFile = RouteData.Values["fileName"] as string;
            string contents = "";
            try
            {
                using (StreamReader sr = new StreamReader(project_path + nameOfFile +".txt"))
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
