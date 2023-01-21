﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Recruitment_Portal_System.Models
{
    public class ApplicantProfile
    {
        [Key]
        [ForeignKey("User")]
        public string UserId
        {
            get; set;
        }

        [Required(ErrorMessage = "Surname is NULL")]
        public string SurName
        {
            get; set;
        }

        [Required(ErrorMessage = "FirstName is NULL")]
        public string FirstName
        {
            get; set;
        }
        [Required(ErrorMessage = "MiddleName is NULL")]
        public string MiddleName
        {
            get; set;
        }
        [Required(ErrorMessage = "Email is NULL")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address")]
        public string Email
        {
            get; set;
        }
        [Required(ErrorMessage = "Password is NULL")]
        [DataType(DataType.Password)]
        public string Password
        {
            get; set;
        }
        [Required(ErrorMessage = "Phone is NULL")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Phone Number")]
        [StringLength(11, ErrorMessage = "Not more than 11 characters!")]
        public string Phone
        {
            get; set;
        }

        [Required(ErrorMessage = "DOB is NULL")]
        public DateTime DOB
        {
            get; set;
        }

        [Required(ErrorMessage = "Gender is NULL")]
        public string Gender
        {
            get; set;
        }

        [Required(ErrorMessage = "Permanent Address is NULL")]
        [Display(Name = "Permanent Resident")]
        public string PermanentResident
        {
            get; set;
        }
        [Required(ErrorMessage = "LGA is NULL")]
        public string LGAOrigin
        {
            get; set;
        }
        [Required(ErrorMessage = "State is NULL")]
        public string StateOrigin
        {
            get; set;
        }

        [Required(ErrorMessage = "Residential Country is NULL")]
        [Display(Name = "Residential Country")]
        public string ResidentialCountry
        {
            get; set;
        }

        [Required(ErrorMessage = "Nationality is NULL")]
        public string Nationality
        {
            get; set;
        }

        public string Skills
        {
            get; set;
        }

        [Required(ErrorMessage = "Passport is NULL")]
        public byte[] Passport
        {
            get; set;
        }

        [Required(ErrorMessage = "Preferred Job Location is NULL")]
        public string PreferredJobLocation
        {
            get; set;
        }

        [Required(ErrorMessage = "Nationality is NULL")]
        public bool EmailNotification
        {
            get; set;
        }

        public DateTime CreatedOn
        {
            get; set;
        }

        [NotMapped]
        public HttpPostedFileBase PassportUpload { get; set; }



        public virtual ApplicationUser User { get; set; }
        public virtual ApplicantWorkExperience ApplicantWorkExperience { get; set; }
        public virtual CV CV { get; set; }
        public virtual ICollection<JobApplication> JobApplications { get; set; }

        public ApplicantProfile()
        {
            this.CreatedOn = DateTime.Now;
        }
    }
}