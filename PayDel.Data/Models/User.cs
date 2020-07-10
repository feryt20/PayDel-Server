using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PayDel.Data.Models
{
    public class User : BaseEntity<string>
    {
        public User()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Required]
        [StringLength(100, MinimumLength = 0)]
        public string Name { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(11, MinimumLength = 11)]
        public string PhoneNumber { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public byte[] PasswordSalt { get; set; }
        public bool Gender { get; set; }
        public DateTime DateOfBith { get; set; }

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

        public ICollection<Photo> Photos { get; set; }
        public ICollection<BankCard> BankCards { get; set; }
        //public string Created { get; set; }
        //public string Created { get; set; }

    }
}
