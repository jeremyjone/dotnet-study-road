using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace AuthenticationAndAuthorization.CustomPolicy.Handlers
{
    /// <summary>
    /// 最小年龄策略要求类，它需要实现 IAuthorizationRequirement 接口
    /// </summary>
    public class MinimumAgeRequirement : IAuthorizationRequirement
    {
        public MinimumAgeRequirement(int minAge)
        {
            MinAge = minAge;
        }

        public int MinAge { get; }
    }

    /// <summary>
    /// 最小年龄的处理类，它需要实现 AuthorizationHandler<TRequirement> 抽象类
    /// </summary>
    public class MinimumAgeHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            MinimumAgeRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.DateOfBirth))
            {
                return Task.CompletedTask;
            }

            var dateOfBirth = context.User.FindFirstValue(ClaimTypes.DateOfBirth);
            if (string.IsNullOrWhiteSpace(dateOfBirth)) return Task.CompletedTask;
            
            var birth = Convert.ToDateTime(dateOfBirth);
            var age = DateTime.Today.Year - birth.Year;
            if (birth > DateTime.Today.AddYears(-age)) age--;

            if (age >= requirement.MinAge)
                // Succeed 方法是验证成功的必要语句
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
