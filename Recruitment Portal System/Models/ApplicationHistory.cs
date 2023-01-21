using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recruitment_Portal_System.Models
{
    public class ApplicationHistory
    {
        [Key]
        [ForeignKey("JobApplication")]
        [Required(ErrorMessage = "Job ID is NULL")]
        public string JobAppID
        { 
            get; set;
        }

        [Required(ErrorMessage = "Application Date is requiredW")]
        [DataType(DataType.DateTime)]
        public DateTime AppliedOn
        {
            get; set;
        }


        [Required(ErrorMessage = "Application Date is requiredW")]
        public bool Status
        {
            get; set;
        }






        public virtual JobApplication JobApplication { get; set; }


        public ApplicationHistory()
        {
            this.AppliedOn = DateTime.Now;
        }

    }
}