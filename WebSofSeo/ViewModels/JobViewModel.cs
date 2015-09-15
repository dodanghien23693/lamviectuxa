using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSoftSeo.Models;

namespace WebSoftSeo.ViewModels
{
    public class CreateJobViewModel
    {
        public string Title { get; set; }
        public int Cost { get; set; }
        public string Description { get; set; }

        //public virtual ICollection<AttachedFile> Files { get; set; }
        public HttpPostedFile[] Files { get; set; }

        public DateTime StartDay { get; set; }
        public DateTime EndDay { get; set; }
   
        public virtual ICollection<Skill> Skills { get; set; }

    }


}