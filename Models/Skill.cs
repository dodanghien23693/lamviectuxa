using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSoftSeo.Models
{
    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<Job> Jobs {get;set;}
        public DateTime CreatedOn { get; set; }


        public Skill()
        {
            CreatedOn = DateTime.Now;
        }
    }
}