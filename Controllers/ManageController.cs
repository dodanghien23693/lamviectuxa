using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WebSoftSeo.Models;
using System.Collections.Generic;

namespace WebSoftSeo.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext _dbContext = new ApplicationDbContext();

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            //ViewBag.StatusMessage =
            //    message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
            //    : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
            //    : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
            //    : message == ManageMessageId.Error ? "An error has occurred."
            //    : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
            //    : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
            //    : "";

            //var userId = User.Identity.GetUserId();
            //var model = new IndexViewModel
            //{
            //    HasPassword = HasPassword(),
            //    PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
            //    TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
            //    Logins = await UserManager.GetLoginsAsync(userId),
            //    BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            //};
            //return View(model);


            //add WorkExperience Sample
            //ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
            //ApplicationDbContext db = new ApplicationDbContext();
            //WorkExperience workExperience = new WorkExperience() { 
            //    WorkPlacement = "Designer", 
            //    Datetime = DateTime.Now,
            //    User=db.Users.Find(User.Identity.GetUserId())
            //};   
            //db.WorkExperiences.Add(workExperience);
            //db.SaveChanges();

            return RedirectToAction("Profile");
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        //
        // GET: /Manage/RemovePhoneNumber
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }



        public ActionResult Profile() {


            return RedirectToAction("EditProfile");
            ApplicationUser user = _dbContext.Users.Find(User.Identity.GetUserId());
            

            ViewBag.Skills = string.Join(",", user.Skills.Select(s=>s.Id));
            return View(user);
            
        }

        public ActionResult EditProfile(){
             

            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
            EditProfileViewModel userVM = new EditProfileViewModel();
            userVM.Id = user.Id;
            userVM.FirstName = user.FirstName;
            userVM.LastName = user.LastName;
            //userVM.WorkPlacement = user.WorkPlacement;
            userVM.Birthday = user.Birthday;
            //userVM.IntroduceYourself = user.IntroduceYourself;
            userVM.Address = user.Address;
            userVM.PhoneNumber = user.PhoneNumber;
            userVM.Email = user.Email;
            userVM.Skype = user.Skype;

            //userVM.WorkExperiences = user.WorkExperiences;
            //userVM.Skills = user.Skills.Select(s => s.Id).ToArray();

            //ViewBag.Skills = new MultiSelectList(_dbContext.Skills.Select(s => new { Id = s.Id, Name = s.Name }), "Id", "Name");
            return View(userVM);
        }
            

        [HttpPost]
        public ActionResult EditProfile(EditProfileViewModel profile)
        {
  
            ApplicationUser user = _dbContext.Users.Find(User.Identity.GetUserId());
            ViewBag.Skills = new MultiSelectList(_dbContext.Skills.Select(s => new { Id = s.Id, Name = s.Name }), "Id", "Name");
            if (ModelState.IsValid)
            {
                //List<Skill> skills = new List<Skill>();
                //foreach (int skillId in profile.Skills)
                //{
                //   skills.Add(_dbContext.Skills.Find(skillId));
                //   // user.Skills.Add(_dbContext.Skills.Find(skillId));
                    
                //}
                //user.Skills.Clear();
                //user.Skills = skills;
                
                user.FirstName = profile.FirstName;
                user.LastName = profile.LastName;
                //user.WorkPlacement = profile.WorkPlacement;
                user.Birthday = profile.Birthday;
                //user.IntroduceYourself = profile.IntroduceYourself;
                user.Address = profile.Address;
                user.PhoneNumber = profile.PhoneNumber;
                user.Email = profile.Email;
                user.Skype = profile.Skype;
                
                //_dbContext.Entry(user).State    = System.Data.Entity.EntityState.Modified;
                _dbContext.SaveChanges();

                return View(profile);
            }

            //profile.WorkExperiences = user.WorkExperiences;
            return View(profile);
        }

        public ActionResult WorkInfo()
        {
            WorkInfoViewModel workInfo = new WorkInfoViewModel();
            ApplicationUser user = _dbContext.Users.Find(User.Identity.GetUserId());
            if (user != null)
            {
                
                workInfo.WorkPlacement = user.WorkPlacement;
                workInfo.IntroduceYourself = user.IntroduceYourself;
               
                workInfo.Skills = user.Skills.Select(s => s.Id).ToArray();

                if (user.ImageUrl == null || user.ImageUrl == string.Empty)
                {
                    
                    workInfo.ImageUrl =  VirtualPathUtility.ToAbsolute(SystemInfo.DefaultProfileImage  );
                }
                else
                {
                    workInfo.ImageUrl = VirtualPathUtility.ToAbsolute(user.ImageUrl);
                }

            }

            //ViewBag.Skills = ViewBag.Skills = new MultiSelectList(_dbContext.Skills.Select(s => new { Id = s.Id, Name = s.Name }), "Id", "Name");
            return View(workInfo);
        }

        [HttpPost]
        public ActionResult WorkInfo(WorkInfoViewModel workInfoVM, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = _dbContext.Users.Find(User.Identity.GetUserId());
                
                user.WorkPlacement = workInfoVM.WorkPlacement;
                user.IntroduceYourself = workInfoVM.IntroduceYourself;

                user.Skills.Clear();
                if (workInfoVM.Skills != null)
                {
                    List<Skill> skills = new List<Skill>();
                    foreach (int skillId in workInfoVM.Skills)
                    {
                        skills.Add(_dbContext.Skills.Find(skillId));
                        // user.Skills.Add(_dbContext.Skills.Find(skillId));

                    }
                    user.Skills = skills;
                }
                

                string path = "";
                string pic = "";
                _dbContext.SaveChanges();

                if (Image != null)
                {
                    string ext = System.IO.Path.GetExtension(Image.FileName);

                    if (ext == ".png" || ext == ".jpg" || ext == ".jpeg" || ext == ".gif")
                    {
                        //delete old image
                        try
                        {
                            if (user.ImageUrl != SystemInfo.DefaultProfileImage)
                            {
                                path = Server.MapPath(user.ImageUrl);
                                System.IO.File.Delete(path);
                            }
                            
                        }
                        catch (Exception e)
                        {

                        }
                        pic = user.Id+"_" + Guid.NewGuid()+"_" + System.IO.Path.GetFileName(Image.FileName);
                        path = System.IO.Path.Combine(Server.MapPath("~/Content/ProfileImages"), pic);
                        // file is uploaded
                        Image.SaveAs(path);
                        user.ImageUrl = "~/Content/ProfileImages" + "/" + pic;
                        workInfoVM.ImageUrl = "/Content/ProfileImages" + "/" + pic;

                        _dbContext.SaveChanges();
                    }
                    else
                    {
                        ModelState.Remove("PopupImageUrl");
                        ModelState.AddModelError("PopupImageUrl", "Chỉ được up ảnh có đuôi  .jpg .jpeg .png .gif ");
                        
                        return View(workInfoVM);
                    }

                }

                else
                {
                    //configModel.PopupImageUrl = config.PopupImageUrl;
                }

            }

            //ViewBag.Skills = ViewBag.Skills = new MultiSelectList(_dbContext.Skills.Select(s => new { Id = s.Id, Name = s.Name }), "Id", "Name");
            
            return View(workInfoVM);
        }

       protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

#region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

#endregion
    }
}