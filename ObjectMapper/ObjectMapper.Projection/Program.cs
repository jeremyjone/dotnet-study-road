using System;
using AutoMapper;

namespace ObjectMapper.Projection
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");

            var source = new OuterSource
            {
                Value = 1,
                Inner = new InnerSource
                {
                    OtherValue = 111
                }
            };


            #region 嵌套映射

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<OuterSource, OuterDest>();
                cfg.CreateMap<InnerSource, InnerDest>();
            });

            var mapper = config.CreateMapper();
            var dest = mapper.Map<OuterSource, OuterDest>(source);
            Console.WriteLine($"{dest.Value}, {dest.Inner.OtherValue}");

            #endregion



            #region 展平

            var customer = new Customer
            {
                Name = "jeremyjone"
            };
            var order = new Order
            {
                Customer = customer
            };
            var product = new Product
            {
                Name = "product",
                Price = 4.99m
            };
            order.AddOrderLineItem(product, 15);

            var configuration = new MapperConfiguration(cfg => cfg.CreateMap<Order, OrderDto>());
            var mapper2 = configuration.CreateMapper();
            var orderDto = mapper2.Map<Order, OrderDto>(order);

            Console.WriteLine($"{orderDto.CustomerName}, {orderDto.Total}");

            #endregion



            #region 使用 IncludeMembers

            var config3 = new MapperConfiguration(cfg =>
            {
                // IncludeMembers 里面参数是有顺序的，映射规则按照先后顺序。先匹配到的优先
                cfg.CreateMap<MemberSource, MemberDestination>()
                    .IncludeMembers(s => s.MemberInnerSource, s => s.MemberOtherInnerSource).ReverseMap();
                cfg.CreateMap<MemberInnerSource, MemberDestination>(MemberList.None).ReverseMap();
                cfg.CreateMap<MemberOtherInnerSource, MemberDestination>().ReverseMap();
            });

            var source3 = new MemberSource
            {
                Name = "name",
                MemberInnerSource = new MemberInnerSource { Name = "inner name", Description = "description" },
                MemberOtherInnerSource = new MemberOtherInnerSource { Title = "title", Name = "other inner name", Description = "other inner desc"}
            };
            var mapper3 = config3.CreateMapper();
            var dest3 = mapper3.Map<MemberDestination>(source3);

            Console.WriteLine($"{dest3.Title}, {dest3.Name}, {dest3.Description}"); // title, name, description

            dest3.Name = "jjjjjj";
            mapper3.Map(dest3, source3);
            Console.WriteLine($"{source3.Name}"); // jjjjjj

            #endregion



            #region 泛型映射

            var config4 =
                new MapperConfiguration(cfg => cfg.CreateMap(typeof(GenericSource<>), typeof(GenericDestination<>)));

            var s1 = new GenericSource<int> { Value = 10 };
            var s2 = new GenericSource<string> {Value = "jeremyjone"};
            var mapper4 = config4.CreateMapper();
            var dest4_1 = mapper4.Map<GenericSource<int>, GenericDestination<int>>(s1);
            var dest4_2 = mapper4.Map<GenericSource<string>, GenericDestination<string>>(s2);
            Console.WriteLine($"{dest4_1.Value}, {dest4_2.Value}"); // 10, jeremyjone



            #endregion



            #region 条件映射

            var config5 = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Source, Destination>()
                    .ForMember(destination => destination.Value, opt => opt.Condition(src => src.Value >= 0));
            });

            var config6 = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Source, Destination>()
                    .ForMember(destination => destination.Value, opt =>
                    {
                        opt.PreCondition(src => src.Value >= 0);
                        // 使用 PreCondition 不要忘记添加真正的映射过程
                        opt.MapFrom(src => src.Value);
                    });
            });

            #endregion
        }
    }
}
