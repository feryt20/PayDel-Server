﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PayDel.Common.Helpers
{
    public class TokenSetting
    {
        public string Site { get; set; }
        public string Secret { get; set; }
        public string ExpireDate { get; set; }
        public string Audience { get; set; }
        public string SendUser { get; set; }
        public string SendKey { get; set; }
    }
}
