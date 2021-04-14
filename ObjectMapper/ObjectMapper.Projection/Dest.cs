using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectMapper.Projection
{
    public class Destination
    {
        public int Value { get; set; }
    }

    public class OuterDest
    {
        public int Value { get; set; }
        public InnerDest Inner { get; set; }
    }

    public class InnerDest
    {
        public int OtherValue { get; set; }
    }
}
