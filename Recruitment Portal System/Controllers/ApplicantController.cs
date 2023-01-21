using Microsoft.AspNet.Identity;
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

namespace Recruitment_Portal_System.Controllers
{
    public class ApplicantController : Controller
    {
        private ApplicationDbContext con;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;


        public ApplicantController()
        {
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

        [HttpGet]
        public ActionResult MyProfile(string Msg)
        {

            if (Msg != null)
            {
                ViewBag.Msg = Msg.ToString();
            }

            try
            {
                var user_email = User.Identity.Name;
                var user = con.Users.FirstOrDefault(c => c.Email == user_email);
                var myProfile = con.ApplicantProfiles.FirstOrDefault(x => x.UserId == user.Id);
                if (myProfile != null)
                {
                    ViewBag.Applicant = myProfile;
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
        public async Task<ActionResult> MyProfile(ApplicantProfile model)
        {
            string msg = "";

            if (ModelState.IsValid)
            {

                try
                {

                    //COME BACK AND CHECK IF USER WITH SAME EMAIL EXIST

                    var chkUserExist = con.Users.FirstOrDefault(x => x.Email == model.Email);
                    var chkApplicantExist = con.ApplicantProfiles.FirstOrDefault(x => x.Email == model.Email);

                    if (chkUserExist == null && chkApplicantExist == null)
                    {
                        //  -   -   BOTH USER AND APPLICANTPROFILE DOES NOT EXIST
                        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                        var result = await UserManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            ////CHECK IF UPLOAD FILE IS EMPTY
                            if (model.PassportUpload == null || model.PassportUpload.ContentLength < 0)
                            {
                                msg = "Please select a file, Try again!";
                                ViewBag.Msg = msg;
                                return View(model);
                            }
                            else if (model.PassportUpload != null && model.PassportUpload.ContentLength > 0 &&
                                model.PassportUpload.FileName.ToLower().EndsWith("jpg") ||
                                model.PassportUpload.FileName.ToLower().EndsWith("png") ||
                                model.PassportUpload.FileName.ToLower().EndsWith("jpeg"))
                            {

                                //UPLOAD NOT EMPTY
                                //CHANGE THE PASSPORT NAME
                                string passName = model.Email.Replace(".", "").Replace("@", "") + Path.GetExtension(model.PassportUpload.FileName);


                                string pathx = Server.MapPath("~/Uploads/Passports/" + passName);
                                model.PassportUpload.SaveAs(pathx);

                                //CONVERT IMAGE OBJECT TO BYTE ARRAY
                                if (pathx != null)
                                {
                                    using (Image img = Image.FromFile(pathx))
                                    {
                                        var ms = new MemoryStream();

                                        img.Save(ms, ImageFormat.Jpeg);

                                        var bytes = ms.ToArray();

                                        // NOW SAVE BYTE 
                                        if (bytes.Length > 0)
                                        {
                                            model.Passport = bytes;
                                            con.ApplicantProfiles.Add(model);
                                            con.SaveChanges();
                                            ViewBag.Msg = "User and profile account created successfully!";
                                            return View();
                                        }
                                        else
                                        {
                                            msg = "Image conversion failed!";
                                        }
                                    }
                                }
                                else
                                    msg = "Applicant Passport Upload counld not be completed successfully" +
                                        " because, the image renaming failed!";

                                if (System.IO.File.Exists(pathx))
                                    System.IO.File.Delete(pathx);

                            }
                            else
                            {
                                //IMAGE NOT VALID
                                msg = "Invalid Image format or Image was not selected!";
                            }
                            ViewBag.Msg = msg;
                            return View(model);
                        }
                        else
                        {
                            //AddErrors(result);
                            msg = "Sorry, User account could not be saved successfully!";
                        }
                        ViewBag.Msg = msg;
                        return View(model);
                    }
                    else if (chkUserExist != null && chkApplicantExist == null)
                    {
                        //  -   -   ONLY APPLICANTPROFILE IS CREATED

                        ////CHECK IF UPLOAD FILE IS EMPTY
                        if (model.PassportUpload == null || model.PassportUpload.ContentLength < 0)
                        {
                            msg = "Please select a file, Try again!";
                            ViewBag.Msg = msg;
                            return View(model);
                        }
                        else if (model.PassportUpload != null && model.PassportUpload.ContentLength > 0 &&
                            model.PassportUpload.FileName.ToLower().EndsWith("jpg") ||
                            model.PassportUpload.FileName.ToLower().EndsWith("png") ||
                            model.PassportUpload.FileName.ToLower().EndsWith("jpeg"))
                        {

                            //UPLOAD NOT EMPTY
                            //CHANGE THE PASSPORT NAME
                            string passName = model.Email.Replace(".", "").Replace("@", "") + Path.GetExtension(model.PassportUpload.FileName);


                            string pathx = Server.MapPath("~/Uploads/Passports/" + passName);
                            model.PassportUpload.SaveAs(pathx);

                            //CONVERT IMAGE OBJECT TO BYTE ARRAY
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
                                        model.Passport = bytes;
                                        con.ApplicantProfiles.Add(model);
                                        con.SaveChanges();
                                        ViewBag.Msg = "User and profile account created successfully!";
                                        return View();
                                    }
                                    else
                                    {
                                        msg = "Image conversion failed!";
                                    }
                                }
                            }
                            else
                                msg = "Applicant Passport Upload counld not be completed successfully" +
                                    " because, the image renaming failed!";

                            if (System.IO.File.Exists(pathx))
                                System.IO.File.Delete(pathx);
                        }
                        else
                        {
                            //IMAGE NOT VALID
                            msg = "Invalid Image format or Image was not selected!";
                        }
                        ViewBag.Msg = msg;
                        return View(model);
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
                            dbObj.ResidentialCountry = model.ResidentialCountry;
                            dbObj.PermanentResident = model.PermanentResident;
                            dbObj.PreferredJobLocation = model.PreferredJobLocation;
                            dbObj.EmailNotification = model.EmailNotification;
                            dbObj.Nationality = model.Nationality;
                            dbObj.Skills = model.Skills;
                            dbObj.UserId = model.UserId;

                            ////CHECK IF THERE'S NEW PASSPORT
                            if (model.PassportUpload != null && model.PassportUpload.ContentLength > 0 &&
                               model.PassportUpload.FileName.ToLower().EndsWith("jpg") ||
                               model.PassportUpload.FileName.ToLower().EndsWith("png") ||
                               model.PassportUpload.FileName.ToLower().EndsWith("jpeg"))
                            {

                                string passName = model.Email.Replace(".", "").Replace("@", "") + Path.GetExtension(model.PassportUpload.FileName);


                                string pathx = Server.MapPath("~/Uploads/Passports/" + passName);
                                model.PassportUpload.SaveAs(pathx);

                                //CONVERT IMAGE OBJECT TO BYTE ARRAY
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
                                            dbObj.Passport = bytes;
                                            con.SaveChanges();
                                            msg = "Applicant profile has been saved successfully!";
                                            ViewBag.Msg = msg;
                                            return View();
                                        }
                                        else
                                        {
                                            msg = "Image conversion failed!";
                                        }
                                    }
                                }
                                else
                                    msg = "Applicant Passport Upload counld not be completed successfully" +
                                        " because, the image renaming failed!";


                                if (System.IO.File.Exists(pathx))
                                    System.IO.File.Delete(pathx);

                                ViewBag.Msg = msg;
                                return View(model);

                            }
                            else
                            {
                                //  - SAVE WITHOUT NEW PASSPORT
                                con.SaveChanges();
                                msg = "Applicant profile updated successfully!";
                                return RedirectToAction("MyProfile", msg);
                            }

                        }
                        else
                        {
                            msg = "No record found!";
                            return RedirectToAction("MyProfile", msg);

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
            }
            else
            {
                msg = "OOOPS, PLEASE PROVIDE ALL VALUES!";
            }

            //return Json(msg, JsonRequestBehavior.AllowGet);
            return RedirectToAction("MyProfile", msg);
        }

        [AllowAnonymous]
        public ActionResult AddEditApplicantProfile(string ID)
        {
            ApplicantProfile model = new ApplicantProfile();
            string msg = "";

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
                        model.ResidentialCountry = obj.ResidentialCountry;
                        model.PermanentResident = obj.PermanentResident;
                        model.PreferredJobLocation = obj.PreferredJobLocation;
                        model.EmailNotification = obj.EmailNotification;
                        model.Nationality = obj.Nationality;
                        model.Skills = obj.Skills;
                        model.UserId = obj.UserId;

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

        //[Authorize(Roles = "SuperAdmin")]
        //public JsonResult DeleteApplicant(string ID)
        //{

        //    bool result = false;
        //    if (ID != null)
        //    {
        //        var objDel = con.StateBranches.SingleOrDefault(o => o.Id == ID);
        //        if (objDel != null)
        //        {
        //            con.StateBranches.Remove(objDel);
        //            con.SaveChanges();
        //            result = true;
        //        }
        //    }
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

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
    }
}