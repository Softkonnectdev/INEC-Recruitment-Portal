using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Recruitment_Portal_System.Models
{
    public class ShortListedCandidate
    {
        [Key]
        [Required(ErrorMessage = "Job Application is NULL")]
        [ForeignKey("JobApplication")]
        public string JobApplicationID
        {
            get; set;
        }

        [Required(ErrorMessage = "Applicant Email is NULL")]
        public string ApplicantEmail
        {
            get; set;
        }
        
        public DateTime ShortListedOn
        {
            get; set;
        }

        [Required(ErrorMessage = "Applicant Merit is NULL")]
        public string ApplicantMerit 
        {
            get; set;
        }

        public virtual JobApplication JobApplication  { get; set; }

        public ShortListedCandidate()
        {
            this.ShortListedOn = DateTime.Now;
        }
    }
}