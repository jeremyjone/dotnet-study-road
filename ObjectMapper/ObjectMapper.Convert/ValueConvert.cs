using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace ObjectMapper.Convert
{
    public class ValueConvertSource
    {
        public int Value1 { get; set; }
        public int Value2 { get; set; }
        public string Word { get; set; }
    }

    public class ValueConvertDestination
    {
        public int Total { get; set; }
        public int OtherValue { get; set; }
        public string Word { get; set; }
    }

    public class ValueConvertResolver : IValueResolver<ValueConvertSource, ValueConvertDestination, int>
    {
        public int Resolve(ValueConvertSource source, ValueConvertDestination destination, int member, ResolutionContext context)
        {
            return source.Value1 + source.Value2;
        }
    }
}
