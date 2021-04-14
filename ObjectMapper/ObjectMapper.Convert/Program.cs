using System;
using AutoMapper;

namespace ObjectMapper.Convert
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");


            #region 类型转换

            var config1 = new MapperConfiguration(cfg =>
            {
                // 简单类型转换
                cfg.CreateMap<string, int>().ConvertUsing(src => System.Convert.ToInt32(src));

                // 自定义（复杂）类型转换
                // 传入自定义转换器的两种方式
                cfg.CreateMap<string, DateTime>().ConvertUsing(new DateTimeTypeConverter());
                cfg.CreateMap<string, Type>().ConvertUsing<TypeTypeConverter>();
                
                // 值的类型转换器
                cfg.CreateMap<Source<int>, Destination>()
                    .ForMember(dest => dest.Value, opt => opt.ConvertUsing(new CustomValueConverter()));
            });

            var source1 = new Source<int>{Value = 1};
            var dest1 = config1.CreateMapper().Map<Destination>(source1);
            Console.WriteLine($"{dest1.Value}");

            #endregion


            #region 值转换器

            var config2 = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ValueConvertSource, ValueConvertDestination>()
                    .ForMember(dest => dest.Total,
                        opt => opt.MapFrom<ValueConvertResolver>());
            });

            var source2 = new ValueConvertSource
            {
                Value1 = 5,
                Value2 = 7
            };
            var mapper2 = config2.CreateMapper();
            var dest2 = mapper2.Map<ValueConvertSource, ValueConvertDestination>(source2);
            Console.WriteLine($"{dest2.Total}"); // 12

            #endregion


            #region 上下文获取键值对象

            var config3 = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ValueConvertSource, ValueConvertDestination>()
                    .ForMember(dest => dest.Total,
                        opt => opt.MapFrom<ValueConvertResolver>())
                    .ForMember(dest => dest.OtherValue,
                        opt => opt.MapFrom((src, dest, destMember, context) => context.Items["Value"]));
            });

            var source3 = new ValueConvertSource
            {
                Value1 = 5,
                Value2 = 7
            };
            var mapper3 = config3.CreateMapper();
            var dest3 = mapper3.Map<ValueConvertSource, ValueConvertDestination>(source3, opt => opt.Items["Value"] = 10);
            Console.WriteLine($"{dest3.Total}, {dest3.OtherValue}"); // 12, 10

            #endregion


            #region 修改值的内容

            var config4 = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Source<string>, Destination>();

                // 全局
                cfg.ValueTransformers.Add<string>(src => src + " World");

                cfg.CreateMap<ValueConvertSource, ValueConvertDestination>()
                    // 局部
                    .ForMember(dest=>dest.Word, opt => opt.AddTransform(src=>src + " World!"));
            });

            var source4 = new ValueConvertSource
            {
                Word = "Hello"
            };
            var source5 = new Source<string> { Value = "Hello"};
            var mapper4 = config4.CreateMapper();
            var dest4 = mapper4.Map<ValueConvertSource, ValueConvertDestination>(source4);
            var dest5 = mapper4.Map<Destination>(source5);
            Console.WriteLine($"{dest4.Word}, {dest5.Value}"); // Hello World! World, Hello World

            #endregion
        }
    }
}
