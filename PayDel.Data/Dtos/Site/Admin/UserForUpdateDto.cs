﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PayDel.Data.Dtos.Site.Admin
{
    public class UserForUpdateDto
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public bool Gender { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
    }

}
