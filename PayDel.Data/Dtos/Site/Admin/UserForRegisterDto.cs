using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PayDel.Data.Dtos.Site.Admin
{
    public class UserForRegisterDto
    {
        [Required]
        [EmailAddress(ErrorMessage ="ایمیل وارد شده صحیح نیست")]
        public string UserName { get; set; }

        [Required]
        [StringLength(20,MinimumLength =6,ErrorMessage ="پسورد باید بین 6 تا 20 کاراکترباشد")]
        public string Password { get; set; }


        [Required]
        public string Name { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "کد فعالسازی باید 5 رقمی باشد")]
        public string Code { get; set; }
    }
}
