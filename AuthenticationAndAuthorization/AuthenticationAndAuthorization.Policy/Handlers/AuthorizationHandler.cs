using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace AuthenticationAndAuthorization.Policy.Handlers
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

    /// <summary>
    /// 和上面方式一样，只不过使用的是接口继承。这个接口是所有授权操作的基础，在 Startup.cs 中注入方法时也是使用该接口
    /// 
    /// 与上面抽象类不同，接口不需要指定 Requirement
    /// 也是因为上面这一点，所以一般不要使用该方案。
    /// 所有授权都基于该接口，所以当请求通过管道时，每一个请求都会调用该接口下的策略方案进行匹配。
    /// 而使用上面的抽象类则只会针对 Requirement 类进行匹配，不是当前的类，就不需要匹配了。
    ///
    /// 测试：将两个类分别打断点，打开针对这两个方法实行的最小年龄的策略方案。当请求进入时，首先进入上面的方法1，然后进入这个方法2；
    /// 当打开一个普通的策略方案时，上面的方法1不会进入，而当前方法2还是会进入。
    /// </summary>
    public class MinimumAgeHandler2 : IAuthorizationHandler
    {
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            // 内部方法的调用大致都是一样的
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.DateOfBirth))
            {
                return Task.CompletedTask;
            }

            var dateOfBirth = context.User.FindFirstValue(ClaimTypes.DateOfBirth);
            if (string.IsNullOrWhiteSpace(dateOfBirth)) return Task.CompletedTask;

            var birth = Convert.ToDateTime(dateOfBirth);
            var age = DateTime.Today.Year - birth.Year;
            if (birth > DateTime.Today.AddYears(-age)) age--;

            // ※※※ 只有这里不一样 ※※※
            // 因为方法不提供第二个 requirement 参数，所以需要自行获取
            var requirement = context.Requirements.FirstOrDefault();
            if (requirement != null && 
                requirement.GetType() == typeof(MinimumAgeRequirement) && 
                age >= ((MinimumAgeRequirement)requirement).MinAge)
                // Succeed 方法是验证成功的必要语句
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
