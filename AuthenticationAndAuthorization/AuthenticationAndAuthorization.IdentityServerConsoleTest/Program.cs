using System;
using System.Net.Http;
using IdentityModel.Client;

namespace AuthenticationAndAuthorization.IdentityServerConsoleTest
{
    public class Program
    {
        public static async void GetToken()
        {
            var client = new HttpClient();
            var discovery = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
            if (discovery.IsError)
            {
                Console.WriteLine("ERR: " + discovery.Error);
            }

            #region 使用客户端认证方式

            var resp = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discovery.TokenEndpoint,
                ClientId = "jz",
                ClientSecret = "www.jeremyjone.com",
            });

            #endregion

            #region 使用密码认证方式

            var resp2 = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = discovery.TokenEndpoint,
                ClientId = "jz",
                ClientSecret = "www.jeremyjone.com",
                UserName = "jeremyjone",
                Password = "123456"
            });

            #endregion

            if (resp.IsError)
            {
                Console.WriteLine("ERR: " + resp.Error);
            }

            Console.WriteLine(resp.Json);
        }

        public static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            GetToken();

            Console.Read();
        }
    }
}
