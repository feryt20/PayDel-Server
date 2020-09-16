using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PayDel.Data.Models
{
    public class VerificationCode : BaseEntity<string>
    {
        public VerificationCode()
        {
            Id = Guid.NewGuid().ToString();
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }
        [Required]
        [StringLength(5, MinimumLength = 5)]
        public string Code { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }
        [Required]
        public DateTime RemoveDate { get; set; }
    }
}
