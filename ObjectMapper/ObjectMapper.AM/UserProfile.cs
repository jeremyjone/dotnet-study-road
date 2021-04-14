using System;
using AutoMapper;

namespace ObjectMapper.AM
{
    public class BirthValueConverter : IValueConverter<DateTime, int>
    {
        public int Convert(DateTime source, ResolutionContext context)
        {
            // 计算年龄
            return DateTime.Now.Year - source.Year;
        }
    }

    public class LoginTimeValueResolver: IValueResolver<User, UserDto, string>
    {
        public string Resolve(User source, UserDto destination, string destMember, ResolutionContext context)
        {
            return source.LoginTime.ToString("yyyy-MM-d dddd");
        }
    }

    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>()
                // 用户名是登录名
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.LoginName))
                // 用户名字是三个名字排列
                .ForMember(dest => dest.FullName,
                    opt => opt.MapFrom(src => $"{src.FirstName} {src.MiddleName} {src.LastName}"))
                // 通过生日计算年龄
                .ForMember(dest => dest.Age, opt =>
                {
                    opt.PreCondition(src => src.BirthDate != null);
                    opt.ConvertUsing(new BirthValueConverter(), src => src.BirthDate.GetValueOrDefault());
                })
                // 将登录时间转换为显示的字符串
                .ForMember(dest => dest.LoginTime, opt => opt.MapFrom<LoginTimeValueResolver>());
                ;

            CreateMap<User, UserEmailDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.LoginName));
        }
    }
}
