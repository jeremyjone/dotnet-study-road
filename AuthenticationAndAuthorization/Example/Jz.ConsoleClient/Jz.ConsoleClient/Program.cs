using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace Jz.ConsoleClient
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // 客户端认证
            await Credentials();


            // 密码认证
            await Password();
        }


        private static async Task Credentials()
        {
            Console.WriteLine("Starting Credentials...");

            var client = new HttpClient();

            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            var res = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "m2m.client",
                ClientSecret = "511536EF-F270-4058-80CA-1C89C192F69A",
                Scope = "mvc.read mvc.create mvc.update mvc.delete"
            });
            if (res.IsError)
            {
                Console.WriteLine(res.Error);
                return;
            }

            Console.WriteLine("Get Access Token:");
            Console.WriteLine(res.AccessToken);

            // 有了 AccessToken，就可以做其他事情了
            // ...

            Console.WriteLine("\nCredentials Closed.\n\n");
        }


        private static async Task Password()
        {
            Console.WriteLine("Starting Password...");

            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            var res = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                UserName = "bob",
                Password = "bob",
                ClientId = "pwd client",
                ClientSecret = "pwd client secret",
                Scope = "openid profile email address mvc.read mvc.create mvc.update mvc.delete api.read api.create api.update api.delete"
            });
            if (res.IsError)
            {
                Console.WriteLine(res.Error);
                return;
            }

            Console.WriteLine("Get Response Json:");
            Console.WriteLine(res.Json);

            // 有了 AccessToken，就可以做其他事情了
            // ... res.AccessToken
            client.SetBearerToken(res.AccessToken);
            var userInfo = await client.GetAsync(disco.UserInfoEndpoint);
            if (userInfo.IsSuccessStatusCode)
            {
                Console.WriteLine("\n\nGet User Info:\n");
                Console.WriteLine(await userInfo.Content.ReadAsStringAsync());
            }

            Console.WriteLine("\nPassword Closed.");
        }
    }
}
