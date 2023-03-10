using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Recruitment_Portal_System.Models
{
    public class UploadEmployeePassportViewModel
    {
        public string PassportUrl { get; set; }
        [Required]
        [Display(Name = "Applicant Passport")]
        public HttpPostedFileBase PassportUpload { get; set; }

    }
}
