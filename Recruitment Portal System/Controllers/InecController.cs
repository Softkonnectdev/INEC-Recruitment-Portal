using Recruitment_Portal_System.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Recruitment_Portal_System.Controllers
{
    public class InecController : Controller
    {
        ApplicationDbContext con;
        public InecController()
        {
            con = new ApplicationDbContext();
        }

        [Authorize(Roles = "Applicant")]
        [HttpGet]
        public ActionResult Index(string jobKeyWord)
        {
            ViewBag.CurrencyFmt = CultureInfo.CreateSpecificCulture("NG-NG");

            if (TempData["Msg"] != null)
            {
                ViewBag.Msg = TempData["Msg"].ToString();
            }
            List<Job> jobs = new List<Job>();

            if (jobKeyWord != null)
            {

                List<Job> jobsList = con.Jobs.Where(x => x.Title.Contains(jobKeyWord)
                                              || x.Description.Contains(jobKeyWord)
                                              || x.JobCategory.Title.Contains(jobKeyWord)
                                              ).ToList();

                jobs = jobsList;

            }
            else
            {
                jobs = con.Jobs.ToList();
            }
            return View(jobs);
        }
        [Authorize(Roles = "Applicant")]

        public ActionResult Details(string id)
        {
            ViewBag.CurrencyFmt = CultureInfo.CreateSpecificCulture("NG-NG");

            Job job = con.Jobs.FirstOrDefault(x => x.Id == id);
            if (job == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(job);
            }
        }

        [Authorize(Roles = "Applicant")]
        public ActionResult ApplyJobNow(string Id)
        {
            if (Id != null || Id != "" && User.Identity.Name != "")
            {
                var userEmail = User.Identity.Name;
                var applicant = con.ApplicantProfiles.Where(x => x.Email == userEmail).FirstOrDefault();
                var job = con.Jobs.Where(x => x.Id == Id).FirstOrDefault();

                if (applicant != null && job != null)
                {
                    var newApplication = new JobApplication()
                    {
                        JobID = job.Id,
                        ApplicantID = applicant.UserId
                    };
                    return PartialView("ApplyJobNow", newApplication);
                }
            }
            TempData["Msg"] = "The process could not coompleted, validation FAILED, please try again later!";
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Applicant")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ApplyNow(JobApplication model)
        {
            string msg = "";

            //  CHECK IF APPLICANT HAS CV
            var cv = con.CVs.FirstOrDefault(x => x.ApplicantID == model.ApplicantID);
            if (cv != null)
            {
                if (model != null && ModelState.IsValid)
                {
                    con.JobApplications.Add(model);

                    string jobApp_ID = model.Id;
                    var appliedB4 = con.JobApplications.FirstOrDefault(x => x.ApplicantID == model.ApplicantID
                                                        && x.JobID == model.JobID);
                    if (appliedB4 != null)
                    {
                        msg = "Please you have already applied for this job!";
                        return Json(msg, JsonRequestBehavior.AllowGet);
                    }

                    int isSaved = con.SaveChanges();
                    if (isSaved > 0)
                    {
                        //CREATE USER APPLICATION HISTORY

                        var userAppHis = new ApplicationHistory()
                        {
                            JobAppID = model.Id,
                            Status = true
                        };

                        con.ApplicationHistories.Add(userAppHis);
                        con.SaveChanges();


                        //  GET LIST OF LOOK UP FROM JOB

                        List<string> lookup = new List<string>();
                        //  GET JOB
                        var job = con.Jobs.FirstOrDefault(x => x.Id == model.JobID);
                        if (job != null)
                        {
                            string[] dbLookUpArr = job.LookingFor.Split(',');
                            foreach (var item in dbLookUpArr)
                            {
                                lookup.Add(item);
                            }

                            lookup.Add(job.Title);
                            lookup.Add(job.JobCategory.Title);

                            int totMatch = 0;
                            string merits = null;
                            string wrkExp = null;

                            //GET WORK EXPERIENCE CRITERIALS
                            var appWorkExp = con.ApplicantWorkExperiences.Where(x => x.ApplicantID == model.ApplicantID);
                            if (appWorkExp != null)
                            {
                                foreach (var i in appWorkExp)
                                {
                                    wrkExp += i;
                                }
                            }

                            foreach (var item in dbLookUpArr)
                            {
                                if (cv.CVText.Contains(item))
                                {
                                    merits += item + ", ";
                                    totMatch += 1;
                                }
                                else if (wrkExp != null && wrkExp.Contains(item))
                                {
                                    merits += ", " + item;
                                    totMatch += 1;
                                }
                            }

                            if (totMatch > 0)
                            {
                                //  ADD APPLICANT TO SHORTLISTED
                                var shortListCandidate = new ShortListedCandidate()
                                {
                                    ApplicantEmail = cv.ApplicantProfile.Email,
                                    ApplicantMerit = merits,
                                    ApplicantID = cv.ApplicantID,
                                    MeritScore = totMatch,
                                    JobID = job.Id
                                };
                                //  CHECK IF APPLICANT ALREADY EXIST BEFORE SAVING

                                var appExist = con.ShortListedCandidates.FirstOrDefault(x => x.ApplicantEmail == cv.ApplicantProfile.Email
                                                                         && x.JobID == job.Id);
                                if (appExist == null)
                                {
                                    //  NOW ADD TO SHORT LISTED CANDIDATE
                                    con.ShortListedCandidates.Add(shortListCandidate);
                                    con.SaveChanges();
                                }

                            }


                            msg = "Congratulations, you've successfully submitted your" +
                        " application!";
                        }
                        else
                            msg = "Sorry, job does not exist anymore in our database!";

                    }
                    //return RedirectToAction("Index");
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                msg = "Application failed, please try again shortly!";
            }
            else
                msg = "Application failed, please upload your CV and try again!";

            return Json(msg, JsonRequestBehavior.AllowGet);
            //return View(model);
        }


    }
}