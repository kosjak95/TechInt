using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TechnikiInternetowe.Controllers;
using TechnikiInternetowe.DBEntity;

namespace TechnikiInternetowe.Communication
{
    public class ExternalAdapterController : Controller
    {

        private static string project_path { get; set; }

        public ExternalAdapterController()
        {
            project_path = AppDomain.CurrentDomain.BaseDirectory;
        }

        [HttpPost]
        [Route("Test1")]
        public async Task<string> DoSmth(string filename)
        {
            return await Task.Run(() => HelloController.HelloName(filename));
        }

        [HttpGet]
        [Route("Json")]
        public async Task<string> TestFunction()
        {
            return await Task.Run(() => FileController.Index());
        }

        [HttpGet]
        [Route("Files")]
        public async Task<string> GetListOfFilesOnServer()
        {
            return await Task.Run(() => FileController.GetListOfFilesOnServer());
        }

        [HttpGet]
        [Route("OpenFile/{fileName}")]
        public async Task<string> GetFileContent(string fileName)
        {
            return await Task.Run(() => FileController.GetFileContent(project_path, fileName));
        }

        [HttpPost]
        [Route("TryCreate")]
        public async Task<bool> PermissionOnCreateFile(string file_name)
        {
            return await Task.Run(() => FileController.PermissionOnCreateFile(project_path, file_name));
        }

        [HttpPost]
        [Route("UpdateContent")]
        public async Task<bool> UpdateFileContent(int file_id, string file_data)
        {
            return await Task.Run(() => FileController.UpdateFileContent(project_path, file_id, file_data));
        }
    }
}