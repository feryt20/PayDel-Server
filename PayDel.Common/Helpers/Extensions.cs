using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http;

namespace PayDel.Common.Helpers
{
    public static class Extensions
    {
        public static void AddAppError(this HttpResponse httpResponse, string message)
        {
            httpResponse.Headers.Add("App-Error", message);
            httpResponse.Headers.Add("Access-Control-Expose-Header", "App-Error");
            httpResponse.Headers.Add("Access-Control-Allow-Origin", "*");
        }
    }
}
