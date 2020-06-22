using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PayDel.Data.Models
{
    public class Photo : BaseEntity<string>
    {
        public Photo()
        {
            Id = Guid.NewGuid().ToString();
        }
        [Required]
        public string Url { get; set; }
        public string Alt { get; set; }
        public string Description { get; set; }
        [Required]
        public bool IsMain { get; set; }

        [Required]
        public string UserId { get; set; }
        public User User { get; set; }
        
    }
}
