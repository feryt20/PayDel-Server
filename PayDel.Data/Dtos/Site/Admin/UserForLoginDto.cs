using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PayDel.Data.Dtos.Site.Admin
{
    public class UserForLoginDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده صحیح نیست")]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public bool IsRemember { get; set; }

    }
}
