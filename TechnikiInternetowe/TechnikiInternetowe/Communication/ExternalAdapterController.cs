using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using TechnikiInternetowe.Controllers;


namespace TechnikiInternetowe.Communication
{
    public class ExternalAdapterController : Controller
    {
        private static string project_path { get; set; }

        public ExternalAdapterController()
        {
            project_path = AppDomain.CurrentDomain.BaseDirectory;
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

        [System.Web.Http.HttpOptions]
        [Route("Cludge")]
        public async Task<string> PermissionOnCreateFile1([System.Web.Http.FromBody] string file_name)
        {
            if (file_name == null)
                return JsonConvert.SerializeObject(false);

            return await Task.Run(() => JsonConvert.SerializeObject(FileController.PermissionOnCreateFile(project_path, file_name)));
        }


        [HttpPost]
        [Route("UpdateContent")]
        public async Task<bool> UpdateFileContent(string file_name, string file_data)
        {
            return await Task.Run(() => FileController.UpdateFileContent(project_path, file_name, file_data));
        }

        [System.Web.Http.HttpOptions]
        [Route("UpdateContentCludge")]
        public async Task<string> UpdateFileContentReqStruct([System.Web.Http.FromBody] string fileName, string content)
        {
            return await Task.Run(() => JsonConvert.SerializeObject(FileController.UpdateFileContent(project_path, fileName,content)));
        }

        [System.Web.Http.HttpOptions]
        [Route("ReleaseFileCludge")]
        public async Task<string> ReleaseFile([System.Web.Http.FromBody] string fileName)
        {
            return await Task.Run(() => JsonConvert.SerializeObject(FileController.ReleaseFile(fileName)));
        }
    }
}
