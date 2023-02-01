using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Recruitment_Portal_System.Models
{
    public class ShortListedCandidate : BaseEntity
    {
        [Required(ErrorMessage = "Job ID is NULL")]
        [ForeignKey("Job")]
        public string JobID
        {
            get; set;
        }

        [Required(ErrorMessage = "Applicant is NULL")]
        [ForeignKey("ApplicantProfile")]
        public string ApplicantID
        {
            get; set;
        }
               
        [Required(ErrorMessage = "Applicant Email is NULL")]
        public string ApplicantEmail
        {
            get; set;
        }

        [Required(ErrorMessage = "Applicant Merit Score")]
        public int MeritScore
        {
            get; set;
        }

        [Required(ErrorMessage = "Applicant Merit is NULL")]
        public string ApplicantMerit 
        {
            get; set;
        }

        public virtual ApplicantProfile ApplicantProfile { get; set; }
        public virtual Job Job  { get; set; }

       
    }
}