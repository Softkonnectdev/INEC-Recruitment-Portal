using System;
using System.ComponentModel.DataAnnotations;
using Recruitment_Portal_System.Models.Utility;

namespace Recruitment_Portal_System.Models
{
    public abstract class BaseEntity
    {
        [Key]
        public string Id { get; set; }
        [Display(Name = "Creaeted Date")]
        public DateTime CreatedAt { get; set; }

        public BaseEntity()
        {
            var dateTime = DateTime.Now;

            Utilities util = new Utilities();
            var _id = util.RandomDigits(15);

            this.Id = _id;
            this.CreatedAt = dateTime;
        }
    }
}
