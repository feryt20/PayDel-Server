using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using ZNetCS.AspNetCore.Logging.EntityFrameworkCore;

namespace PayDel.Data.Models
{
    public class ExtendedLog : Log
    {
        public ExtendedLog(IHttpContextAccessor http)
        {
            string browser = "";// http.HttpContext.Request.Headers["User-Agent"];
            if (!string.IsNullOrEmpty(browser) && (browser.Length > 255))
            {
                browser = browser.Substring(0, 255);
            }

            this.Browser = browser;
            this.Host = "";//http.HttpContext.Connection?.RemoteIpAddress.ToString();
            this.User = "";//http.HttpContext.User?.Identity.Name;
            this.Path = "";//http.HttpContext.Request.Path;

        }
        public ExtendedLog()
        {

        }

        public string Browser { get; set; }
        public string Host { get; set; }
        public string User { get; set; }
        public string Path { get; set; }
    }
}
