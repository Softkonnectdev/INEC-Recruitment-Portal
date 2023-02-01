using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Recruitment_Portal_System.Models;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
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
                        return View("CreateProfile", newApplicant);
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
                            UtilityHelpers utility = new UtilityHelpers();
                            ViewBag.GenderList = utility.GetGenders();
                            msg = "OOOPS, PLEASE PROVIDE ALL VALUES!";
                            return View(model);
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
        public ActionResult UploadPassport(UploadPassportViewModel model)
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

                    string passName = user.Id + System.IO.Path.GetExtension(model.PassportUpload.FileName);

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


        [HttpGet]
        [Authorize(Roles = "Applicant")]
        public ActionResult UploadCV()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Applicant")]
        [ValidateAntiForgeryToken]
        public ActionResult UploadCV(UploadCVViewModel model)
        {
            try
            {
                string msg = "";
                // CHECK IF IMAGE1 UPLOAD FILE IS EMPTY
                if (model.CVUpload == null || model.CVUpload.ContentLength < 0 && model.CoverPage.Count() > 100)
                {
                    msg = "Please select a file, Try again!";
                    ViewBag.Msg = msg;

                    return View();
                }
                else if (model.CVUpload != null && model.CVUpload.ContentLength > 0 &&
                    model.CVUpload.FileName.ToLower().EndsWith("pdf"))
                {
                    //UPLOAD NOT EMPTY
                    string email = User.Identity.Name;

                    var user = con.Users.FirstOrDefault(x => x.Email == email);

                    string passName = user.Id + System.IO.Path.GetExtension(model.CVUpload.FileName);

                    string pathx = Server.MapPath("~/Uploads/CV/" + passName);
                    model.CVUpload.SaveAs(pathx);


                    Stream str = model.CVUpload.InputStream;
                    BinaryReader Br = new BinaryReader(str);
                    Byte[] FileDet = Br.ReadBytes((Int32)str.Length);

                    string cvText = GetTextFromPDF(pathx);

                    //POPULATE APPLICANT CV TABLE
                    var appCv = new CV()
                    {
                        ApplicantID = user.Id,
                        FileContent = FileDet,
                        CoverPage = model.CoverPage,
                        CVText = cvText
                    };


                    // CHECK IF RECORD EXIST WITH SAME APPLICANT
                    var cvBefore = con.CVs.FirstOrDefault(x => x.ApplicantID == user.Id);
                    if (cvBefore != null)
                    {
                        //UPDATE
                        cvBefore.CoverPage = model.CoverPage ?? cvBefore.CoverPage;
                        cvBefore.FileContent = FileDet;
                        cvBefore.CVText = cvText;
                    }
                    else
                        con.CVs.Add(appCv);

                    int isSaved = con.SaveChanges();
                    if (isSaved > 0)
                    {
                        TempData["Msg"] = "CV uploaded ssuccessfully!";
                        return RedirectToAction("Index", "Inec");

                    }

                    if (System.IO.File.Exists(pathx))
                        System.IO.File.Delete(pathx);
                }
                else
                {
                    msg = "Please select pdf file only, Try again!";
                    ViewBag.Msg = msg;

                    return View();
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


        [HttpGet]
        public FileResult DownloadCV(string id)
        {
            if (id != null)
            {
                var cv = con.CVs.FirstOrDefault(x => x.ApplicantID == id);
                if (cv != null)
                {
                    return File(cv.FileContent, "application/pdf", cv.ApplicantProfile.Email + ".pdf");
                }
            }
            else
                HttpNotFound();

            return null;
        }


        [Authorize(Roles = "Applicant")]
        [HttpGet]
        public ActionResult WorkExperience(string UserID = null)
        {
            string msg = "";
            try
            {
                var userEmail = User.Identity.Name;
                var applicant = con.ApplicantProfiles.FirstOrDefault(x => x.Email == userEmail);

                if (applicant == null)
                {
                    msg = "Please Register as Applicant and try again later!";
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }

                var allWorkExperience = con.ApplicantWorkExperiences.Where(x => x.ApplicantID == applicant.UserId).ToList();
                if (allWorkExperience != null && allWorkExperience.Count > 0)
                {
                    ViewBag.WorkExperience = allWorkExperience;
                    ViewBag.WorkExperienceCount = allWorkExperience.Count();
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.msg = Session["csmsg"].ToString() + " " + ex.InnerException.Message.ToString();
                else
                    ViewBag.msg = Session["csmsg"].ToString() + " " + ex.Message.ToString();

                return View();
            }
            return View();
        }


        [Authorize(Roles = "Applicant")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult WorkExperience(ApplicantWorkExperience model)
        {
            string msg = "";


            var dbObj = con.ApplicantWorkExperiences.FirstOrDefault(o => o.Id == model.Id);
            try
            {
                var userEmail = User.Identity.Name;
                var applicant = con.ApplicantProfiles.FirstOrDefault(x => x.Email == userEmail);

                if (applicant == null)
                {
                    msg = "Please Register as Applicant and try again later!";
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }

                if (dbObj != null)
                {
                    dbObj.Company = model.Company;
                    dbObj.Position = model.Position;
                    dbObj.StartedDate = model.StartedDate;
                    dbObj.TerminationDate = model.TerminationDate;
                    dbObj.ApplicantID = dbObj.ApplicantID;
                    dbObj.YearsofExperience = model.YearsofExperience;
                    con.SaveChanges();
                    msg = "Work Experience has been update successfully!";
                }
                else
                {

                    ApplicantWorkExperience objWorkEx = new ApplicantWorkExperience()
                    {
                        Company = model.Company,
                        Position = model.Position,
                        StartedDate = model.StartedDate,
                        TerminationDate = model.TerminationDate,
                        YearsofExperience = model.YearsofExperience,
                        ApplicantID = applicant.UserId
                    };
                    if (ModelState.IsValid)
                    {
                        con.ApplicantWorkExperiences.Add(objWorkEx);
                        con.SaveChanges();
                        msg = "Work Experience has been added successfully!";
                    }
                    else
                    {
                        msg = "OOOPS, PLEASE PROVIDE ALL VALUES!";
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

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Applicant")]
        public ActionResult AddEditWorkExperience(string ID)
        {
            string msg = "";

            ApplicantWorkExperience model = new ApplicantWorkExperience();
            try
            {

                var userEmail = User.Identity.Name;
                var applicant = con.ApplicantProfiles.FirstOrDefault(x => x.Email == userEmail);

                if (applicant == null)
                {
                    msg = "Please Register as Applicant and try again later!";
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }

                if (ID != "")
                {
                    var obj = con.ApplicantWorkExperiences.SingleOrDefault(o => o.Id == ID);
                    if (obj != null)
                    {
                        model.Id = obj.Id;
                        model.Company = obj.Company;
                        model.Position = obj.Position;
                        model.StartedDate = obj.StartedDate;
                        model.TerminationDate = obj.TerminationDate;
                        model.YearsofExperience = obj.YearsofExperience;

                    }
                }
                else
                    model.ApplicantID = applicant.UserId;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    msg = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN! \n" + ex.InnerException.Message.ToString();
                else
                    msg = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN! \n" + ex.Message.ToString();

                TempData["Msg"] = msg;
                return RedirectToAction("Index", "Home");
            }

            return PartialView("AddEditWorkExperience", model);
        }


        [Authorize(Roles = "Applicant")]
public JsonResult DeleteWorkExperience(string ID)
        {

            string msg = "";
            if (ID != null)
            {
                var objDel = con.ApplicantWorkExperiences.SingleOrDefault(o => o.Id == ID);
                if (objDel != null)
                {
                    con.ApplicantWorkExperiences.Remove(objDel);
                    con.SaveChanges();
                    msg = "1";
                }
                else
                {
                    msg = "0";
                }
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }



        #endregion

        #region METHOD TO ADD INTO ROLE
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


        private string GetTextFromPDF(string uploadedFile)
        {
            StringBuilder text = new StringBuilder();
            using (PdfReader reader = new PdfReader(uploadedFile))
            {
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                }
            }

            return text.ToString();
        }
    }
}