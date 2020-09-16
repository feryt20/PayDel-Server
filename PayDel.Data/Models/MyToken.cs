using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PayDel.Data.Models
{
    public class MyToken : BaseEntity<string>
    {
        public MyToken()
        {
            Id = Guid.NewGuid().ToString();
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }

        [Required]
        public string ClientId { get; set; }
        [Required]
        public string Ip { get; set; }
        [Required]
        public string Value { get; set; }
        [Required]
        public DateTime ExpireTime { get; set; }
        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
