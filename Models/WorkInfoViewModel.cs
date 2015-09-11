using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using WebSoftSeo.Models;

namespace WebSoftSeo.Models
{
    public class WorkInfoViewModel
    {
        public int Id { get; set; }

        
        public string ImageUrl { get; set; }

        [Required]
        [Display(Name="Work Placement")]
        public string WorkPlacement { get; set; }

        [Display(Name="Introduce Yourself")]
        [Required]
        [DataType(DataType.MultilineText)]
        public string IntroduceYourself { get; set; }


        public int[] Skills { get; set; }
        //public ICollection<WorkExperience> WorkExperiences { get; set; }

    }
}
