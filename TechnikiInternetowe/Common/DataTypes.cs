using System;
using System.Collections.Generic;

namespace TechnikiInterentoweCommon
{
    public abstract class FileBase
    {
        public int FileId { get; set; }
        public bool IsEdited { get; set; }
        public string Name { get; set; }
        public string EditorName { get; set; }
    }

    public class CommonFileContent : FileBase
    {
        public string FileContent1 { get; set; }
    }

    public class FileData : FileBase
    {
        public string LastUpdateTs { get; set; }
        public int Version { get; set; }
    }

    public class FullFileData : FileData
    {
        public string FileContent { get; set; }
        public bool WasChanged { get; set; }

        public void setAll(FileData p_base, string p_content)
        {
            FileId = p_base.FileId;
            IsEdited = p_base.IsEdited;
            Name = p_base.Name;
            EditorName = p_base.EditorName;
            LastUpdateTs = p_base.LastUpdateTs;
            Version = p_base.Version;
            FileContent = p_content;
            WasChanged = false;
        }
    }

    public class SynchronizeAfterConnectionEstablishedMsg
    {
        public List<FullFileData> filesList;
        public string sender;
    }

    public class UserAndFileNamesPair
    {
        public string UserName { get; set; }
        public string FileName { get; set; }
    }

    public class Message
    {
        public MsgType Key;
        public string Destination;
        public string Sender;
        public string Value;
    }

    //1. server update actions
    //2. naming of clients
    //3. chat
    public enum MsgType
    {
        REFRESH_FILES_LIST_MSG = 1,
        FAIL_SYNC_FILES_MSG = 2,
        AUTHORIZATION_MSG = 3,
        CHAT_MSG = 4
    }

    public class NoUserNameException : Exception{}
}