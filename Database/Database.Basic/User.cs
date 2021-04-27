using System;

namespace Database.Basic
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
        public string Username { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 用户所在部门
        /// </summary>
        public int DepartmentId { get; set; }
    }
}
