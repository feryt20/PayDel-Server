using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PayDel.Data.Models
{
    public class BankCard : BaseEntity<string>
    {
        public BankCard()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string BankName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string OwnerName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Shaba { get; set; }

        [Required]
        [StringLength(16,MinimumLength =16)]
        public string CardNumber { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string HesabNumber { get; set; }

        [Required]
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}
