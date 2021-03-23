using System;
using System.Threading.Tasks;
using AuthenticationAndAuthorization.CustomPolicy.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace AuthenticationAndAuthorization.CustomPolicy.Providers
{
    /// <summary>
    /// 提供最小年龄的授权服务，通过 IAuthorizationPolicyProvider 接口，实现 GetPolicyAsync 方法即可。
    /// </summary>
    internal class MinimumAgePolicyProvider: IAuthorizationPolicyProvider
    {
        private const string POLICY_PREFIX = "MinimumAge";

        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        public MinimumAgePolicyProvider(IOptions<AuthorizationOptions> options)
        {
            // 提供其他授权策略方案，这里使用默认方案
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        /// <summary>
        /// 获取策略并进行处理
        /// </summary>
        /// <param name="policyName"></param>
        /// <returns></returns>
        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            // 获取策略字符串并解析出年龄
            if (policyName.StartsWith(POLICY_PREFIX, StringComparison.OrdinalIgnoreCase) &&
                int.TryParse(policyName.Substring(POLICY_PREFIX.Length), out var age))
            {
                // 这里其实和 Startup.ConfigureService 中的配置差不多，都是添加一个 Requirement
                var policy = new AuthorizationPolicyBuilder();
                policy.AddRequirements(new MinimumAgeRequirement(age));
                return Task.FromResult(policy.Build());
            }

            //return Task.FromResult<AuthorizationPolicy>(null);
            // 当上述授权流程出现问题（例如获取不到年龄），则使用该备选方案
            return FallbackPolicyProvider.GetPolicyAsync(policyName);
        }

        //public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<AuthorizationPolicy> GetFallbackPolicyAsync()
        //{
        //    throw new NotImplementedException();
        //}

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();

        public Task<AuthorizationPolicy> GetFallbackPolicyAsync() => FallbackPolicyProvider.GetFallbackPolicyAsync();
    }
}
