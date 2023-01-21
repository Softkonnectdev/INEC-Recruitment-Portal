using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Recruitment_Portal_System.Models
{
    public class JobCategory : BaseEntity
    {
        [Required(ErrorMessage = "Title is NULL")]
        public string Title
        { get; set; }


        [Required(ErrorMessage = "Description is NULL")]
        [DataType(DataType.MultilineText)]
        public string Description
        {
            get; set;
        }


        public virtual ICollection<Job> Jobs { get; set; }
    }
}