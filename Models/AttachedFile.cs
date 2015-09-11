using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebSoftSeo.Models
{
    public  enum FileType
    {
        Pdf = 0,
        Word = 1
    }

    public class AttachedFile
    {
        public int Id { get; set; }
        public int FileType { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public virtual Job Job { get; set; }

        public DateTime CreatedOn { get; set; }

        public AttachedFile()
        {
            CreatedOn = DateTime.Now;
        }
    }
}
