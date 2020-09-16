using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PayDel.Data.Models
{
    public class User : IdentityUser
    {

        [Required]
        [StringLength(100, MinimumLength = 0)]
        public string Name { get; set; }

        //[Required]
        //public byte[] PasswordSalt { get; set; }
        public bool Gender { get; set; }
        public DateTime DateOfBirth { get; set; }

        [StringLength(50, MinimumLength = 0)]
        public string City { get; set; }

        [StringLength(500, MinimumLength = 0)]
        public string Address { get; set; }

        [StringLength(10, MinimumLength = 10)]
        public string PostalCode { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public DateTime LastActive { get; set; }

        [Required]
        public bool Confirmed { get; set; }//Statut

        [StringLength(150, MinimumLength = 0)]
        public string ImageUrl { get; set; }

        public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<BankCard> BankCards { get; set; }
        public virtual ICollection<MyToken> MyTokens { get; set; }
        public virtual ICollection<Wallet> Wallets { get; set; }
        public virtual ICollection<EasyPay> EasyPays { get; set; }
    }
}
