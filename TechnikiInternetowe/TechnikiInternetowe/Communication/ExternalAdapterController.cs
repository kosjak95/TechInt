using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using TechnikiInterentoweCommon;
using TechnikiInternetowe.Controllers;
using TechnikiInternetowe.WebSockets;

namespace TechnikiInternetowe.Communication
{
    public class ExternalAdapterController : Controller
    {
        private static string Project_path { get; set; }
        private ServerWebSocket ServerSocket { get; set; }

        public ExternalAdapterController()
        {
            Project_path = AppDomain.CurrentDomain.BaseDirectory;
            ServerSocket = ServerWebSocket.Instance;
        }

        ~ExternalAdapterController()
        {
            ServerSocket.closeServer();
        }

        [HttpGet]
        [Route("Files")]
        public async Task<string> GetListOfFilesOnServer()
        {
            return await Task.Run(() => FileController.GetListOfFilesOnServer());
        }

        [HttpGet]
        [Route("OpenFile/{fileName}/{editorName}")]
        public async Task<string> GetFileContent(string fileName, string editorName)
        {
            return await Task.Run(() => FileController.GetFileContent(Project_path, fileName, editorName));
        }

        [HttpPost]
        [Route("TryCreate")]
        public async Task<bool> PermissionOnCreateFile(string file_name)
        {
            return await Task.Run(() => FileController.PermissionOnCreateFile(Project_path, file_name));
        }

        [HttpPost]
        [Route("SynchronizeAfterConnectionEstablished")]
        public async Task<bool> SynchronizeAfterConnectionEstablished(List<FullFileData> filesList)
        {
            return await Task.Run(() => FileController.SynchronizeAfterConnectionEstablished(Project_path, filesList));
        }

        [System.Web.Http.HttpOptions]
        [Route("Cludge")]
        public async Task<string> PermissionOnCreateFile1([System.Web.Http.FromBody] string file_name)
        {
            if (file_name == null)
                return JsonConvert.SerializeObject(false);

            return await Task.Run(() => JsonConvert.SerializeObject(FileController.PermissionOnCreateFile(Project_path, file_name)));
        }


        [HttpPost]
        [Route("UpdateContent")]
        public async Task<bool> UpdateFileContent(string file_name, string file_data)
        {
            return await Task.Run(() => FileController.UpdateFileContent(Project_path, file_name, file_data));
        }

        [System.Web.Http.HttpOptions]
        [Route("UpdateContentCludge")]
        public async Task<string> UpdateFileContentReqStruct([System.Web.Http.FromBody] string fileName, string content)
        {
            return await Task.Run(() => JsonConvert.SerializeObject(FileController.UpdateFileContent(Project_path, fileName,content)));
        }

        [System.Web.Http.HttpOptions]
        [Route("ReleaseFileCludge")]
        public async Task<string> ReleaseFile([System.Web.Http.FromBody] string fileName)
        {
            return await Task.Run(() => JsonConvert.SerializeObject(FileController.ReleaseFile(fileName)));
        }
    }
}
