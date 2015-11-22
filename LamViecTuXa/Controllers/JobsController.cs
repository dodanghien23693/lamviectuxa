using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebSoftSeo.Models;
using Microsoft.AspNet.Identity;

namespace WebSoftSeo.Controllers
{
    public class JobsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Jobs
        public ActionResult Index()
        {
            return View(db.Jobs.ToList());
        }


        public ActionResult FindJobs(string searchString, int? fromPrice, int? toPrice, string filter, string orderBy)
        {
            IEnumerable<Job> Jobs = from j in db.Jobs 
                                    where j.Status != JobStatus.Pending
                                    where j.Status != JobStatus.Cancedled
                                    select j;
                                    
                                    
            //filter orderBy
            if (orderBy == null) orderBy = "lastest";
            if (orderBy.Equals("lastest")) Jobs = Jobs.OrderByDescending(j=>j.StartDay).Select(j=>j);
            if (orderBy.Equals("dePrice")) Jobs = Jobs.OrderByDescending(j => j.Cost).Select(j => j);
            if (orderBy.Equals("inPrice")) Jobs = Jobs.OrderBy(j => j.Cost).Select(j => j);
            
            //filter Price
            if (fromPrice != null)
            {
                if(fromPrice>=0) Jobs = Jobs.Where(j => j.Cost >= fromPrice);
            }
            if (toPrice != null)
            {
                if (toPrice >= fromPrice) Jobs = Jobs.Where(j => j.Cost <= toPrice);
            }

            //filter searching
            if (searchString != null)
            {
                Jobs = Jobs.Where(j=>j.Description.Contains(((string)searchString).ToLower()));
            }

            return View(Jobs);
        }

        // GET: Jobs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // GET: Jobs/Create
        public ActionResult Create()
        {
            ViewBag.Skills = new MultiSelectList(db.Skills.Select(s => new { Id = s.Id, Name = s.Name }), "Id", "Name");
          
            return View();
        }

        // POST: Jobs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(JobViewModel jobModel)
        {
            if (ModelState.IsValid)
            {
                List<Skill> skills = new List<Skill>();
                if(jobModel.Skills!=null && jobModel.Skills.Count()>0){
                    foreach(int skillId in jobModel.Skills){
                        skills.Add(db.Skills.Find(skillId));
                    }
                }

                Job job = new Job() {
                    Title = jobModel.Title,
                    Description = jobModel.Description,
                    Cost = jobModel.Cost,
                    EndDay = jobModel.EndDay,
                    
                    Skills = skills,
                    User = SystemInfo.GetCurrentUser(),
                    Status = JobStatus.Pending 
                };
                db.Jobs.Add(job);
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            //ViewBag.Skills = new MultiSelectList(db.Skills.Select(s => new { Id = s.Id, Name = s.Name }), "Id", "Name");
            return View(jobModel);
        }

        // GET: Jobs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }

            JobViewModel jobVM = new JobViewModel(){
                Id = job.Id,
                Title = job.Title,
                EndDay = job.EndDay,
                Cost = job.Cost,
                Description = job.Description,
                Skills = job.Skills.Select(s=>s.Id).ToArray()
                
            };

            //ViewBag.Skills = new MultiSelectList(db.Skills.Select(s => new { Id = s.Id, Name = s.Name }), "Id", "Name");
            return View(jobVM);
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(JobViewModel jobVM)
        {
            if (ModelState.IsValid)
            {
                Job job = db.Jobs.Find(jobVM.Id);
                if (job == null)
                {
                    return HttpNotFound();
                }

                job.Title = jobVM.Title;
                job.Description = jobVM.Description;
                job.Cost = jobVM.Cost;
                job.EndDay = jobVM.EndDay;

                job.Skills.Clear();
                if (jobVM.Skills != null)
                {
                    List<Skill> skills = new List<Skill>();
                    foreach (int skillId in jobVM.Skills)
                    {
                        skills.Add(db.Skills.Find(skillId));
                    }
                    job.Skills = skills;
                }
                
                //db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(jobVM);
        }

        // GET: Jobs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Job job = db.Jobs.Find(id);
            db.Jobs.Remove(job);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        
        [HttpPost]
        public ActionResult Bid(int JobId)
        {
            var job = db.Jobs.Find(JobId);
            if (job != null)
            {
                if (Request.Form["Description"] != null)
                {

                    //job.Bidders.Add(new Bidder() { BidDay = DateTime.Now, User = SystemInfo.GetCurrentUser(), Description = Request.Form["Description"], IsChoosed = false });
                    var bidder = new Bidder() { BidDay = DateTime.Now, Description = Request.Form["Description"], IsChoosed = false};
                    
                    
                    job.Bidders.Add(bidder);
                    db.SaveChanges();
                    bidder.User = db.Users.Find(User.Identity.GetUserId());                    
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = job.Id });
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SelectBidder(int JobId,string BidderId)
        {
            if (Request.IsAjaxRequest())
            {
                var job = db.Jobs.Find(JobId);
                if (job != null)
                {
                    var bidder = job.Bidders.Where(b => b.IsChoosed == true).FirstOrDefault();
                    if (bidder != null)
                    {
                        if (bidder.User.Id != BidderId)
                        {
                            bidder.IsChoosed = false;
                            db.SaveChanges();
                        }
                        var setBidder = db.Bidders.Where(b => b.User.Id == BidderId).FirstOrDefault();
                        if (setBidder != null)
                        {
                            setBidder.IsChoosed = true;
                            db.SaveChanges();
                            return Json(new { status = "success" });
                        }
                    }
                }
                return Json(new { status = "fail" });
            }
            return null;
        }

        [HttpPost]
        public ActionResult ChangeJobStatus(int JobId, int Status)
        {
            if (Request.IsAjaxRequest())
            {
                var job = db.Jobs.Find(JobId);
                if (job != null)
                {
                    if (User.Identity.GetUserId() == job.User.Id)
                    {
                        job.Status = Status;
                        db.SaveChanges();
                        return Json(new { status = "success" });
                    }
                }
                return Json(new { status = "fail" });
            }
            return null;
        }

        protected override void Dispose(bool disposing)
        {
            
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
