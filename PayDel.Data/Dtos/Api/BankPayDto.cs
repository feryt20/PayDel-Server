using PayDel.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PayDel.Data.Dtos.Api
{
    public class BankPayDto
    {
        public Factor Factor { get; set; }
        public Gate Gate { get; set; }
    }
}
