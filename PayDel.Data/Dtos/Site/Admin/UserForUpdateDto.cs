using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PayDel.Data.Dtos.Site.Admin
{
    public class UserForUpdateDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public bool Gender { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public string City { get; set; }
        [Required]
        public string Address { get; set; }
        public string PostalCode { get; set; }
    }

}
