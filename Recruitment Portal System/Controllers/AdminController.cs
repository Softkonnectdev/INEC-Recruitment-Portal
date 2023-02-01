using Recruitment_Portal_System.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UBA_Network_Security_System.Models;

namespace Recruitment_Portal_System.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext con;
        public AdminController()
        {
            con = new ApplicationDbContext();
        }


        #region      Branch/State CRUD

        [HttpGet]
        public ActionResult StateBranch(string Msg)
        {

            if (Msg != null)
            {
                ViewBag.Msg = Msg.ToString();
            }

            try
            {
                var allStateBranches = con.StateBranches.ToList();
                if (allStateBranches != null && allStateBranches.Count > 0)
                {
                    ViewBag.AllStateBranches = allStateBranches;
                    ViewBag.StateBranchCount = allStateBranches.Count();
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
        public JsonResult StateBranch(StateBranch model)
        {
            string msg = "";

            if (ModelState.IsValid)
            {

                var dbObj = con.StateBranches.SingleOrDefault(o => o.Id == model.Id);
                try
                {
                    if (model.Id != null && dbObj != null)
                    {
                        dbObj.Name = model.Name;
                        dbObj.Description = model.Description;
                        dbObj.Address = model.Address;
                        con.SaveChanges();
                        msg = "State Branch has been updated successfully!";
                    }
                    else
                    {

                        StateBranch objStateBranch = new StateBranch()
                        {
                            Name = model.Name,
                            Description = model.Description,
                            Address = model.Address
                        };

                        con.StateBranches.Add(objStateBranch);
                        msg = "State Branch has been created successfully!";
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
        public ActionResult AddEditStateBranch(string ID)
        {
            StateBranch model = new StateBranch();
            try
            {
                if (ID != null)
                {
                    //string uId = userID();
                    var obj = con.StateBranches.SingleOrDefault(o => o.Id == ID);
                    if (obj != null)
                    {
                        model.Id = obj.Id;
                        model.Name = obj.Name;
                        model.Description = obj.Description;
                        model.Address = obj.Address;
                    }
                }
            }
            catch (Exception ex)
            {
                Session["grmsg"] = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN!";
                return RedirectToAction("StateBranch");
            }
            return PartialView("AddEditStateBranch", model);
        }


        public JsonResult DeleteStateBranch(string ID)
        {

            bool result = false;
            if (ID != null)
            {
                var objDel = con.StateBranches.SingleOrDefault(o => o.Id == ID);
                if (objDel != null)
                {
                    con.StateBranches.Remove(objDel);
                    con.SaveChanges();
                    result = true;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
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
                        msg = "Job Category has been updated successfully!";
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
                        msg = "Job Category has been created successfully!";
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
                if (ID != null && ID != "")
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

        #region      JOB CRUD

        [HttpGet]
        public ActionResult Job(string Msg)
        {
            ViewBag.CurrencyFmt = CultureInfo.CreateSpecificCulture("NG-NG");

            if (Msg != null)
            {
                ViewBag.Msg = Msg.ToString();
            }

            try
            {
                var allJobs = con.Jobs.ToList();
                if (allJobs != null && allJobs.Count > 0)
                {
                    ViewBag.AllJobs = allJobs;
                    ViewBag.JobCount = allJobs.Count();
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
        public JsonResult Job(Job model)
        {
            string msg = "";


            var dbObj = con.Jobs.FirstOrDefault(o => o.Id == model.Id);
            try
            {
                if (dbObj != null)
                {
                    dbObj.Title = model.Title;
                    dbObj.JobCategoryID = model.JobCategoryID ?? dbObj.JobCategoryID;
                    dbObj.JobStateBranchID = model.JobStateBranchID ?? dbObj.JobStateBranchID;
                    dbObj.JobType = model.JobType ?? dbObj.JobType;
                    dbObj.Description = model.Description;
                    dbObj.Salary = model.Salary;
                    dbObj.LookingFor = model.LookingFor;
                    dbObj.Status = model.Status;
                    con.SaveChanges();
                    msg = "Job has been update successfully!";
                }
                else
                {

                    Job objJob = new Job()
                    {
                        Title = model.Title,
                        JobCategoryID = model.JobCategoryID,
                        JobStateBranchID = model.JobStateBranchID,
                        JobType = model.JobType,
                        Description = model.Description,
                        Salary = model.Salary,
                        LookingFor = model.LookingFor,
                        Status = model.Status,
                    };
                    if (ModelState.IsValid)
                    {
                        con.Jobs.Add(objJob);
                        con.SaveChanges();
                        msg = "Job has been saved successfully!";
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

        [AllowAnonymous]
        public ActionResult AddEditJob(string ID)
        {
            Job model = new Job();
            try
            {
                if (ID != null)
                {
                    var obj = con.Jobs.SingleOrDefault(o => o.Id == ID);
                    if (obj != null)
                    {
                        model.Id = obj.Id;
                        model.Title = obj.Title;
                        model.Description = obj.Description;
                        model.Status = obj.Status;
                        model.Salary = obj.Salary;
                        model.LookingFor = obj.LookingFor;

                    }
                }
            }
            catch (Exception ex)
            {
                Session["grmsg"] = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN!";
                return RedirectToAction("StateBranch");
            }
            UtilityHelpers utilityHelpers = new UtilityHelpers();

            ViewBag.JobCategories = con.JobCategories.ToList();
            ViewBag.JobTypes = utilityHelpers.GetJobTypeList();
            ViewBag.StateBranches = con.StateBranches.ToList();


            return PartialView("AddEditJob", model);
        }

        public JsonResult DeleteJob(string ID)
        {

            bool result = false;
            if (ID != null)
            {
                var objDel = con.Jobs.SingleOrDefault(o => o.Id == ID);
                if (objDel != null)
                {
                    con.Jobs.Remove(objDel);
                    con.SaveChanges();
                    result = true;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region VIEW APPLICATION AND SHORTLISTED CANDIDATE

        [HttpGet]
        public ActionResult JobApplication(string id = null)
        {
            if (id == null)
            {
                ViewBag.Msg = "Please the selected job has no ID!";
                return RedirectToAction("Job");
            }
            var jobApp = con.JobApplications.Where(x => x.JobID == id).ToList();
            if (jobApp.Count > 0)
            {
                ViewBag.JobTitle = jobApp.FirstOrDefault().Job.Title;
                return View(jobApp);
            }
            ViewBag.Msg = "Please the selected job has no Job Application at the moment, try again later!";
            return RedirectToAction("Job");
        }

        [HttpGet]
        public ActionResult JobShortList(string id = null)
        {
            if (id == null)
            {
                ViewBag.Msg = "Please the selected job has no ID!";
                return RedirectToAction("Job");
            }
            var shortlisted = con.ShortListedCandidates.Where(x => x.JobID == id).ToList();
            if (shortlisted.Count > 0)
            {
                ViewBag.JobID = id;
                return View(shortlisted);
            }
            ViewBag.Msg = "Please the selected job has no shortlisted candidate at the moment, try again later!";
            return RedirectToAction("Job");
        }

        public JsonResult SceduleInterview(string ID = null)
        {
            string msg = "";

            if (ID == null)
            {
                msg = "Invalid ID!";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            var candidates = con.ShortListedCandidates.Where(x => x.JobID == ID).ToList();
            if (candidates.Count > 0)
            {
                foreach (var item in candidates)
                {
                    //SEND EMAIL HERE WHEN EMAIL SERVICE IS AVAILABLE
                    msg += "Interview mail has been sent to " + item.ApplicantEmail + ".\n";
                }
                
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemoveCandidate(string ID)
        {
            bool result = false;
            if (ID != null)
            {
                var objDel = con.ShortListedCandidates.SingleOrDefault(o => o.Id == ID);
                if (objDel != null)
                {
                    con.ShortListedCandidates.Remove(objDel);
                    con.SaveChanges();
                    result = true;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}