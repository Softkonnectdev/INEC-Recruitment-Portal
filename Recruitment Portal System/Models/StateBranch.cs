using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Recruitment_Portal_System.Models
{
    public class StateBranch : BaseEntity
    {
        [Required(ErrorMessage = "Name is NULL")]
        public string Name
        {
            get; set;
        }
                
        [Required(ErrorMessage ="Address is NULL")]
        public string Address
        {
            get; set;
        }

        [DataType(DataType.MultilineText)]
        public string Description
        {
            get; set;
        }



        public virtual ICollection<Job> Jobs
        {
            get; set;
        }
    }
}