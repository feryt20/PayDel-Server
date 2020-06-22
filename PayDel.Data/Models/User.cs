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
        public string Name { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        public string Gender { get; set; }
        public string DateOfBith { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public bool Confirmed { get; set; }//Statut
        public string ImageUrl { get; set; }
        public ICollection<Photo> Photos { get; set; }
        public ICollection<BankCard> BankCards { get; set; }
        //public string Created { get; set; }
        //public string Created { get; set; }

    }
}
