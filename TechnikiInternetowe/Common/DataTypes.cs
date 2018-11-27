﻿namespace TechnikiInterentoweCommon
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
        SYSTEM_ACTION_MSG = 1,
        AUTHORIZATION_MSG = 2,
        CHAT_MSG = 3
    }
}