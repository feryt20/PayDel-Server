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
        public string BankName { get; set; }
        [Required]
        public string Shaba { get; set; }
        [Required]
        [Range(16,16)]
        //[StringLength(16,MinimumLength =16)]
        public string CardNumber { get; set; }
        [Required]
        public string HesabNumber { get; set; }

        [Required]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
