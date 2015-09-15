using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebSoftSeo.Models
{
    public static class JobStatus
    {
        public const  int Done=0;
        public const int Suspended =1;
        public const int Cancedled = 2;
        public const int Pending = 3;
        public const int Starting = 4;
    }
    public class Job
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Cost { get; set; }
        public string Description { get; set; }

        public virtual ICollection<AttachedFile> Files { get; set; }


        public DateTime StartDay { get; set; }
        public DateTime EndDay { get; set; }

        public  virtual ICollection<Bidder> Bidders { get; set; }

        public int Status { get; set; }

        public virtual ICollection<Skill> Skills { get; set; }

        public virtual ApplicationUser User { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }


        public Job()
        {
            var now = DateTime.Now;
            StartDay = now;
            CreatedOn = now;
            ModifiedOn = now;
            Status = JobStatus.Pending;
        }
    }

    public class JobViewModel
    {
        public int Id {get;set;}

        [Required]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [Range(0,50000000)]
        public int Cost { get; set; }

        
        public int[] Skills { get; set; }

        [Required]
        [Display(Name="End Day")]
        [DataType(DataType.Date)]
        public DateTime EndDay { get; set; }

        public JobViewModel()
        {
            DateTime now = DateTime.Now;
            EndDay = now;
        }
    }
}