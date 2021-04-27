using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Database.Basic
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Start...");

            using var db = new UserDbContext();

            #region 创建数据

            Console.WriteLine("创建数据");
            db.Add(new Department { Id = 1, Name = "IT" });
            db.Add(new User
            {
                Id = 1,
                DepartmentId = 1,
                Username = "jeremyjone",
                Nickname = "Jeremy Jone"
            });
            db.SaveChanges();
            Console.WriteLine("创建数据完成");

            #endregion


            #region 查询数据

            Console.WriteLine("查询数据");
            var user = db.Users.FirstOrDefault();
            if (user != null)
            {
                var department = db.Departments.FirstOrDefault(x => x.Id == user.DepartmentId);
                if (department == null)
                {
                    Console.WriteLine("部门为空");
                }
                Console.WriteLine($"读取到 {user.Username}，昵称为：{user.Nickname}，部门为：{department?.Name}");
            }
            else
            {
                Console.WriteLine("没有读取到用户信息");
            }

            #endregion


            #region 更新数据

            Console.WriteLine("更新数据");
            if (user != null)
            {
                user.Nickname = "Jz";
            }
            db.SaveChanges();
            Console.WriteLine("更新数据完成");

            #endregion


            #region 删除数据

            Console.WriteLine("删除数据");
            if (user != null)
            {
                db.Remove(user);
            }
            db.SaveChanges();
            Console.WriteLine("删除数据完成");

            #endregion
        }
    }
}
