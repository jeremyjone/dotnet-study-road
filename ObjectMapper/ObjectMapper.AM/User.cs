using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ObjectMapper.AM
{
    public class User
    {
        /// <summary>
        /// 用户 Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 用户真实首名字
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// 用户真实中间名
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// 用户真实尾名字
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 用户邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 上次登录时间
        /// </summary>
        public DateTime LoginTime { get; set; }

        /// <summary>
        /// 用户生日
        /// </summary>
        public DateTime? BirthDate { get; set; }
    }
}
