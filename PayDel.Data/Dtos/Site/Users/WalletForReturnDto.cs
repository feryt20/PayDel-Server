using System;
using System.Collections.Generic;
using System.Text;

namespace PayDel.Data.Dtos.Site.Users
{
    public class WalletForReturnDto
    {
        public string Id { get; set; }
        public bool IsMain { get; set; }
        public bool IsSms { get; set; }
        public bool IsBlock { get; set; }


        public string Name { get; set; }

        public string Code { get; set; }

        public int Inventory { get; set; }

        public int InterMoney { get; set; }

        public int ExitMoney { get; set; }

        public int OnExitMoney { get; set; }
    }
}
