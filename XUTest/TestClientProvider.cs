using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using PayDel.Presentation;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace XUTest
{
    public class TestClientProvider
    {
        public HttpClient _client;
        public TestClientProvider()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());

            _client = server.CreateClient();

        }
    }
}
