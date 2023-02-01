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

        [Required(ErrorMessage = "CV Merits")]
        public string CVText
        {
            get; set;
        }

        public byte[] FileContent { get; set; }

        [Required(ErrorMessage = "Cover Page is NULL")]
        [DataType(DataType.MultilineText)]
        public string CoverPage
        {
            get; set;
        }

        public virtual ApplicantProfile ApplicantProfile { get; set; }
    }
}