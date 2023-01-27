using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Recruitment_Portal_System.Models
{
    public class CV
    {
        [Key]
        [ForeignKey("ApplicantProfile")]
        [Required(ErrorMessage = "Applicant is NULL")]
        public string ApplicantID
        {
            get; set;
        }

        [Required(ErrorMessage = "CV Merit is NULL")]
        public string CVMerits
        {
            get; set;
        }

        [Required(ErrorMessage = "CV Path is NULL")]
        public string CVPath
        {
            get; set;
        }

        [Required(ErrorMessage = "Cover Page is NULL")]
        [DataType(DataType.MultilineText)]
        [StringLength(500, ErrorMessage = "Not more than 500 characters!")]
        public string CoverPage
        {
            get; set;
        }

        public virtual ApplicantProfile ApplicantProfile { get; set; }
    }
}