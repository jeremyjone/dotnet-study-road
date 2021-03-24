using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationAndAuthorization.EF.Models
{
    public class ApplicationRole : IdentityRole<int>
    {
        /// <summary>
        /// 有效
        /// </summary>
        public bool Validity { get; set; } = true;

        /// <summary>
        /// 启用
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
