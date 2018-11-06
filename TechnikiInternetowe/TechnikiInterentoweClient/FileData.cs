using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnikiInterentoweClient
{
    class FileData
    {
        public string FileId { get; set; }
        public string LastUpdateTs { get; set; }
        public string Name { get; set; }
        public int Version { get; set; }
        public bool IsEdited { get; set; }
    }
}
