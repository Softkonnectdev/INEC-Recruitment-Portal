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
        public ActionResult ApplyNow(string Id)
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
                    return View(newApplication);
                }
            }
            TempData["Msg"] = "The process could not coompleted, validation FAILED, please try again later!";
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Applicant")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApplyNow(JobApplication model)
        {
            if (model != null)
            {

            }
            return View();
        }
    }
}