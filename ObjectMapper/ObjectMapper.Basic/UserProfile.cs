using AutoMapper;

namespace ObjectMapper.Basic
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<User, UserEmailDto>();
        }
    }
}
