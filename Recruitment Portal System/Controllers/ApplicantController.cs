using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Recruitment_Portal_System.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UBA_Network_Security_System.Models;

namespace Recruitment_Portal_System.Controllers
{
    public class ApplicantController : Controller
    {
        private ApplicationDbContext con;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;


        public ApplicantController()
        {
            con = new ApplicationDbContext();
        }

        public ApplicantController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            con = new ApplicationDbContext();
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


        #region      Applicant CRUD

        [Authorize(Roles = "Applicant")]
        [HttpGet]
        public ActionResult MyProfile()
        {
            if (TempData["msg"] != null)
            {
                ViewBag.Msg = TempData["msg"].ToString();
            }

            string Email = User.Identity.Name;
            if (Email != "")
            {
                var user = con.Users.Where(x => x.Email == Email).First();
                if (Email != null && user != null)
                {
                    var objAppProfile = con.ApplicantProfiles.FirstOrDefault(x => x.UserId == user.Id);

                    if (objAppProfile != null)
                    {
                        if (objAppProfile.Passport != null)
                        {
                            var base64 = Convert.ToBase64String(objAppProfile.Passport);
                            var imgSrc = String.Format("data:image/png;base64,{0}", base64);
                            ViewBag.Passport = imgSrc;
                        }

                        return View(objAppProfile);
                    }
                    else
                    {
                        var newApplicant = new ApplicantProfile()
                        {
                            Email = user.Email
                        };
                        UtilityHelpers utilityHelpers = new UtilityHelpers();
                        ViewBag.GenderList = utilityHelpers.GetGenders();
                        return View("CreateProfile",newApplicant);
                    }
                }
                else
                {
                    HttpNotFound();
                }
            }

            TempData["msg"] = "Sorry, you need to login as applicant!";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult CreateProfile()
        {
            UtilityHelpers utilityHelpers = new UtilityHelpers();
            ViewBag.GenderList = utilityHelpers.GetGenders();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateProfile(ApplicantProfile model)
        {
            string msg = "";

            try
            {

                //COME BACK AND CHECK IF USER WITH SAME EMAIL EXIST
                string role = "Applicant";
                var chkUserExist = con.Users.Where(x => x.Email == model.Email).FirstOrDefault();
                var chkApplicantExist = con.ApplicantProfiles.FirstOrDefault(x => x.Email == model.Email);

                if (chkUserExist == null && chkApplicantExist == null)
                {
                    //  -   -   BOTH USER AND APPLICANTPROFILE DOES NOT EXIST
                    var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {

                        var dbRole = con.Roles.Where(x => x.Name == role).FirstOrDefault();
                        if (dbRole != null)
                        {
                            var roleassign = RoleAssignment(user.Id, dbRole.Name);
                        }
                        else
                        {
                            //CREATE ROLE AND ADD USER
                            IdentityRole newRole = new IdentityRole()
                            {
                                Name = role
                            };

                            con.Roles.Add(newRole);
                            con.SaveChanges();

                            var roleassign = RoleAssignment(user.Id, newRole.Name);
                        }

                        model.UserId = user.Id;

                        if (ModelState.IsValid)
                        {
                            con.ApplicantProfiles.Add(model);
                            con.SaveChanges();
                            TempData["msg"] = "User and profile account created successfully!";
                            return RedirectToAction("MyProfile");
                        }
                        else
                        {
                            msg = "OOOPS, PLEASE PROVIDE ALL VALUES!";
                        }


                    }
                    else
                    {
                        msg = "Sorry, User account could not be created successfully!";
                    }
                    ViewBag.Msg = msg;
                    return View(model);
                }
                else if (chkUserExist != null && chkApplicantExist == null)
                {
                    //  -   -   ONLY APPLICANTPROFILE IS CREATED

                    model.UserId = chkUserExist.Id;
                    if (ModelState.IsValid)
                    {
                        con.ApplicantProfiles.Add(model);
                        con.SaveChanges();
                        msg = "Profile account created successfully!";
                    }
                    else
                        msg = "The provided data did not match required format, please try again later!!";
                    TempData["msg"] = msg;
                    return RedirectToAction("MyProfile");
                }
                else if (chkUserExist != null && chkApplicantExist != null)
                {
                    //EDIT
                    var dbObj = con.ApplicantProfiles.SingleOrDefault(o => o.User.Email == model.Email);

                    if (dbObj != null)
                    {
                        dbObj.FirstName = model.FirstName;
                        dbObj.MiddleName = model.MiddleName;
                        dbObj.SurName = model.SurName;
                        dbObj.Gender = model.Gender;
                        dbObj.DOB = model.DOB;
                        dbObj.Email = model.Email;
                        dbObj.Phone = model.Phone;
                        dbObj.LGAOrigin = model.LGAOrigin;
                        dbObj.StateOrigin = model.StateOrigin;
                        dbObj.ResidentialAddress = model.ResidentialAddress;
                        dbObj.PreferredJobLocation = model.PreferredJobLocation;
                        dbObj.EmailNotification = model.EmailNotification;
                        dbObj.Skills = model.Skills;
                        dbObj.UserId = model.UserId;

                        con.SaveChanges();
                        msg = "Profile account updated successfully!";
                        TempData["msg"] = msg;
                        return RedirectToAction("MyProfile");

                    }
                    else
                    {
                        msg = "No record found!";
                        TempData["msg"] = msg;
                        return RedirectToAction("MyProfile");

                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    msg = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN! \n" + ex.InnerException.Message.ToString();
                else
                    msg = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN! \n" + ex.Message.ToString();
            }

            UtilityHelpers utilityHelpers = new UtilityHelpers();
            ViewBag.GenderList = utilityHelpers.GetGenders();
            ViewBag.Msg = msg;
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult AddEditApplicantProfile(string ID)
        {
            ApplicantProfile model = new ApplicantProfile();
            string msg = "";

            UtilityHelpers utilityHelpers = new UtilityHelpers();

            try
            {

                if (ID != null)
                {
                    var obj = con.ApplicantProfiles.SingleOrDefault(o => o.UserId == ID);
                    if (obj != null)
                    {
                        model.FirstName = obj.FirstName;
                        model.MiddleName = obj.MiddleName;
                        model.SurName = obj.SurName;
                        model.Gender = obj.Gender;
                        model.DOB = obj.DOB;
                        model.Email = obj.Email;
                        model.Phone = obj.Phone;
                        model.LGAOrigin = obj.LGAOrigin;
                        model.StateOrigin = obj.StateOrigin;
                        model.ResidentialAddress = obj.ResidentialAddress;
                        model.PreferredJobLocation = obj.PreferredJobLocation;
                        model.EmailNotification = obj.EmailNotification;
                        model.Skills = obj.Skills;
                        model.UserId = obj.UserId;

                        ViewBag.GenderList = utilityHelpers.GetGenders();

                        return PartialView("AddEditApplicantProfile", model);

                    }
                    else
                    {
                        msg = "Record doesn't exist anymore!";
                    }
                }
            }
            catch (Exception ex)
            {
                msg = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN!";
            }
            return RedirectToAction("MyProfile", msg);
        }

        [HttpGet]
        [Authorize(Roles = "Applicant")]
        public ActionResult UploadPassport()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Applicant")]
        [ValidateAntiForgeryToken]
        public ActionResult UploadPassport(UploadEmployeePassportViewModel model)
        {
            try
            {
                string msg = "";
                // CHECK IF IMAGE1 UPLOAD FILE IS EMPTY
                if (model.PassportUpload == null || model.PassportUpload.ContentLength < 0)
                {
                    msg = "Please select a file, Try again!";
                    ViewBag.Msg = msg;

                    return View();
                }
                else if (model.PassportUpload != null && model.PassportUpload.ContentLength > 0 &&
                    model.PassportUpload.FileName.ToLower().EndsWith("jpg") ||
                    model.PassportUpload.FileName.ToLower().EndsWith("png"))
                {
                    //UPLOAD NOT EMPTY
                    string email = User.Identity.Name;

                    var user = con.Users.FirstOrDefault(x => x.Email == email);

                    string passName = user.Id + Path.GetExtension(model.PassportUpload.FileName);

                    string pathx = Server.MapPath("~/Uploads/Passports/" + passName);
                    model.PassportUpload.SaveAs(pathx);


                    if (pathx != null)
                    {
                        using (Image img = Image.FromFile(pathx))
                        {
                            var ms = new MemoryStream();

                            img.Save(ms, ImageFormat.Jpeg);

                            var bytes = ms.ToArray();

                            // NOW SAVE BYTE WITH STUDENT REGNO NUMBER
                            if (bytes.Length > 0)
                            {

                                var appl = con.ApplicantProfiles.FirstOrDefault(x => x.UserId == user.Id);
                                if (appl != null)
                                {
                                    appl.Passport = bytes;
                                    con.SaveChanges();
                                    ViewBag.Msg = "Passport Upload Completed";
                                }
                            }
                            else
                            {
                                ViewBag.Msg = "Image conversion failed!";
                            }
                        }
                    }
                    else
                        ViewBag.Msg = "Sorry, Passport Upload counld not be completed successfully" +
                            " because, the image renaming failed!";

                    if (System.IO.File.Exists(pathx))
                        System.IO.File.Delete(pathx);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message == null)
                {
                    TempData["Msg"] = "Please copy response and send to admin: \n" + ex.Message.ToString();
                    ViewBag.Msg = TempData["Msg"].ToString();
                    return View();
                }
                else
                {
                    TempData["Msg"] = "Please copy response and send to admin: \n" +
                                            ex.Message.ToString() + "\n" +
                                            ex.InnerException.Message.ToString();
                    ViewBag.Msg = TempData["Msg"].ToString();
                    return View();
                }
            }
            return RedirectToAction("MyProfile");
        }

        #endregion

        #region      JOB CATEGORY CRUD

        [HttpGet]
        public ActionResult JobCategory(string Msg)
        {

            if (Msg != null)
            {
                ViewBag.Msg = Msg.ToString();
            }

            try
            {
                var allJobCategory = con.JobCategories.ToList();
                if (allJobCategory != null && allJobCategory.Count > 0)
                {
                    ViewBag.AllJobsCategories = allJobCategory;
                    ViewBag.JobCategoryCount = allJobCategory.Count();
                }
            }
            catch (Exception ex)
            {
                ViewBag.msg = Session["csmsg"].ToString() + " " + ex.Message.ToString();
                return View();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult JobCategory(JobCategory model)
        {
            string msg = "";

            if (ModelState.IsValid)
            {

                var dbObj = con.JobCategories.SingleOrDefault(o => o.Id == model.Id);
                try
                {
                    if (model.Id != null && dbObj != null)
                    {
                        dbObj.Title = model.Title;
                        dbObj.Description = model.Description;
                        con.SaveChanges();
                        msg = "Job Category has been created successfully!";
                    }
                    else
                    {

                        JobCategory objJobCategory = new JobCategory()
                        {
                            Title = model.Title,
                            Description = model.Description
                        };

                        con.JobCategories.Add(objJobCategory);
                        con.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                        msg = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN! \n" + ex.InnerException.Message.ToString();
                    else
                        msg = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN! \n" + ex.Message.ToString();
                }
            }
            else
            {
                msg = "OOOPS, PLEASE PROVIDE ALL VALUES!";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult AddEditJobCategory(string ID)
        {
            JobCategory model = new JobCategory();
            try
            {
                if (ID != null)
                {
                    var obj = con.JobCategories.SingleOrDefault(o => o.Id == ID);
                    if (obj != null)
                    {
                        model.Id = obj.Id;
                        model.Title = obj.Title;
                        model.Description = obj.Description;
                    }
                }
            }
            catch (Exception ex)
            {
                Session["grmsg"] = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN!";
                return RedirectToAction("StateBranch");
            }
            return PartialView("AddEditJobCategory", model);
        }

        [Authorize(Roles = "SuperAdmin")]
        public JsonResult DeleteJobCategory(string ID)
        {

            bool result = false;
            if (ID != null)
            {
                var objDel = con.JobCategories.SingleOrDefault(o => o.Id == ID);
                if (objDel != null)
                {
                    con.JobCategories.Remove(objDel);
                    con.SaveChanges();
                    result = true;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion



        #region METHOD TO ADD STUDENT INTO ROLE
        [NonAction]
        public bool RoleAssignment(string userId, string role)
        {
            if (userId == null || role == null)
            {
                return false;
            }
            else
            {
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(con));

                var result = UserManager.AddToRole(userId, role);
                if (result.Succeeded)
                {
                    return true;
                }
                return false;
            }
        }
        #endregion
    }
}