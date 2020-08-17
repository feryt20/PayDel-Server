using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace PayDel.Data.Models
{
    public class Role : IdentityRole
    {

        public Role() : base() { }
        public Role(string name) : base(name) { }
       // public ICollection<UserRole> UserRoles { get; set; }
    }

}
