// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace Jz.IdServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Address(),
                new IdentityResources.Phone(),
                //new IdentityResources.Email(), 
                //new IdentityResources.Profile(), 
                // 如果需要修改默认配置内容，可以通过自定义方式定义
                new IdentityResources.Email
                {
                    //Name = IdentityServerConstants.StandardScopes.Email,
                    DisplayName = "您的邮箱",
                    Description = "您的邮箱地址",
                    Required = true,
                    //Emphasize = true,
                    UserClaims = new List<string>{ IdentityServerConstants.StandardScopes.Email }
                },
                new IdentityResources.Profile
                {
                    //Name = IdentityServerConstants.StandardScopes.Profile,
                    DisplayName = "您的个人信息",
                    Description = "您的个人信息 (如 姓名，生日 等)",
                    //UserClaims = new List<string>
                    //{
                    //    JwtClaimTypes.Name,
                    //    JwtClaimTypes.FamilyName,
                    //    JwtClaimTypes.GivenName,
                    //    JwtClaimTypes.MiddleName,
                    //    JwtClaimTypes.NickName,
                    //    JwtClaimTypes.PreferredUserName,
                    //    JwtClaimTypes.Profile,
                    //    JwtClaimTypes.Picture,
                    //    JwtClaimTypes.WebSite,
                    //    JwtClaimTypes.Gender,
                    //    JwtClaimTypes.BirthDate,
                    //    JwtClaimTypes.ZoneInfo,
                    //    JwtClaimTypes.Locale,
                    //    JwtClaimTypes.UpdatedAt
                    //}
                },
                new IdentityResource("roles", "角色信息", new List<string> {JwtClaimTypes.Role})
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("mvc.read", "Mvc Client Read"),
                new ApiScope("mvc.create", "Mvc Client Create"),
                new ApiScope("mvc.update", "Mvc Client Update"),
                new ApiScope("mvc.delete", "Mvc Client Delete"),

                new ApiScope("api.read", "Api Client Read"),
                new ApiScope("api.create", "Api Client Create"),
                new ApiScope("api.update", "Api Client Update"),
                new ApiScope("api.delete", "Api Client Delete"),
            };

        // 定义资源。
        // 在 v4+ 中，Scope 与 Resource 分离，定义客户端时填写 scope 的名称即可。
        //
        // 资源的作用：对于需要进行资源认证（如：implicit）的客户端，需要将所对应的 Scope 配置到 Resource 中
        // 并且设置资源的秘钥（如下）。在客户端验证时使用该名称和秘钥即可。

        // 资源名称：配置客户端时只需要填写 Scope，框架会根据 Scope 自动添加资源名称，资源名称配置为 Audience（aud）
        // 遇到多个 Scope 的，会自动叠加 aud 名称。所以客户端验证时只需要配置 Audience = ApiName & Secret = ApiSecret 即可。
        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource("api", "Api Resource"){
                    ApiSecrets = { new Secret("api resource secret".Sha256()) },
                    Scopes = { "api.read", "api.create", "api.update", "api.delete" }
                },

                new ApiResource("mvc", "Mvc Resource"){
                    ApiSecrets = { new Secret("mvc resource secret".Sha256()) },
                    Scopes = { "mvc.read", "mvc.create", "mvc.update", "mvc.delete" }
                }
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // m2m client credentials flow client
                new Client
                {
                    ClientId = "m2m.client",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                    RequireConsent = true,
                    AllowedScopes = { "mvc.read", "mvc.create", "mvc.update", "mvc.delete" }
                },

                // interactive client using code flow + pkce
                new Client
                {
                    ClientId = "interactive",
                    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,

                    // 创建一个如下根地址的 Web 应用即可测试。也可以根据测试地址修改
                    RedirectUris = { "http://localhost:44300/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:44300/signout-callback-oidc" },

                    RequireConsent = true,
                    AllowOfflineAccess = true,
                    AllowedScopes = {
                        "mvc.read", "mvc.create",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                },

                // mix client
                new Client
                {
                    ClientId = "mix client",
                    ClientName = "混合客户端",
                    ClientSecrets = { new Secret("mix client secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,

                    RedirectUris = {"http://localhost:6500/siginin-oidc"},
                    FrontChannelLogoutUri = "http://localhost:6500/signout-oidc",
                    PostLogoutRedirectUris = {"http://localhost:6500/signout-callback-oidc"},

                    AlwaysIncludeUserClaimsInIdToken = true,
                    RequireConsent = true,
                    AllowOfflineAccess = true,
                    AccessTokenLifetime = 60,

                    AllowedScopes =
                    {
                        "mvc.read", "mvc.create", "mvc.update", "mvc.delete",
                        "api.read", "api.create", "api.update", "api.delete",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Phone,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                }, 

                // password grant client
                new Client
                {
                    ClientId = "pwd client",
                    ClientSecrets = { new Secret("pwd client secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes =
                    {
                        "mvc.read", "mvc.create", "mvc.update", "mvc.delete",
                        "api.read", "api.create", "api.update", "api.delete",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Address
                    }
                }, 

                // implicit flow
                new Client
                {
                    ClientId = "implicit client",
                    ClientName = "Implicit 客户端",
                    ClientUri = "http://localhost:8080",

                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = true,
                    AccessTokenLifetime = 60 * 5,
                    AlwaysIncludeUserClaimsInIdToken = true,

                    RedirectUris =
                    {
                        "http://localhost:8080/callback",
                        "http://localhost:8080/callback-refresh"
                    },

                    PostLogoutRedirectUris =
                    {
                        "http://localhost:8080/logout"
                    },

                    AllowedCorsOrigins =
                    {
                        "http://localhost:8080"
                    },

                    AllowedScopes =
                    {
                        "api.read", "api.create", "api.update", "api.delete", "mvc.delete",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email
                    }
                },

                // hybrid flow
                new Client
                {
                    ClientId = "hybrid client",
                    ClientName = "Hybrid 客户端",
                    ClientSecrets = {new Secret("hybrid client secret".Sha256())},

                    AllowedGrantTypes = GrantTypes.Hybrid,
                    AccessTokenType = AccessTokenType.Reference,
                    RequireConsent = true,
                    RequirePkce = false,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = {"http://localhost:7100/signin-oidc"},
                    PostLogoutRedirectUris = {"http://localhost:7100/signout-callback-oidc"},
                    AllowOfflineAccess = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowedScopes =
                    {
                        "mvc.read", "mvc.create", "mvc.update", "mvc.delete",
                        "api.read", "api.create", "api.update", "api.delete",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Phone,
                        IdentityServerConstants.StandardScopes.Profile,
                        "roles",
                    }
                },
            };
    }
}
