using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace AuthenticationAndAuthorization.IS4WithUI.Config
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiScope> GetApiScopes =>
            new[]
            {
                new ApiScope("api.jeremyjone.com", "Api.Jz"),
            };

        public static IEnumerable<Client> GetClients =>
            new[]
            {
                new Client
                {
                    ClientId = "jz",
                    ClientSecrets = {new Secret("www.jeremyjone.com".Sha256())},
                    AccessTokenLifetime = 3600,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes =
                    {
                        "api.jeremyjone.com"
                    }
                },

                new Client
                {
                    ClientId = "mvc_client",
                    ClientName = "MVC Client",
                    ClientSecrets = {new Secret("www.jeremyjone.com".Sha256())},
                    // 使用授权码
                    AllowedGrantTypes = GrantTypes.Code,
                    // 重定向，地址是客户端的地址
                    RedirectUris = {"http://localhost:33223/signin-oidc"},
                    PostLogoutRedirectUris = {"http://localhost:33223/signout-callback-oidc"},
                    // 用户的授权范围
                    AllowedScopes =
                    {
                        // 允许用户的授权
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        "api.jeremyjone.com"
                    },
                    // 是否需要用户同意（需要的话，会跳转到一个同意页面）
                    RequireConsent = true
                },
            };

        public static List<TestUser> GetUsers =>
        new List<TestUser>
        {
            new TestUser
            {
                SubjectId = "1",
                Username = "jeremyjone",
                Password = "123456"
            }
        };
    }
}
