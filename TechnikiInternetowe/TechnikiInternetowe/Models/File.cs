using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TechnikiInternetowe.Models
{
    public class File
    {
        [Key]
        public int FileId { get; set; }
        public string FileName { get; set; }
        public int version { get; set; }
    }
}