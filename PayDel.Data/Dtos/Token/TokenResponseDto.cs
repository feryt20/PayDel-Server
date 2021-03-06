﻿using PayDel.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PayDel.Data.Dtos.Token
{
    public class TokenResponseDto
    {
        public string token { get; set; }
        public string refresh_token { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
        public User user { get; set; }
    }
}
