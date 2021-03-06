﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PayDel.Data.Dtos.Site.Admin
{
    public class UserForRegisterWithSocialDto
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Provider { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhotoUrl { get; set; }
    }
}
