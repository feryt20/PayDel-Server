using System;
using System.Collections.Generic;
using System.Text;

namespace PayDel.Data.Dtos.Site.Users
{
    public class GateWalletsForReturnDto
    {
        public GateForReturnDto Gate { get; set; }
        public IEnumerable<WalletForReturnDto> Wallets { get; set; }
    }
}
