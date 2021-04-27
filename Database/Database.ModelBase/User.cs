using System.ComponentModel.DataAnnotations;

namespace Database.ModelBase
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
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 用户所在部门
        /// </summary>
        public int DepartmentId { get; set; }

        public Department Department { get; set; }
    }
}
