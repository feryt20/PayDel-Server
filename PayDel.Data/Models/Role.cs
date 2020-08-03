using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace PayDel.Data.Models
{
    public class Role : IdentityRole
    {
        

        public ICollection<UserRole> UserRoles { get; set; }
    }

}
