using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebSoftSeo.Models
{
    public class WorkExperience 
    {
        public int Id { get; set; }

        public string WorkPlacement { get; set; }

        public string Address { get; set; }
        public DateTime Datetime { get; set; }
        
        public string Company { get; set; }

        public string Description { get; set; }

        public virtual ApplicationUser User { get; set; }

        public WorkExperience()
        {
            Datetime = DateTime.Now;
        }

    }
}