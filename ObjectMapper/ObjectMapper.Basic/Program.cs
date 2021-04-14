using System;
using AutoMapper;

namespace ObjectMapper.Basic
{
    public class Program
    {
        public static void Main(string[] args)
        {
            #region AutoMapper 的基本使用

            // 所有映射都可以匹配到，通过验证
            var config1 = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDto>();
                cfg.CreateMap<User, UserEmailDto>();
            });
            config1.AssertConfigurationIsValid();

            var user = new User
            {
                Id = 1,
                Username = "jeremyjone",
                Nickname = "Jeremy Jone",
                Email = "jeremyjone@qq.com",
                Password = "123456"
            };

            var mapper = new Mapper(config1);
            var userDto = mapper.Map<UserDto>(user);

            Console.WriteLine($"{userDto.Username}, {userDto.Nickname}");

            #endregion

            #region 错误的示例

            // 在 ErrorUserDto 中有一个源数据没有的 ErrorAttr 属性，且配置中没有对应如何映射该属性，验证不通过
            //var config2 = new MapperConfiguration(cfg => cfg.CreateMap<User, ErrorUserDto>());
            //config2.AssertConfigurationIsValid();

            #endregion

            #region 使用配置文件进行配置

            var config3 = new MapperConfiguration(cfg => cfg.AddProfile<UserProfile>());
            // or
            //var config3 = new MapperConfiguration(cfg => cfg.AddProfile(new UserProfile()));

            config3.AssertConfigurationIsValid();

            // config.CreateMapper 与 new Mapper(config) 等效
            var mapper3 = config3.CreateMapper();
            var userDto3 = mapper3.Map<UserDto>(user);

            Console.WriteLine($"{userDto3.Username}, {userDto3.Nickname}");

            #endregion

            #region 程序集自动扫描配置文件

            var config4 = new MapperConfiguration(cfg => cfg.AddMaps(typeof(Program)));
            // or 动态获取
            //var config4 = new MapperConfiguration(cfg => cfg.AddMaps("ObjectMapper.Basic"));
            // or 数组形式
            //var config4 = new MapperConfiguration(cfg => cfg.AddMaps(new[] {typeof(Program)}));
            var mapper4 = config4.CreateMapper();
            var userDto4 = mapper4.Map<UserDto>(user);

            Console.WriteLine($"{userDto4.Username}, {userDto4.Nickname}");

            #endregion
        }
    }
}
