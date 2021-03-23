using System.Collections.Generic;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace AuthenticationAndAuthorization.IdentityServer4
{
    public static class Config
    {
        /// <summary>
        /// 配置认证资源信息
        /// </summary>
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                // 认证资源，OpenId 是必须要添加的
                new IdentityResources.OpenId(),
                // Profile 也是需要带上的
                new IdentityResources.Profile()
            };

        /// <summary>
        /// 配置 Api 信息
        /// </summary>
        public static IEnumerable<ApiScope> Apis =>
            new[]
            {
                new ApiScope("api.jeremyjone.com", "Jz.Api"), 
            };

        /// <summary>
        /// 配置客户端
        /// </summary>
        public static IEnumerable<Client> Clients =>
            new []
            {
                new Client
                {
                    // 客户端 Id
                    ClientId = "jz",
                    // 客户端获取 Token
                    ClientSecrets = new[] {new Secret("www.jeremyjone.com".Sha256()) },
                    // 使用客户端认证
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    // 允许访问的 Api
                    AllowedScopes = new[] { "api.jeremyjone.com" }
                }
            };

        /// <summary>
        /// 配置测试账户
        /// </summary>
        public static List<TestUser> Users =>
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
