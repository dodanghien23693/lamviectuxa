using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSoftSeo.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace WebSoftSeo.Controllers
{
    [Authorize]
    public class WorkExperienceController : Controller
    {

        private ApplicationDbContext _dbContext;
        ApplicationDbContext _db = new ApplicationDbContext();

        //public WorkExperienceController(ApplicationDbContext db)
        //{
        //    this._dbContext = db;
        //}

        public ApplicationDbContext DbContext{
            get
            {
                if (_dbContext == null)
                {
                    this._dbContext = new ApplicationDbContext();
                }
                return _dbContext;
            }
            private set 
            { 
                _dbContext = value; 
            }
        }

        // GET: WorkExperience
        public ActionResult Index()
        {
            return View();
        }

        // GET: WorkExperience/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: WorkExperience/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WorkExperience/Create
        [HttpPost]
        public ActionResult Create(WorkExperience workExperience)
        {
            if (ModelState.IsValid)
            {

                workExperience.User = DbContext.Users.Find(User.Identity.GetUserId());
                DbContext.WorkExperiences.Add(workExperience);
                DbContext.SaveChanges();
                return Json(new { Id = workExperience.Id });
            }
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: WorkExperience/Edit/5
        public ActionResult Edit(int id)
        {
            if (id != null)
            {
                WorkExperience wex =  DbContext.WorkExperiences.Find(id);
                if (wex != null)
                {
                    return Json(new { Id = wex.Id, WorkPlacement = wex.WorkPlacement, Address = wex.Address, Datetime = wex.Datetime, Company = wex.Company, Description = wex.Description }, JsonRequestBehavior.AllowGet);
                }
            }
            return null;
            //return View();
        }

        // POST: WorkExperience/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, WorkExperience workExperience)
        {
            if (id != null)
            {
                WorkExperience wex  = DbContext.WorkExperiences.Find(id);
                if (wex != null)
                {
                    wex.WorkPlacement = workExperience.WorkPlacement;
                    wex.Address = workExperience.Address;
                    wex.Datetime = workExperience.Datetime;
                    wex.Company = workExperience.Company;
                    wex.Description = workExperience.Description;
                    DbContext.SaveChanges();
                    return Json(new { state = "success" });
                }
            }
            return null;
        }

        // GET: WorkExperience/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: WorkExperience/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (id != null)
            {
                WorkExperience wex = DbContext.WorkExperiences.Find(id);
                if (wex != null)
                {
                    DbContext.WorkExperiences.Remove(wex);
                    DbContext.SaveChanges();
                    return Json(new { state = "success" });
                }

            }
            return null;
        }
    }
}
