using System;
using System.Reflection;
using AutoMapper;

namespace ObjectMapper.Convert
{
    public class TypeTypeConverter : ITypeConverter<string, Type>
    {
        public Type Convert(string source, Type destination, ResolutionContext context)
        {
            return Assembly.GetExecutingAssembly().GetType(source);
        }
    }

    public class DateTimeTypeConverter : ITypeConverter<string, DateTime>
    {
        public DateTime Convert(string source, DateTime destination, ResolutionContext context)
        {
            return System.Convert.ToDateTime(source);
        }
    }



    public class Source<T>
    {
        public T Value { get; set; }
    }

    public class Destination
    {
        public string Value { get; set; }
    }

    public class CustomValueConverter : IValueConverter<int, string>
    {
        public string Convert(int sourceMember, ResolutionContext context)
        {
            return sourceMember.ToString();
        }
    }
}
