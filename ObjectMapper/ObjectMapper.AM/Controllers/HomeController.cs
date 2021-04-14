using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace ObjectMapper.AM.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="mapper">在构造器中注入映射器</param>
        public HomeController(IMapper mapper)
        {
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var user = new User
            {
                Id = 1,
                LoginName = "jeremyjone",
                Nickname = "JeremyJone",
                FirstName = "Jeremy",
                MiddleName = "",
                LastName = "Jone",
                BirthDate = DateTime.Parse("2000-01-01"),
                Email = "jeremyjone@qq.com",
                LoginTime = DateTime.Parse("2021-4-10"),
                Password = "123456"
            };

            var userDto = _mapper.Map<UserDto>(user);

            return Ok(userDto);
        }
    }
}
