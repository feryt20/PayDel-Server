using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace IdentityTestConsole
{
    class Program
    {
        public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        private static async Task MainAsync()
        {
            var client = new HttpClient();

            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5000");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "client",
                ClientSecret = "secret",
                Scope = "read"
            });

            //var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
            //var tokenResponse = await tokenClient.RequestClientCredentialsTokenAsync("invoices");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");


            var client2 = new HttpClient();
            client2.SetBearerToken(tokenResponse.AccessToken);
            var res = await client2.GetAsync("https://localhost:44318/v1/site/admin/Users");

            if (!res.IsSuccessStatusCode)
            {
                Console.WriteLine(res.StatusCode);
            }

            var content = await res.Content.ReadAsStringAsync();
            Console.WriteLine(content);


            Console.Read();
        }


    }
}
