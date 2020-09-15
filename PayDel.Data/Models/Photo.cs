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
        [StringLength(150, MinimumLength = 1)]
        public string Url { get; set; }

        [StringLength(150, MinimumLength = 0)]
        public string Alt { get; set; }

        [StringLength(350, MinimumLength = 0)]
        public string Description { get; set; }

        [Required]
        public bool IsMain { get; set; }

        [Required]
        public string UserId { get; set; }
        public User User { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

    }
}
