using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PayDel.Data.Models
{
    public class BaseEntity<T>
    {
        public BaseEntity()
        {
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }

        [Key]
        public T Id { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public DateTime DateModified { get; set; }

        public bool IsDeleted { get; set; }
    }
}
