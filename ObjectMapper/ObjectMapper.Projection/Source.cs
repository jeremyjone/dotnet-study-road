using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectMapper.Projection
{
    public class Source
    {
        public int Value { get; set; }
    }

    public class OuterSource
    {
        public int Value { get; set; }
        public InnerSource Inner { get; set; }
    }

    public class InnerSource
    {
        public int OtherValue { get; set; }
    }
}
