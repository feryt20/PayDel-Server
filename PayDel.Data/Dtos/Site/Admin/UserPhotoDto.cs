using System;
using System.Collections.Generic;
using System.Text;

namespace PayDel.Data.Dtos.Site.Admin
{
    public class UserPhotoDto
    {
        public string Url { get; set; }
        public string Alt { get; set; }
        public string Description { get; set; }
        public bool IsMain { get; set; }
    }
}
