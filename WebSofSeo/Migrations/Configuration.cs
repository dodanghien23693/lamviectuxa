namespace WebSoftSeo.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Security;
    using WebSoftSeo.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<WebSoftSeo.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(WebSoftSeo.Models.ApplicationDbContext context)
        {
  
            //var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            ////Add role
            //IdentityRole superadminRole = context.Roles.FirstOrDefault(r => r.Name == "SuperAdmin");
            //if (superadminRole == null)
            //{
            //    superadminRole = new IdentityRole("SuperAdmin");
            //    context.Roles.Add(superadminRole);
            //    context.SaveChanges();
            //}

            //IdentityRole adminRole = context.Roles.FirstOrDefault(r => r.Name == "Admin");
            //if (adminRole == null)
            //{
            //    adminRole = new IdentityRole("Admin");
            //    context.Roles.Add(adminRole);
            //    context.SaveChanges();
            //}


            //IdentityRole userRole = context.Roles.FirstOrDefault(r => r.Name == "User");
            //if (userRole == null)
            //{
            //    userRole = new IdentityRole("User");
            //    context.Roles.Add(userRole);
            //    context.SaveChanges();
            //}

            ////Add User
            //ApplicationUser superadmin = userManager.FindByName("superadmin");
            //if (superadmin == null)
            //{
            //    superadmin = new ApplicationUser
            //    {
            //        UserName = "superadmin",
            //        Email = "superadmin@gmail.com",
            //        FirstName = "superadmin",
            //        LastName = "websoftseo",
            //        Birthday = DateTime.Now,
            //        Gender = true
            //    };
            //    userManager.Create(superadmin, "123456");
            //}
            //superadmin.Roles.Clear();
            //superadmin.Roles.Add(new IdentityUserRole { UserId = superadmin.Id, RoleId = superadminRole.Id });
            //context.SaveChanges();

            //ApplicationUser admin = userManager.FindByName("admin");
            //if (admin == null)
            //{
            //    admin = new ApplicationUser
            //    {
            //        UserName = "admin",
            //        Email = "admin@gmail.com",
            //        FirstName = "admin",
            //        LastName = "websoftseo",
            //        Birthday = DateTime.Now,
            //        Gender = true
            //    };
            //    userManager.Create(admin, "123456");
            //}
            //admin.Roles.Clear();
            //admin.Roles.Add(new IdentityUserRole { UserId = admin.Id, RoleId = adminRole.Id });
            //context.SaveChanges();

            //ApplicationUser user = userManager.FindByName("user");
            //if (user == null)
            //{
            //    user = new ApplicationUser
            //    {
            //        UserName = "user",
            //        Email = "user@gmail.com",
            //        FirstName = "user",
            //        LastName = "websoftseo",
            //        Birthday = DateTime.Now,
            //        Gender = true
            //    };
            //    userManager.Create(user, "123456");
            //}
            //user.Roles.Clear();
            //user.Roles.Add(new IdentityUserRole { UserId = user.Id, RoleId = userRole.Id });
            //context.SaveChanges();



        }
    }
}
