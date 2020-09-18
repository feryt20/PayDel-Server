using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PayDel.Data.Dtos.Api
{
    public class PayResponseDto
    {
        [Description("توکن پرداخت")]
        public string Token { get; set; }
        [Description("آدرسی که برای پرداخت باید به آن ریدایرکت کنید")]
        public string RedirectUrl { get; set; }
    }
}
