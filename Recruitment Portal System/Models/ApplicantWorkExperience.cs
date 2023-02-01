﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Recruitment_Portal_System.Models
{
    public class ApplicantWorkExperience : BaseEntity
    {
      
        [ForeignKey("ApplicantProfile")]
        [Required(ErrorMessage = "Applicant ID is NULL")]
        public string ApplicantID { get; set; }

        [Required(ErrorMessage = "Position is NULL")]
        public string Position
        {
            get; set;
        }

        [Required(ErrorMessage = "Company is NULL")]
        public string Company
        {
            get; set;
        }

        [Required(ErrorMessage = "Experience Years is NULL")]
        public int YearsofExperience
        {
            get; set;
        }

        [Required(ErrorMessage = "Date of employment is NULL")]
        [DisplayFormat(DataFormatString ="{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Started Date")]
        [DataType(DataType.Date)]
        public DateTime StartedDate
        {
            get; set;
        }
        [Required(ErrorMessage = "Termination date is NULL")]
        [Display(Name = "Termination Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime TerminationDate
        {
            get; set;
        }


        public virtual ApplicantProfile ApplicantProfile { get; set; }
    }
}