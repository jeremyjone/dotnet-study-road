using Microsoft.AspNetCore.Authorization;

namespace AuthenticationAndAuthorization.CustomPolicy.Attributes
{
    /// <summary>
    /// 通过将参数映射到一个字符串，用于检索相应的授权策略。
    /// </summary>
    internal class MinimumAgeAuthorizeAttribute : AuthorizeAttribute
    {
        private const string POLICY_PREFIX = "MinimumAge";
        public MinimumAgeAuthorizeAttribute(int age) => Age = age;

        public int Age
        {
            get => int.TryParse(Policy.Substring(POLICY_PREFIX.Length), out var age) ? age : default;
            set => Policy = $"{POLICY_PREFIX}{value.ToString()}";
        }
    }
}
