using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recruitment_Portal_System.Models
{
    public class Job : BaseEntity
    {
        [ForeignKey("JobCategory")]
        [Display(Name = "Job Category")]
        [Required(ErrorMessage = "Job Category ID is NULL")]
        public string JobCategoryID
        {
            get; set;
        }

        [Display(Name = "Job Title")]
        [Required(ErrorMessage = "Job Title is NULL")]
        public string Title
        {
            get; set;
        }

        [ForeignKey("JobBranchState")]
        [Display(Name = "Job State")]
        [Required(ErrorMessage = "Job State is NULL")]
        public string JobStateBranchID
        {
            get; set;
        }

        [Display(Name = "Job Type")]
        [Required(ErrorMessage = "Job Type is NULL")]
        public string JobType
        {
            get; set;
        }


        [Required(ErrorMessage = "Description is NULL")]
        [DataType(DataType.MultilineText)]
        [StringLength(500, ErrorMessage = "Not More than 500 characters!")]
        public string Description
        {
            get; set;
        }

        public bool Status
        {
            get; set;
        }

        [Required(ErrorMessage = "Salary is NULL")]
        [DataType(DataType.Currency)]
        public decimal Salary
        {
            get; set;
        }


        public virtual ICollection<ShortListedCandidate> ShortListedCandidates
        {
            get; set;
        }
        public virtual JobCategory JobCategory
        {
            get; set;
        }
        public virtual StateBranch JobBranchState
        {
            get; set;
        }
        public virtual ICollection<JobApplication> JobApplications
        {
            get; set;
        }

    }
}