using System;
using System.Collections.Generic;
using System.Text;

namespace PayDel.Data.Dtos.Site.Admin
{
    public class UserProfileDto
    {
       
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public bool Gender { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastActive { get; set; }
        public bool Confirmed { get; set; }//Statut
        public string ImageUrl { get; set; }

        public ICollection<UserPhotoDto> Photos { get; set; }
        public ICollection<UserBankCardDto> BankCards { get; set; }
    }
}
