using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PayDel.Data.Dtos.Site.Admin
{
    public class DocumentForCreateDto
    {
        [Required]
        public bool IsApproved { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 0)]
        public string Name { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 0)]
        public string NationalCode { get; set; }
       
        [Required]
        public DateTime BirthDay { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 0)]
        public string Address { get; set; }

        public IFormFile File { get; set; }
    }
}
