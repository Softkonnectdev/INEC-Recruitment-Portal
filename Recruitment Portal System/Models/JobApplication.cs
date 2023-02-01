using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Recruitment_Portal_System.Models
{
    public class JobApplication : BaseEntity
    {
        [ForeignKey("ApplicantProfile")]
        [Required(ErrorMessage = "Applicant Email is NULL")]
        public string ApplicantID
        {
            get; set;
        }

        [ForeignKey("Job")]
        [Required(ErrorMessage = "Job is NULL")]
        public string JobID
        {
            get; set;
        }

        [Required(ErrorMessage = "CoverPage is NULL")]
        [DataType(DataType.MultilineText)]
        public string ApplicantCoverPage
        {
            get; set;
        }


        public virtual Job Job
        {
            get; set;
        }

        public virtual ApplicantProfile ApplicantProfile
        {
            get; set;
        }
        //public virtual ShortListedCandidate ShortListedCandidate 
        //{
        //    get; set;
        //}
        public virtual ApplicationHistory ApplicationHistory
        {
            get; set;
        }


       
    }
}