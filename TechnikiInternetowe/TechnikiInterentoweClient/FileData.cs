using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnikiInterentoweClient
{
    public class FileData
    {
        public string FileId { get; set; }
        public string LastUpdateTs { get; set; }
        public string Name { get; set; }
        public int Version { get; set; }
        public bool IsEdited { get; set; }
        public string EditorName { get; set; }
    }


    public class Message
    {
        public int Key;
        public string Destination;
        public string Sender;
        public string Value;
    }
}
