using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PayDel.Data.Dtos
{
    public class ApiReturn<T>
    {
        [Description("وضعیت درخواست")]
        public bool Status { get; set; }
        [Description("پیام درخواست")]
        public string Message { get; set; }

        [JsonProperty(NullValueHandling =NullValueHandling.Ignore)]
        [Description("نتیجه درخواست")]
        public T Result { get; set; }
    }
}
