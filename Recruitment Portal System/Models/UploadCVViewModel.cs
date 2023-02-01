using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Recruitment_Portal_System.Models
{
    public class UploadCVViewModel
    {
        [Required]
        [Display(Name = "Applicant CV")]
        public HttpPostedFileBase CVUpload { get; set; }

        [Required(ErrorMessage = "Cover Page is NULL")]
        [DataType(DataType.MultilineText)]
        public string CoverPage
        {
            get; set;
        }
        public byte[] FileContent { get; set; }

    }
}
