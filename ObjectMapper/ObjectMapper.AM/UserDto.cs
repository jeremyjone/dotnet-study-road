using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ObjectMapper.AM
{
    public class UserDto
    {
        /// <summary>
        /// 用户 Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 用户全名
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 用户年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 显示的登录时间
        /// </summary>
        public string LoginTime { get; set; }
    }
}
