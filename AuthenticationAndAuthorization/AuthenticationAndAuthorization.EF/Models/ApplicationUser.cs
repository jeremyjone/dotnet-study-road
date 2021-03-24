using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationAndAuthorization.EF.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        /// <summary>
        /// 有效
        /// </summary>
        public bool Validity { get; set; } = true;

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime BirthDate { get; set; }

        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
