using System;
using System.Collections.Generic;
using System.Text;

namespace PayDel.Data.Dtos.Site.Users
{
    public class GatesWalletsForReturnDto
    {
        public IEnumerable<GateForReturnDto> Gates { get; set; }
        public IEnumerable<WalletForReturnDto> Wallets { get; set; }
    }
}
