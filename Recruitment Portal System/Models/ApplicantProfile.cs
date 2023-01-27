using System;
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
        [Display(Name = "Sur Name")]
        public string SurName
        {
            get; set;
        }

        [Required(ErrorMessage = "FirstName is NULL")]
        [Display(Name = "First Name")]
        public string FirstName
        {
            get; set;
        }
        [Required(ErrorMessage = "MiddleName is NULL")]
        [Display(Name = "Middle Name")]
        public string MiddleName
        {
            get; set;
        }
        [Required(ErrorMessage = "Email is NULL")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address")]
        [Display(Name = "Email")]
        public string Email
        {
            get; set;
        }

        [DataType(DataType.Password)]
        [NotMapped]
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
        [DisplayFormat(DataFormatString = "dd-MM-yyyy", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
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
        [DataType(DataType.Text)]
        public string ResidentialAddress
        {
            get; set;
        }
        [Required(ErrorMessage = "LGA is NULL")]
        [Display(Name = "LGA of origin")]
        public string LGAOrigin
        {
            get; set;
        }
        [Required(ErrorMessage = "State is NULL")]
        [Display(Name = "State of origin")]
        public string StateOrigin
        {
            get; set;
        }
        
        public string Skills
        {
            get; set;
        }

        public byte[] Passport
        {
            get; set;
        }

        [Required(ErrorMessage = "Preferred Job Location is NULL")]
        [Display(Name = "Preferred Job Location")]
        public string PreferredJobLocation
        {
            get; set;
        }

        [Required(ErrorMessage = "Nationality is NULL")]
        [Display(Name = "Email Notification")]
        public bool EmailNotification
        {
            get; set;
        }

        public DateTime? CreatedOn
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