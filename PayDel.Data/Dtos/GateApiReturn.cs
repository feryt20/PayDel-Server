using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PayDel.Data.Dtos
{
    public class GateApiReturn<T>
    {
        [Description("وضعیت درخواست ارسال شده")]
        public bool Status { get; set; }
        [Description("پیغام سرور به درخواست ارسال شده")]
        public string[] Messages { get; set; }
        [Description("نتیجه ی درخواست ارسال شده")]
        public T Result { get; set; }
    }
}
