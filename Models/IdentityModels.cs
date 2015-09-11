using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebSoftSeo.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }


        public System.DateTime Birthday { get; set; }

        public bool Gender { get; set; }

        public string Address { get; set; }

        public string Skype { get; set; }

        public string IntroduceYourself { get; set; }

        public virtual ICollection<Skill> Skills { get; set; }

        public string WorkPlacement { get; set; } 

        public virtual ICollection<WorkExperience> WorkExperiences { get; set; }

        public int Rating { get; set; }

        public string ImageUrl { get; set; }

        public DateTime CreatedOn { get; set; }

        public ApplicationUser()
        {
            CreatedOn = DateTime.Now;
            Birthday = DateTime.Now;
            Rating = 0;
        }
    }




    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<WorkExperience> WorkExperiences { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<AttachedFile> AttachedFiles { get; set; }
        public DbSet<Bidder> Bidders { get; set; }
        public DbSet<Skill> Skills { get; set; }
        
    }
}