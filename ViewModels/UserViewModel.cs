using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSoftSeo.Models
{
    public class EditProfileViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public string WorkPlacement { get; set; }
        public DateTime Birthday { get; set; }
        //public string IntroduceYourself { get; set; }

        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Skype { get; set; }

        //public int[] Skills { get; set; }
        //public IEnumerable<WorkExperience> WorkExperiences { get; set; }


        
    }

    public class ProfileViewModel
    {
        public string Description { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Skype { get; set; }
        public int[] Skills { get; set; }
        public IEnumerable<WorkExperience> WorkExperiences { get; set; }

    }

    public class ProjectUserViewModel
    {
        public string Title { get; set; }
        public int Cost { get; set; }
        public DateTime Time { get; set; }

    }


}